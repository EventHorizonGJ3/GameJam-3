using UnityEngine;

public class MeleeWeapon : Weapon
{
	protected bool firstHit;
	[SerializeField] OtherWeapon otherWeapon;
	private protected override void Awake()
	{
		base.Awake();
		if (otherWeapon != null)
			otherWeapon.WeaponSo = WeaponSo;
	}

	protected override void OnEnable()
	{
		this.LastAttack += ActivateKnockBack;
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
		if (otherWeapon != null)
			otherWeapon.transform.parent = transform;
	}

	protected override void OnGrabbed(Transform _leftHand)
	{
		base.OnGrabbed(_leftHand);
		otherWeapon?.OnGrabbed(_leftHand);
	}

	protected virtual void ActivateKnockBack()
	{
		currentKnockBack = WeaponSo.KnockBackPower;
		otherWeapon?.ActivateKnockBack();
	}

	protected override void OnAttack(int _Dmg)
	{
		firstHit = true;
		otherWeapon?.OnAttack(_Dmg);
		base.OnAttack(_Dmg);
	}
	protected override void OnAttackEnd()
	{
		base.OnAttackEnd();
		otherWeapon?.OnAttackEnd();
	}
	protected override void UpdateTrigger(bool _X)
	{
		base.UpdateTrigger(_X);
		otherWeapon?.UpdateTrigger(_X);
	}

}
