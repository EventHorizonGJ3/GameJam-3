using UnityEngine;

public class RangedWeapon : Weapon
{
	Transform target;
	Vector3 startPos;
	bool isThrowned;
	float timer = 0;
	protected override void Awake()
	{
		base.Awake();
	}
	protected override void OnEnable()
	{
		WeaponSo.GetTarget += GetTarget;
		base.OnEnable();
	}
	protected override void OnDisable()
	{
		WeaponSo.GetTarget -= GetTarget;
		base.OnDisable();
	}
	protected override void OnBreak()
	{
		base.OnBreak();
	}
	protected override void OnAttack(int _Dmg)
	{
		currentKnockBack = 0;
		this.myDmg = _Dmg;
	}
	protected override void OnAttackEnd()
	{
		transform.parent = null;
		startPos = transform.position;
		isThrowned = true;
		timer = 0;
	}
	private void Update()
	{
		if (isThrowned)
		{
			if (timer < WeaponSo.ThrowDuration)
			{
				transform.position = Vector3.Lerp(startPos, target.position, timer / WeaponSo.ThrowDuration);
				timer += Time.deltaTime;
			}
			else
			{
				timer = 0;
				transform.position = target.position;
				if (target.TryGetComponent(out IDamageable _Hp))
				{
					_Hp.TakeDamage(myDmg);
					gameObject.SetActive(false);
				}
			}
		}
	}

	protected override void UpdateTrigger(bool _X)
	{
		base.UpdateTrigger(_X);
	}
	void GetTarget(Transform _Target)
	{
		target = _Target;
	}

	protected override void OnGrabbed()
	{
		base.OnGrabbed();
	}
}