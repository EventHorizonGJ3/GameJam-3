using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IPickable
{
	[Header("only when used by the player: ")]
	[SerializeField] float KnockBackPower;
	float currentKncokBack = 0;

	Collider myTrigger, myCollider;
	int myDmg;
	int hitsCounter;

	public Transform Transform => transform;

	[field: SerializeField] public WeaponsSO WeaponsSO { get; set; }
	private void Start()
	{
		var colliders = GetComponents<Collider>();
		foreach (var collider in colliders)
		{
			if (collider.isTrigger)
			{
				myTrigger = collider;
			}
			else
			{
				myCollider = collider;
			}
		}
	}

	private void OnEnable()
	{
		WeaponsSO.OnAttack += GetDamage;
		WeaponsSO.AttackEnd += DeactivateTrigger;
		WeaponsSO.LastAttack += ActivateKnockBack;
		WeaponsSO.OnBreak += DisableThySelf;
	}

	private void OnDisable()
	{
		hitsCounter = 0;
		WeaponsSO.OnAttack -= GetDamage;
		WeaponsSO.AttackEnd -= DeactivateTrigger;
		WeaponsSO.LastAttack -= ActivateKnockBack;
		WeaponsSO.OnBreak -= DisableThySelf;
	}

	private void DisableThySelf()
	{
		gameObject.SetActive(false);
	}

	private void ActivateKnockBack()
	{
		currentKncokBack = KnockBackPower;
	}

	private void OnTriggerEnter(Collider _Other)
	{
		if (_Other.TryGetComponent(out IDamageable hp))
		{
			hitsCounter++;
			hp.TakeDamage(myDmg);
			hp.Kncokback(currentKncokBack);
			if (hitsCounter >= WeaponsSO.NumberOfUses)
			{
				WeaponsSO.OnBreak?.Invoke();
			}
		}
	}

	///<summary>
	/// disables trigger and eneables collider
	///</summary>
	private void DeactivateTrigger()
	{
		UpdateTrigger(false);
	}

	private void GetDamage(int _Dmg)
	{
		myDmg = _Dmg;
		UpdateTrigger(true);
	}
	///<summary>
	/// uses myTrigger and myCollider .enabled 
	/// <para> while myTrigger follows the bool </para>
	/// myCollider does the opposite
	///</summary>
	void UpdateTrigger(bool x)
	{
		myTrigger.enabled = x;
		myCollider.enabled = !x;
	}

}
