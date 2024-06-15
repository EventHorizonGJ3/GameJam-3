using UnityEngine;

public class MeleeWeapon : Weapon
{
	bool firstHit;
	protected override void Awake()
	{
		base.Awake();
	}
	protected override void OnEnable()
	{
		WeaponSo.LastAttack += ActivateKnockBack;
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		WeaponSo.LastAttack -= ActivateKnockBack;
		base.OnDisable();
	}

	protected override void OnTriggerEnter(Collider _Other)
	{
		Debug.Log("trigger active and hit");
		if (_Other.TryGetComponent(out IDamageable hp))
		{
			hp.colliderTransform = transform.root;
			hp.TakeDamage(myDmg);
			hp.Knockback(currentKnockBack);
			if (firstHit)
			{
				firstHit = false;
				hitCounter++;
				if (hitCounter >= WeaponSo.NumberOfUses)
				{
					WeaponSo.OnBreak?.Invoke();
				}
			}
		}
	}

	protected override void OnBreak()
	{
		base.OnBreak();
	}

	private void ActivateKnockBack()
	{
		currentKnockBack = WeaponSo.KnockBackPower;
	}


	///<summary>
	/// disables trigger and eneables collider
	///</summary>


	// OnAttack
	protected override void OnAttack(int _Dmg)
	{
		firstHit = true;
		base.OnAttack(_Dmg);
	}
	protected override void OnAttackEnd()
	{
		base.OnAttackEnd();
	}
	protected override void UpdateTrigger(bool _X)
	{
		base.UpdateTrigger(_X);
	}

}
