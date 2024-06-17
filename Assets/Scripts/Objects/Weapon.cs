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
	[SerializeField] public Weapon MyWeapon { get => this; set => MyWeapon = value; }
	public WeaponsSO WeaponSo;

	//- "Actions: "
	public Action<int> Attack;
	public Action<Transform> Target;
	public Action AttackEnd;
	public Action LastAttack;
	public Action Break;
	public Action Grabbed;
	public Action StartAttack;

	protected virtual void Awake()
	{
		TryGetComponent(out this.trigger);
	}
	protected virtual void OnEnable()
	{
		Grabbed += this.OnGrabbed;
		Attack += this.OnAttack;
		AttackEnd += this.OnAttackEnd;
		Break += this.OnBreak;
	}

	protected virtual void OnGrabbed()
	{
		this.UpdateTrigger(false);
	}

	protected virtual void OnDisable()
	{
		hitCounter = 0;
		currentKnockBack = 0;
		this.Attack -= this.OnAttack;
		this.AttackEnd -= this.OnAttackEnd;
		this.Break -= this.OnBreak;
		this.Grabbed -= this.OnGrabbed;
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