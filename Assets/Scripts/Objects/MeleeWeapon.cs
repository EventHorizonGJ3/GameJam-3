using UnityEngine;
using UnityEditor;

public class MeleeWeapon : Weapon
{
	bool firstHit;
	private void OnEnable()
	{
		TryGetComponent(out trigger);
		UpdateTrigger(false);
		WeaponsSO.OnAttack += OnAttack;
		WeaponsSO.AttackEnd += OnAttackEnd;
		WeaponsSO.LastAttack += ActivateKnockBack;
		WeaponsSO.OnBreak += Break;
	}

	private void OnDisable()
	{
		hitCounter = 0;
		currentKnockBack = 0;
		WeaponsSO.OnAttack -= OnAttack;
		WeaponsSO.AttackEnd -= OnAttackEnd;
		WeaponsSO.LastAttack -= ActivateKnockBack;
		WeaponsSO.OnBreak -= Break;
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

	private void Break()
	{
		transform.parent = null;
		// play weapon breaking breaking sound 
		gameObject.SetActive(false);
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
