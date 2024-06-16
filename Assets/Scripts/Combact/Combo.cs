using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Combo : MonoBehaviour
{
	[Header("Weapon Settings:")]
	[SerializeField] protected WeaponsSO punches;
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
	[SerializeField] protected WeaponsSO currentWeapon;
	[SerializeField] protected Transform meshHolder;
	protected float lastAttackTime = 0, lastComboTime;
	protected int comboCounter;
	protected Coroutine resetCombo;
	protected Animator anim;
	protected float animStopTime = 0.9f;

	protected virtual void OnDisable()
	{
		currentWeapon.OnBreak -= BackToPunches;
	}

	protected virtual void Start()
	{
		UpdateCurrentWeapon(punches);
		anim = GetComponentInChildren<Animator>();
	}

	protected virtual void Update()
	{
		EndAttack();
	}

	protected virtual void BackToPunches()
	{
		UpdateCurrentWeapon(punches);
	}

	public virtual void UpdateCurrentWeapon(WeaponsSO _NewWeapon)
	{
		if (currentWeapon != null)
		{
			if (currentWeapon.IsRanged)
			{
				currentWeapon.StartAttack -= RangedAttack;
			}
			else
			{
				currentWeapon.StartAttack -= MeleeAttack;
			}
			currentWeapon.OnBreak -= BackToPunches;
		}

		currentWeapon = _NewWeapon;
		comboCounter = 0;
		lastComboTime = 0;
		lastAttackTime = 0;

		if (currentWeapon.IsRanged)
		{
			currentWeapon.StartAttack += RangedAttack;
			animStopTime = 0.7f;
		}
		else
		{
			currentWeapon.StartAttack += MeleeAttack;
			animStopTime = 0.9f;
		}
		currentWeapon.OnBreak += BackToPunches;
	}

	protected virtual void RangedAttack()
	{
		Collider[] _Colliders = new Collider[10];
		var _NumberOfColl = Physics.OverlapSphereNonAlloc(transform.position, currentWeapon.RangedRange, _Colliders, currentWeapon.EnemyLayer);
		var _minDistance = Mathf.Infinity;
		Transform _target = null;
		for (int i = 0; i < _NumberOfColl; i++)
		{
			var _coll = _Colliders[i];
			var _Dir = (_coll.transform.position - transform.position).normalized;
			if (Vector3.Dot(_Dir, meshHolder.forward) >= fieldOfView)
			{
				var _dist = Vector3.Distance(transform.position, _coll.transform.position);
				if (_dist <= _minDistance)
				{
					_target = _coll.transform;
					_minDistance = _dist;
				}
			}
		}
		if (_target != null)
		{
			currentWeapon.OnAttack?.Invoke(currentWeapon.AttackCombo[comboCounter].Dmg);
			currentWeapon.GetTarget?.Invoke(_target);
			anim.runtimeAnimatorController = currentWeapon.AttackCombo[comboCounter].AnimOverrider;
			anim.Play(attackAnimationName);
		}
	}

	protected virtual void MeleeAttack()
	{
		if (resetCombo != null)
		{
			StopAllCoroutines();
			resetCombo = null;
		}

		if (Time.time - lastComboTime < ComboDelay || Time.time - lastAttackTime < AttackDelay || comboCounter > currentWeapon.AttackCombo.Count)
			return;

		if (comboCounter - 1 >= 0)
		{
			if (currentWeapon.AttackCombo[comboCounter].AnimOverrider == currentWeapon.AttackCombo[comboCounter - 1].AnimOverrider)
				anim.Play("IdleHand");
		}

		// animation: 
		anim.runtimeAnimatorController = currentWeapon.AttackCombo[comboCounter].AnimOverrider;
		anim.Play(attackAnimationName);
		currentWeapon.OnAttack?.Invoke(currentWeapon.AttackCombo[comboCounter].Dmg);

		lastAttackTime = Time.time;

		comboCounter++;

		if (comboCounter >= currentWeapon.AttackCombo.Count)
		{
			//Debug.Log("last attack: " + comboCounter);
			if (resetCombo != null)
			{
				StopAllCoroutines();
				resetCombo = null;
			}
			currentWeapon.LastAttack?.Invoke();
			lastComboTime = Time.time;
			comboCounter = 0;
		}
	}

	protected virtual void EndAttack()
	{
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > animStopTime && anim.GetCurrentAnimatorStateInfo(0).IsName(attackAnimationName))
		{
			//  if resetCombo == null then:
			resetCombo ??= StartCoroutine(EndCombo());
			currentWeapon.AttackEnd?.Invoke();
		}
	}

	protected virtual IEnumerator EndCombo()
	{
		yield return new WaitForSeconds(ComboResetTime);
		lastComboTime = Time.time;
		comboCounter = 0;
	}

	// #if UNITY_EDITOR
	// 	private void OnDrawGizmos()
	// 	{

	// 	}
	// #endif
}