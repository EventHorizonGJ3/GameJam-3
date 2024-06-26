using System.Collections;
using UnityEngine;

public class Combo : MonoBehaviour
{
    [Header("Weapon Settings:")]
    [SerializeField] public Weapon defaultWeapon;
    [SerializeField] protected string attackAnimationName;

    [Tooltip("The angle that the player sees in fron of him")]
    [SerializeField] protected float fieldOfView;
    [SerializeField] protected float animTime = 0.3f;

    [Tooltip("the time it waits after an attack")]
    [SerializeField] protected float AttackDelay = 0.2f;

    [Tooltip("the time it waits after a combo \nif you don't want it make it 0")]
    [SerializeField] protected float ComboDelay = 0.5f;

    [Tooltip("the time it waits before resetting the combo \nex: a = attack w= wait \nif comboReset = 3 sec then: \nif a1 w4 then it goes to a1 \nbut if a1 w1, then it goes to a2")]
    [SerializeField] protected float ComboResetTime = 1f;
    [SerializeField] protected Transform meshHolder;
    [SerializeField] protected Transform weaponHolder;
    [SerializeField] public Weapon currentWeapon;
    protected float lastAttackTime = 0, lastComboTime;
    protected int comboCounter;
    protected Coroutine resetCombo;
    protected Animator anim;
    [SerializeField] protected float animStopTime = 0.9f;
    protected float multDmg;
    protected Coroutine attackEnd;




    protected virtual void OnEnable()
    {

        this.UpdateCurrentWeapon(defaultWeapon);

    }
    protected virtual void OnDisable()
    {
        this.currentWeapon.Break -= BackToPunches;
        this.currentWeapon = null;
    }

    protected virtual void Start()
    {
        this.anim = GetComponentInChildren<Animator>();
    }

    public virtual void UpdateCurrentWeapon(Weapon _NewWeapon)
    {
        if (this.currentWeapon != null)
        {
            if (this.currentWeapon.WeaponSo.IsRanged)
            {
                this.currentWeapon.StartAttack -= this.RangedAttack;
            }
            else
            {
                this.currentWeapon.StartAttack -= this.MeleeAttack;
            }
            this.currentWeapon.Break -= this.BackToPunches;
        }

        this.currentWeapon = _NewWeapon;
        this.currentWeapon.Inizialize?.Invoke();
        this.comboCounter = 0;
        this.lastComboTime = 0;
        this.lastAttackTime = 0;

        if (this.currentWeapon.WeaponSo.IsRanged)
        {
            this.currentWeapon.StartAttack += this.RangedAttack;

        }
        else
        {
            this.currentWeapon.StartAttack += this.MeleeAttack;

        }
        this.currentWeapon.Break += this.BackToPunches;
    }

    protected virtual void BackToPunches()
    {
        this.UpdateCurrentWeapon(defaultWeapon);
    }

    protected virtual void RangedAttack()
    {
        bool hasEnemy = true;
        Collider[] _Colliders = new Collider[10];
        var _NumberOfColl = Physics.OverlapSphereNonAlloc(this.transform.position, this.currentWeapon.WeaponSo.RangedRange, _Colliders, this.currentWeapon.WeaponSo.EnemyLayer);
        if (_NumberOfColl <= 0)
        {
            hasEnemy = false;
            _NumberOfColl = Physics.OverlapSphereNonAlloc(this.transform.position, this.currentWeapon.WeaponSo.RangedRange, _Colliders);
        }
        var _minDistance = Mathf.Infinity;
        Transform _target = null;
        for (int i = 0; i < _NumberOfColl; i++)
        {
            var _coll = _Colliders[i];

            if (_coll.TryGetComponent(out IDamageable _) || hasEnemy == true)
            {
                var _Dir = (_coll.transform.position - transform.position).normalized;
                if (Vector3.Dot(_Dir, this.meshHolder.forward) >= this.fieldOfView)
                {
                    var _dist = Vector3.Distance(this.transform.position, _coll.transform.position);
                    if (_dist <= _minDistance)
                    {
                        _target = _coll.transform;
                        _minDistance = _dist;
                    }
                }
            }
        }

        if (_target != null)
        {
            this.currentWeapon.Target?.Invoke(_target);
            this.currentWeapon.Attack?.Invoke(Damage());
            
            this.anim.runtimeAnimatorController = this.currentWeapon.WeaponSo.AttackCombo[this.comboCounter].AnimOverrider;
            this.anim.Play(attackAnimationName);
        }
    }

    protected virtual void MeleeAttack()
    {

        if (Time.time - this.lastComboTime < this.ComboDelay || Time.time - this.lastAttackTime < this.AttackDelay || this.comboCounter > this.currentWeapon.WeaponSo.AttackCombo.Count)
            return;

        this.currentWeapon.Attack?.Invoke(Damage());

        if (this.resetCombo != null)
        {
            this.StopCoroutine(resetCombo);
            this.resetCombo = null;
        }

        if (this.attackEnd != null)
        {
            this.StopCoroutine(attackEnd);
            this.attackEnd = null;
        }
        attackEnd = StartCoroutine(EndAttack());

        if (this.comboCounter - 1 >= 0)
        {
            if (this.currentWeapon.WeaponSo.AttackCombo[this.comboCounter].AnimOverrider == this.currentWeapon.WeaponSo.AttackCombo[this.comboCounter - 1].AnimOverrider)
                this.anim.Play("Idle");
        }
        // animation: 
        this.anim.runtimeAnimatorController = this.currentWeapon.WeaponSo.AttackCombo[this.comboCounter].AnimOverrider;
        this.anim.Play(this.attackAnimationName);

        this.lastAttackTime = Time.time;

        this.comboCounter++;

        if (this.comboCounter >= this.currentWeapon.WeaponSo.AttackCombo.Count)
        {
            // ("last attack: " + comboCounter);

            this.currentWeapon.LastAttack?.Invoke();
            this.lastComboTime = Time.time;
            this.lastAttackTime = 0;
            this.comboCounter = 0;
        }
    }

    protected virtual IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(currentWeapon.WeaponSo.AttackCombo[comboCounter].clipLenght);

        Debug.Log("AttackEnded");

        if (this.resetCombo == null)
            this.resetCombo = StartCoroutine(EndCombo());

        this.currentWeapon.AttackEnd?.Invoke();
    }

    protected virtual IEnumerator EndCombo()
    {
        yield return new WaitForSeconds(this.ComboResetTime);
        this.lastComboTime = Time.time;
        this.comboCounter = 0;
    }

    protected virtual float Damage()
    {
        if (multDmg != 0)
            return this.currentWeapon.WeaponSo.AttackCombo[this.comboCounter].Dmg * this.multDmg;
        else
            return this.currentWeapon.WeaponSo.AttackCombo[this.comboCounter].Dmg;
    }


    // #if UNITY_EDITOR
    // 	private void OnDrawGizmos()
    // 	{

    // 	}
    // #endif
}