using UnityEngine;

public class RangedWeapon : Weapon
{
	Transform target;
	Vector3 startPos;
	bool isThrowned;
	float timer = 0;
	protected override void Start()
	{
		base.Start();
	}
	protected override void OnEnable()
	{
		WeaponsSO.GetTarget += GetTarget;
		base.OnEnable();
	}
	protected override void OnDisable()
	{
		WeaponsSO.GetTarget -= GetTarget;
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
			if (timer < WeaponsSO.ThrowDuration)
			{
				transform.position = Vector3.Lerp(startPos, target.position, timer / WeaponsSO.ThrowDuration);
				timer += Time.deltaTime;
			}
			else
			{
				timer = 0;
				transform.position = target.position;
			}
		}
	}
	protected override void OnTriggerEnter(Collider _Other)
	{
		base.OnTriggerEnter(_Other);
	}
	protected override void UpdateTrigger(bool _X)
	{
		base.UpdateTrigger(_X);
		gameObject.SetActive(_X);
	}
	void GetTarget(Transform _Target)
	{
		target = _Target;
	}
}