using System.Collections;
using UnityEngine.InputSystem;

/// public = VarX
/// private = varX
/// function = _VarX

public class PlayerComboM : Combo
{
	ActionMap inputs;
	protected void Awake()
	{

		inputs = InputManager.ActionMap;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		inputs.Player.Attack.started += Attack;
		RageBar.OnBerserkExtraDmg += ActivateBerserk;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		inputs.Player.Attack.started -= Attack;
		RageBar.OnBerserkExtraDmg -= ActivateBerserk;
	}

	protected override void Start()
	{
		base.Start();
	}

	//void Update()
	//{
	//	this.EndAttack();
	//}

	private void Attack(InputAction.CallbackContext context)
	{
		currentWeapon.StartAttack?.Invoke();
	}

	public override void UpdateCurrentWeapon(Weapon _NewWeapon)
	{
		base.UpdateCurrentWeapon(_NewWeapon);
	}

	protected override void BackToPunches()
	{
		base.BackToPunches();
	}

	protected override IEnumerator EndAttack()
	{
		return base.EndAttack();
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

	protected void ActivateBerserk(float _Dmg)
	{
		multDmg = _Dmg;
	}
	protected override float Damage()
	{
		return base.Damage();
	}
}

