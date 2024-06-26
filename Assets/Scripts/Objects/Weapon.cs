using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Weapon : MonoBehaviour, IPickable, ISoundable
{
	public Transform Transform => this.transform;
	protected float myDmg;
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
	public Action Inizialize;
	public Action<float> Attack;
	public Action<Transform> Target;
	public Action AttackEnd;
	public Action LastAttack;
	public Action Break;
	public Action<Transform> Grabbed;
	public Action StartAttack;

	protected virtual private void Awake()
	{
		this.trigger = this.GetComponent<Collider>();
		this.Inizialize += OnInizialize;
	}

	protected virtual void OnInizialize()
	{
		this.UpdateTrigger(false);
	}

	protected virtual void OnEnable()
	{
		SceneManager.sceneUnloaded += OnSceneUnloaded;
		this.initialPos = transform.position;
		this.Grabbed += this.OnGrabbed;
		this.Attack += this.OnAttack;
		this.AttackEnd += this.OnAttackEnd;
		this.Break += this.OnBreak;
	}

	private void OnSceneUnloaded(Scene arg0)
	{
		this.Inizialize -= OnInizialize;
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
		this.transform.parent = null;
		this.gameObject.SetActive(false);
	}
	protected virtual void OnAttack(float _Dmg)
	{
		this.UpdateTrigger(true);
		this.currentKnockBack = 0;
		this.myDmg = _Dmg;
	}

	protected virtual void OnAttackEnd()
	{
		this.currentKnockBack = 0;

	}
	protected virtual void UpdateTrigger(bool _X)
	{
		if (this.trigger.enabled == _X)
			return;
		
		this.trigger.enabled = _X;
	}

	protected virtual void OnTriggerEnter(Collider _Other)
	{
		
	}

    public void PlaySound()
    {
		AudioManager.instance.PlaySFX(AudioManager.instance.AudioData.SFX_HitBonk,transform);
    }
}