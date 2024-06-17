using System.Collections;
using UnityEngine;

public class EnemyCombo : Combo
{
	[Header("Enemy settings: ")]
	[SerializeField] float attackRange;

	protected override void OnDisable()
	{
		base.OnDisable();
	}

	protected override void Start()
	{
		UpdateCurrentWeapon(defaultWeapon);
		anim = GetComponentInChildren<Animator>();
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

		//base.MeleeAttack();
		if (resetCombo != null)
		{
			StopAllCoroutines();
			resetCombo = null;
		}

		if (Time.time - lastComboTime < ComboDelay || Time.time - lastAttackTime < AttackDelay || comboCounter > currentWeapon.WeaponSo.AttackCombo.Count)
			return;

		if (comboCounter - 1 >= 0)
		{
			if (currentWeapon.WeaponSo.AttackCombo[comboCounter].AnimOverrider == currentWeapon.WeaponSo.AttackCombo[comboCounter - 1].AnimOverrider)
				anim.Play("IdleHand");
		}
		Debug.Log("i am:", transform);
		// animation: 
		anim.runtimeAnimatorController = currentWeapon.WeaponSo.AttackCombo[comboCounter].AnimOverrider;
		anim.Play(attackAnimationName);
		currentWeapon.Attack?.Invoke(currentWeapon.WeaponSo.AttackCombo[comboCounter].Dmg);

		lastAttackTime = Time.time;

		comboCounter++;

		if (comboCounter >= currentWeapon.WeaponSo.AttackCombo.Count)
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

	protected override void RangedAttack()
	{
		base.RangedAttack();
	}

	public override void UpdateCurrentWeapon(Weapon _NewWeapon)
	{
		base.UpdateCurrentWeapon(_NewWeapon);
	}

	public void CheckAttack()
	{
		EndAttack();
		var dir = (GameManager.enemyTargetPosition.position - transform.position).normalized;
		if (Vector3.Distance(transform.position, GameManager.enemyTargetPosition.position) < attackRange &&
			Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, attackRange))
		{
			currentWeapon.StartAttack?.Invoke();
		}
	}
	protected override int Damage()
	{
		return base.Damage();
	}

#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);
	}
#endif
}