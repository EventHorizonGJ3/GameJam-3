using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// public = VarX
/// private = varX
/// function = _VarX

public class PlayerComboM : Combo
{
	ActionMap inputs;

	private void Awake()
	{
		inputs = InputManager.ActionMap;
	}

	private void OnEnable()
	{
		inputs.Player.Attack.started += Attack;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		inputs.Player.Attack.started -= Attack;
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	private void Attack(InputAction.CallbackContext context)
	{
		currentWeapon.StartAttack?.Invoke();
	}

	public override void UpdateCurrentWeapon(Weapon _NewWeapon)
	{
		base.UpdateCurrentWeapon(_NewWeapon);
		PlayerActions.PlayerWeapon?.Invoke(_NewWeapon);
	}

	protected override void BackToPunches()
	{
		base.BackToPunches();
	}

	protected override void EndAttack()
	{
		base.EndAttack();
	}

	protected override IEnumerator EndCombo()
	{
		return base.EndCombo();
	}

	protected override void MeleeAttack()
	{
		base.MeleeAttack();
	}

	protected override void RangedAttack()
	{
		base.RangedAttack();
	}

}

