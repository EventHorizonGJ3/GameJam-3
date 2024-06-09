using System.Collections;
using Unity.VisualScripting;
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

	[Tooltip("the time it waits after the animation ended")]
	public float AttackDelay = 0.2f;

	[Tooltip("the time it waits for the next input before resetting the combo")]
	public float ComboResetTime = 1f;
	float lastAttackTime = 0;
	float currentAttackDelay, currentComboResetTime;
	int comboCounter;
	Coroutine resetCombo;
	Animator anim;
	ActionMap inputs;

	private void Awake()
	{
		inputs = InputManager.ActionMap;
	}
	private void OnEnable()
	{
		inputs.Player.Attack.started += StartAttack;
	}
	private void OnDisable()
	{
		inputs.Player.Attack.started -= StartAttack;
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

	public void UpdateCurrentWeapon(WeaponsSO _NewWeapon)
	{
		currentWeapon = _NewWeapon;
		Debug.Log("currentWeapon = " + currentWeapon.name);
	}

	void StartAttack(InputAction.CallbackContext _Context)
	{
		if (comboCounter <= currentWeapon.AttackCombo.Count)
		{
			//Debug.Log("is resetCombo null? " + resetCombo.IsUnityNull());
			if (resetCombo != null)
			{
				StopAllCoroutines();
				resetCombo = null;
			}

			if (Time.time - lastAttackTime > currentAttackDelay)
			{
				lastAttackTime = Time.time;

				// animation
				anim.runtimeAnimatorController = currentWeapon.AttackCombo[comboCounter].AnimOverrider;
				anim.Play(attackAnimationName);

				currentAttackDelay = anim.GetCurrentAnimatorStateInfo(0).length + AttackDelay;
				currentComboResetTime = anim.GetCurrentAnimatorStateInfo(0).length + ComboResetTime;

				currentWeapon.OnAttack?.Invoke(currentWeapon.AttackCombo[comboCounter].Dmg);

				comboCounter++;

				if (comboCounter >= currentWeapon.AttackCombo.Count)
				{
					currentWeapon.LastAttack?.Invoke(currentWeapon.AttackCombo[comboCounter].Dmg);
					comboCounter = 0;
				}

			}
		}
	}

	void EndAttack()
	{
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName(attackAnimationName))
		{
			//  if null then:
			resetCombo ??= StartCoroutine(EndCombo());
		}
	}

	IEnumerator EndCombo()
	{
		yield return new WaitForSeconds(currentComboResetTime);
		Debug.Log("we have waited");
		comboCounter = 0;
	}
}
