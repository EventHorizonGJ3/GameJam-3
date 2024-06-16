using UnityEngine;

public class MeleeWeapon : Weapon
{
	bool firstHit;
	protected override void OnEnable()
	{
		TryGetComponent(out trigger);
		WeaponsSO.LastAttack += ActivateKnockBack;
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		WeaponsSO.LastAttack -= ActivateKnockBack;
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
				if (hitCounter >= WeaponsSO.NumberOfUses)
				{
					WeaponsSO.OnBreak?.Invoke();
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
		currentKnockBack = WeaponsSO.KnockBackPower;
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
