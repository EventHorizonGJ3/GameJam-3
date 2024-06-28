using System.Collections;
using UnityEngine;

public class RangedWeapon : Weapon
{
	Transform target;
	Vector3 startPos;

	float timer = 0;

	private protected override void Awake()
	{
		base.Awake();
	}
	protected override void OnEnable()
	{
		Target += GetTarget;
		base.OnEnable();
	}
	protected override void OnDisable()
	{
		Target -= GetTarget;
		base.OnDisable();
	}
	protected override void OnInizialize()
	{
		base.OnInizialize();
	}
	protected override void OnBreak()
	{
		//base.OnBreak();
	}
	protected override void OnAttack(float _Dmg)
	{
		currentKnockBack = 0;
		this.myDmg = _Dmg;
		OnAttackEnd();
	}
	protected override void OnAttackEnd()
	{
		transform.parent = null;
		startPos = transform.position;
		timer = 0;

		if (target != null)
		{
			StartCoroutine(ThrowObject());
		}

	}

	IEnumerator ThrowObject()
	{
		while (timer < WeaponSo.ThrowDuration)
		{
			transform.position = Vector3.Lerp(startPos, target.position + Vector3.up * 0.5f, timer / WeaponSo.ThrowDuration);
			timer += Time.deltaTime;
			yield return null;
		}

		timer = 0;
		transform.position = target.position;
		if (target.TryGetComponent(out IDamageable _Hp))
		{
			WeaponParticleOnHit.OnHitPos?.Invoke(transform.position);
			_Hp.TakeDamage(myDmg);
			if (myDmg > 0) base.PlaySound();
		}
		gameObject.SetActive(false);
		Break?.Invoke();
	}

	protected override void UpdateTrigger(bool _X)
	{
		base.UpdateTrigger(_X);
	}
	void GetTarget(Transform _Target)
	{
		target = _Target;
	}

	protected override void OnGrabbed(Transform _leftHand)
	{
		base.OnGrabbed(_leftHand);
		transform.parent = _leftHand; //ByEma
		transform.localPosition = Vector3.zero; //ByEma
		transform.localRotation = Quaternion.identity; //ByEma
	}
	protected override void OnTriggerEnter(Collider _Other)
	{ }

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{

		Gizmos.DrawWireSphere(transform.root.position, WeaponSo.RangedRange);
	}
#endif
}