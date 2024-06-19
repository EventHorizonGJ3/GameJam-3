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
		LastAttack += ActivateKnockBack;
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		LastAttack -= ActivateKnockBack;
		base.OnDisable();
	}

	protected override void OnTriggerEnter(Collider _Other)
	{

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
					Break?.Invoke();
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
