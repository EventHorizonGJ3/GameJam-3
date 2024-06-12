using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// public = VarX
/// private = varX
/// function = _VarX

public class PlayerComboM : MonoBehaviour
{
	[Header("Weapon Settings:")]
	[SerializeField] WeaponsSO currentWeapon;
	[SerializeField] WeaponsSO punches;
	[SerializeField] string attackAnimationName;

	[Tooltip("The angle that the player sees in fron of him")]
	[SerializeField] float fieldOfView;

	[Tooltip("the time it waits after an attack")]
	public float AttackDelay = 0.2f;

	[Tooltip("the time it waits after a combo \nif you don't want it make it 0")]
	public float ComboDelay = 0.5f;

	[Tooltip("the time it waits before resetting the combo \nex: a = attack w= wait \nif comboReset = 3 sec then: \nif a1 w4 then it goes to a1 \nbut if a1 w1, then it goes to a2")]
	public float ComboResetTime = 1f;
	float lastAttackTime = 0, lastComboTime;
	int comboCounter;
	Coroutine resetCombo;
	Animator anim;
	ActionMap inputs;
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
		}
		else
		{
			inputs.Player.Attack.started += MeleeAttack;
		}
		currentWeapon.OnBreak += BackToPunches;
	}

	private void RangedAttack(InputAction.CallbackContext _Context)
	{
		var _colliders = Physics.OverlapSphere(transform.position, currentWeapon.Range);
		var _minDistance = 999f;
		Transform _target = null;
		foreach (var _coll in _colliders)
		{
			if (Vector3.Dot(_coll.transform.position, transform.position) < fieldOfView)
			{
				var _dist = Vector3.Distance(transform.position, _coll.transform.position);
				if (_dist < _minDistance)
				{
					_target = _coll.transform;
					_minDistance = _dist;
				}
			}
		}

		MeleeAttack(_Context);

		if (_target != null)
			currentWeapon.GetTarget?.Invoke(_target);
	}

	void MeleeAttack(InputAction.CallbackContext _Context)
	{
		if (resetCombo != null)
		{
			StopAllCoroutines();
			resetCombo = null;
		}

		if (Time.time - lastComboTime < ComboDelay || Time.time - lastAttackTime < AttackDelay || comboCounter > currentWeapon.AttackCombo.Count/*|| canAttack == false*/)
			return;

		lastAttackTime = Time.time;
		currentWeapon.OnAttack?.Invoke(currentWeapon.AttackCombo[comboCounter].Dmg);
		//canAttack = false;

		if (comboCounter - 1 >= 0)
		{
			if (currentWeapon.AttackCombo[comboCounter].AnimOverrider == currentWeapon.AttackCombo[comboCounter - 1].AnimOverrider)
				anim.Play("IdleHand");
		} 

		// animation: 
		anim.runtimeAnimatorController = currentWeapon.AttackCombo[comboCounter].AnimOverrider;
		anim.Play(attackAnimationName);

		if (comboCounter == currentWeapon.AttackCombo.Count - 1)
		{
			currentWeapon.LastAttack?.Invoke();
		}

		comboCounter++;

		if (comboCounter >= currentWeapon.AttackCombo.Count)
			comboCounter = 0;
	}

	void EndAttack()
	{
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName(attackAnimationName))
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
		Debug.Log("we have waited");
		lastComboTime = Time.time;
		comboCounter = 0;
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		if (currentWeapon != null)
		{
			Gizmos.DrawWireSphere(transform.position, currentWeapon.Range);
		}
		else
		{
			Gizmos.DrawWireSphere(transform.position, punches.Range);
		}
	}
#endif
}

