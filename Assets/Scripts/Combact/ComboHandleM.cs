using System;
using UnityEngine;

public class ComboHandleM : MonoBehaviour
{
	public float AttackDelay;
	public float ComboCooldown;
}
public class ComboInput
{
	float AttackDelay;
	float ComboReset;
	public ComboInput(float _AttackDelay, float _ComboReset)
	{
		AttackDelay = _AttackDelay;
		ComboReset = _ComboReset;
	}

	public virtual void Tick() { }
	public virtual void CheckInput() { }
	public virtual void Exit() { }
}
public class PlayerInputG : ComboInput
{
	ActionMap inuptActions;
	int counter = 0;
	int comboLenght;
	public PlayerInputG(float _AttackDelay, float _ComboReset, int _comboLenght) : base(_AttackDelay, _ComboReset)
	{
		inuptActions = InputManager.ActionMap;
		inuptActions.Player.Attack.performed += Attack;
		comboLenght = _comboLenght;
		counter = 0;
	}

	private void Attack(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		CheckInput();
	}
}