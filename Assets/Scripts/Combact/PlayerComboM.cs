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
		if (resetCombo != null)
		{
			StopAllCoroutines();
			resetCombo = null;
		}

		if (Time.time - lastComboTime < ComboDelay || comboCounter > currentWeapon.AttackCombo.Count || Time.time - lastAttackTime < AttackDelay)
			return;

		lastAttackTime = Time.time;

		if (comboCounter - 1 > 0)
			if (currentWeapon.AttackCombo[comboCounter].AnimOverrider == currentWeapon.AttackCombo[comboCounter - 1].AnimOverrider)
				anim.Play("IdleHand");


		// animation: 
		anim.runtimeAnimatorController = currentWeapon.AttackCombo[comboCounter].AnimOverrider;
		anim.Play(attackAnimationName);

		if (comboCounter == currentWeapon.AttackCombo.Count - 1)
		{
			currentWeapon.LastAttack?.Invoke();
		}

		currentWeapon.OnAttack?.Invoke(currentWeapon.AttackCombo[comboCounter].Dmg);

		comboCounter++;

		if (comboCounter > currentWeapon.AttackCombo.Count)
			comboCounter = 0;
	}

	void EndAttack()
	{
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName(attackAnimationName))
		{
			//  if null then:
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
}
