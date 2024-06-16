using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// public = VarX
/// private = varX
/// function = _VarX

public class PlayerComboM : MonoBehaviour
{
	[Header("Weapon Settings:")]
	[SerializeField] WeaponsSO punches;
	[SerializeField] string attackAnimationName;

	[Tooltip("The angle that the player sees in fron of him")]
	[SerializeField] float fieldOfView;
	[SerializeField] float animTime = 0.3f;

	[Tooltip("the time it waits after an attack")]
	[SerializeField] float AttackDelay = 0.2f;

	[Tooltip("the time it waits after a combo \nif you don't want it make it 0")]
	[SerializeField] float ComboDelay = 0.5f;

	[Tooltip("the time it waits before resetting the combo \nex: a = attack w= wait \nif comboReset = 3 sec then: \nif a1 w4 then it goes to a1 \nbut if a1 w1, then it goes to a2")]
	[SerializeField] float ComboResetTime = 1f;
	[SerializeField] WeaponsSO currentWeapon;
	[SerializeField] Transform meshHolder;
	float lastAttackTime = 0, lastComboTime;
	int comboCounter;
	Coroutine resetCombo;
	Animator anim;
	ActionMap inputs;
	float animStopTime = 0.9f;
	//bool canAttack = true;


	private void Awake()
	{
		inputs = InputManager.ActionMap;
	}

	private void OnDisable()
	{
		if (currentWeapon.IsRanged)
		{
			inputs.Player.Attack.started -= RangedAttack;
		}
		else
		{
			inputs.Player.Attack.started -= MeleeAttack;
		}
		currentWeapon.OnBreak -= BackToPunches;
	}

	private void Start()
	{
		UpdateCurrentWeapon(punches);
		anim = GetComponentInChildren<Animator>();
	}

	private void Update()
	{
		EndAttack();
	}

	void BackToPunches()
	{
		UpdateCurrentWeapon(punches);
	}

	public void UpdateCurrentWeapon(WeaponsSO _NewWeapon)
	{
		if (currentWeapon != null)
		{
			if (currentWeapon.IsRanged)
			{
				inputs.Player.Attack.started -= RangedAttack;
			}
			else
			{
				inputs.Player.Attack.started -= MeleeAttack;
			}
			currentWeapon.OnBreak -= BackToPunches;
		}

		currentWeapon = _NewWeapon;
		comboCounter = 0;
		lastComboTime = 0;
		lastAttackTime = 0;

		if (currentWeapon.IsRanged)
		{
			inputs.Player.Attack.started += RangedAttack;
			animStopTime = 0.8f;
		}
		else
		{
			inputs.Player.Attack.started += MeleeAttack;
			animStopTime = 0.9f;
		}
		currentWeapon.OnBreak += BackToPunches;
	}

	private void RangedAttack(InputAction.CallbackContext _Context)
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
		Debug.Log(_target.IsUnityNull());
		if (_target != null)
		{
			currentWeapon.OnAttack?.Invoke(currentWeapon.AttackCombo[comboCounter].Dmg);
			currentWeapon.GetTarget?.Invoke(_target);
			anim.runtimeAnimatorController = currentWeapon.AttackCombo[comboCounter].AnimOverrider;
			anim.Play(attackAnimationName);
		}
	}

	void MeleeAttack(InputAction.CallbackContext _Context)
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

	void EndAttack()
	{
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > animStopTime && anim.GetCurrentAnimatorStateInfo(0).IsName(attackAnimationName))
		{
			//canAttack = true;

			//  if resetCombo == null then:
			resetCombo ??= StartCoroutine(EndCombo());

			currentWeapon.AttackEnd?.Invoke();
		}
	}

	IEnumerator EndCombo()
	{
		yield return new WaitForSeconds(ComboResetTime);
		//Debug.Log("we have waited");
		lastComboTime = Time.time;
		comboCounter = 0;
	}

	// #if UNITY_EDITOR
	// 	private void OnDrawGizmos()
	// 	{

	// 	}
	// #endif

}

