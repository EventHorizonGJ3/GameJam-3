using System.Collections;
using UnityEngine;

public class EnemyCombo : Combo
{
	[Header("Enemy settings: ")]
	[SerializeField] float attackRange;
	private void OnEnable()
	{

	}
	protected override void OnDisable()
	{
		base.OnDisable();
	}
	protected override void Start()
	{
		base.Start();
	}
	protected override void Update()
	{
		base.Update();
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
		var dir = (GameManager.enemyTargetPosition.position - transform.position).normalized;
		if (Vector3.Distance(transform.position, GameManager.enemyTargetPosition.position) < attackRange && Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, attackRange))
			base.MeleeAttack();
	}
	protected override void RangedAttack()
	{
		base.RangedAttack();
	}
	public override void UpdateCurrentWeapon(WeaponsSO _NewWeapon)
	{
		base.UpdateCurrentWeapon(_NewWeapon);
	}
}