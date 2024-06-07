using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// public = VarX
/// private = varX
/// function = _VarX

public class PlayerComboM : MonoBehaviour
{
	[SerializeField] WeaponsSO currentWeapon;
	[SerializeField] WeaponsSO punches;
	[SerializeField] string attackAnimationName;

	public float AttackDelay = 0.2f;
	public float ComboResetTime = 2f;

	float lastAttackTime;
	float lastCombotime;
	int comboCounter;
	IEnumerator resetCombo;
	Animator anim;

	private void OnEnable()
	{
		InputManager.ActionMap.Player.Attack.performed += StartAttack;
	}
	private void Start()
	{
		UpdateCurrentWeapon(punches);
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		EndAttack();
	}

	void UpdateCurrentWeapon(WeaponsSO _NewWeapon)
	{
		currentWeapon = _NewWeapon;
	}

	void StartAttack(InputAction.CallbackContext _Context)
	{
		if (resetCombo != null)
			StopCoroutine(resetCombo);

		if (Time.time - lastAttackTime > AttackDelay)
		{
			lastAttackTime = Time.time;

			anim.runtimeAnimatorController = currentWeapon.AttackCombo[comboCounter].AnimOverrider;
			anim.Play(attackAnimationName);



			if (comboCounter + 1 >= currentWeapon.AttackCombo.Count)
			{
				currentWeapon.LastAttack(currentWeapon.AttackCombo[comboCounter].Dmg);
				comboCounter = 0;
			}
			else
			{
				currentWeapon.OnAttack?.Invoke(currentWeapon.AttackCombo[comboCounter].Dmg);
				comboCounter++;
			}
		}
	}

	void EndAttack()
	{
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName(attackAnimationName))
		{
			resetCombo = EndCombo();
			StartCoroutine(resetCombo);
		}
	}

	IEnumerator EndCombo()
	{
		yield return new WaitForSeconds(ComboResetTime);
		comboCounter = 0;
	}
}
