using UnityEngine;

public class OtherWeapon : MeleeWeapon
{
	private protected override void Awake()
	{
		trigger = GetComponent<Collider>();
	}
	protected override void OnEnable()
	{

	}
	protected override void OnDisable()
	{

	}

	protected override void OnGrabbed(Transform _leftHand)
	{
		transform.position = _leftHand.position;
		transform.parent = _leftHand.parent;
	}

	protected override void OnAttack(float _Dmg)
	{
		firstHit = true;
		currentKnockBack = 0;
		myDmg = _Dmg;
	}

	protected override void OnTriggerEnter(Collider _Other)
	{
		Debug.Log("UwU");
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
		gameObject.SetActive(false);
	}

	protected override void OnAttackEnd()
	{
		this.currentKnockBack = 0;
		this.UpdateTrigger(false);
	}
	protected override void ActivateKnockBack()
	{
		currentKnockBack = WeaponSo.KnockBackPower;
	}

	protected override void UpdateTrigger(bool _X)
	{
		trigger.enabled = _X;
	}
}