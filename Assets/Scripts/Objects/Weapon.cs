using UnityEngine;

public class Weapon : MonoBehaviour, IPickable
{
	public Transform Transform => this.transform;
	protected bool isTriggerActive;
	protected int myDmg;
	protected int hitCounter;
	protected float currentKnockBack = 0;
	protected Collider trigger;
	protected IDamageable hp;


	[field: SerializeField] public WeaponsSO WeaponsSO { get; set; }
	protected virtual void OnAttack(int _Dmg)
	{
		currentKnockBack = 0;
		this.myDmg = _Dmg;
		this.UpdateTrigger(true); // isTriggerActive = true;
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