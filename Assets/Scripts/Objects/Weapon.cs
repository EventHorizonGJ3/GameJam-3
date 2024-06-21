using System;
using UnityEngine;

public class Weapon : MonoBehaviour, IPickable
{
	public Transform Transform => this.transform;
	protected int myDmg;
	protected int hitCounter;
	public int HitCounter { get => hitCounter; set => hitCounter = value; }
	protected float currentKnockBack = 0;
	protected Collider trigger;
	protected IDamageable hp;
	protected Vector3 initialPos;
	[SerializeField] public Weapon MyWeapon { get => this; set => MyWeapon = value; }
	public WeaponsSO WeaponSo;

	[field: SerializeField] public bool IsEnemyWeapon { get; set; }

	//- "Actions: "
	public Action<int> Attack;
	public Action<Transform> Target;
	public Action AttackEnd;
	public Action LastAttack;
	public Action Break;
	public Action<Transform> Grabbed;
	public Action StartAttack;

	protected virtual private void Awake()
	{
		this.trigger = this.GetComponent<Collider>();

	}

	protected virtual void OnEnable()
	{
		this.initialPos = transform.position;
		this.Grabbed += this.OnGrabbed;
		this.Attack += this.OnAttack;
		this.AttackEnd += this.OnAttackEnd;
		this.Break += this.OnBreak;
	}

	protected virtual void OnGrabbed(Transform _leftHand)
	{
		this.UpdateTrigger(false);
	}

	protected virtual void OnDisable()
	{
		this.hitCounter = 0;
		this.currentKnockBack = 0;
		this.Attack -= this.OnAttack;
		this.AttackEnd -= this.OnAttackEnd;
		this.Break -= this.OnBreak;
		this.Grabbed -= this.OnGrabbed;
		this.UpdateTrigger(true);
	}
	protected virtual void OnBreak()
	{
		this.transform.parent = base.transform;
		this.gameObject.SetActive(false);
	}
	protected virtual void OnAttack(int _Dmg)
	{
		this.currentKnockBack = 0;
		this.myDmg = _Dmg;
		this.UpdateTrigger(true);

	}

	protected virtual void OnAttackEnd()
	{
		this.currentKnockBack = 0;
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