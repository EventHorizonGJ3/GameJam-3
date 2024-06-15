using System;
using UnityEngine;

public class Weapon : MonoBehaviour, IPickable
{
	public Transform Transform => this.transform;
	protected int myDmg;
	protected int hitCounter;
	protected float currentKnockBack = 0;
	protected Collider trigger;
	protected IDamageable hp;
	[field: SerializeField] public WeaponsSO WeaponSo { get; set; }


	protected virtual void Awake()
	{
		TryGetComponent(out this.trigger);
	}
	protected virtual void OnEnable()
	{
		WeaponSo.OnGrabbed += this.OnGrabbed;
		WeaponSo.OnAttack += this.OnAttack;
		WeaponSo.AttackEnd += this.OnAttackEnd;
		WeaponSo.OnBreak += this.OnBreak;
	}

	protected virtual void OnGrabbed()
	{
		this.UpdateTrigger(false);
	}

	protected virtual void OnDisable()
	{
		hitCounter = 0;
		currentKnockBack = 0;
		WeaponSo.OnAttack -= this.OnAttack;
		WeaponSo.AttackEnd -= this.OnAttackEnd;
		WeaponSo.OnBreak -= this.OnBreak;
		WeaponSo.OnGrabbed -= this.OnGrabbed;
	}
	protected virtual void OnBreak()
	{
		this.transform.parent = null;
		gameObject.SetActive(false);
	}
	protected virtual void OnAttack(int _Dmg)
	{
		this.currentKnockBack = 0;
		this.myDmg = _Dmg;
		this.UpdateTrigger(true);
	}

	protected virtual void OnAttackEnd()
	{
		currentKnockBack = 0;
		this.UpdateTrigger(false);
	}
	protected virtual void UpdateTrigger(bool _X)
	{
		this.trigger.enabled = _X;
	}

	protected virtual void OnTriggerEnter(Collider _Other)
	{
	}
}