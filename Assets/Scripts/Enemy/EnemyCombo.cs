using System;
using System.Collections;
using UnityEngine;

public class EnemyCombo : Combo
{
	[Header("Enemy settings: ")]
	[SerializeField] float attackRange;

   
    protected override void OnEnable()
	{
		UpdateCurrentWeapon(defaultWeapon);
		base.OnEnable();
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		comboCounter = 0;
		lastAttackTime = 0;
		lastComboTime = 0;
		multDmg = 0;
	}

	protected override void Start()
	{
		anim = GetComponentInChildren<Animator>();
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

		currentWeapon.HitCounter = 0;
		// animation: 
		anim.runtimeAnimatorController = currentWeapon.WeaponSo.AttackCombo[comboCounter].AnimOverrider;
		anim.Play(attackAnimationName);
		currentWeapon.Attack?.Invoke(currentWeapon.WeaponSo.AttackCombo[comboCounter].Dmg);

		lastAttackTime = Time.time;

		comboCounter++;

		if (comboCounter >= currentWeapon.WeaponSo.AttackCombo.Count)
		{
			// ("last attack: " + comboCounter);
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

	}

	public override void UpdateCurrentWeapon(Weapon _NewWeapon)
	{
		if (currentWeapon != null)
		{
			currentWeapon.StartAttack -= MeleeAttack;
		}

		currentWeapon = _NewWeapon;
		comboCounter = 0;
		lastComboTime = 0;
		lastAttackTime = 0;

		currentWeapon.StartAttack += MeleeAttack;
		animStopTime = 0.9f;
	}

	public void CheckAttack(out bool canMove)
	{
		var dir = (GameManager.enemyTargetPosition.position - transform.position).normalized;
		//Debug.Log("distance= " + Vector3.Distance(transform.position, GameManager.enemyTargetPosition.position) + "\nAttackRange= " + attackRange);
		if (Vector3.Distance(transform.position, GameManager.enemyTargetPosition.position) < attackRange &&
			Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, attackRange))
		{
			canMove = false;
			currentWeapon.StartAttack?.Invoke();
			//EndAttack();
		}
		else
		{
			canMove = true;
		}
	}
	protected override float Damage()
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