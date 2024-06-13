using UnityEngine;
using UnityEditor;

public class MeleeWeapon : MonoBehaviour, IPickable
{
	[Header("Settings: ")]
	[SerializeField] LayerMask enemyLayer;
	[SerializeField] float radius;
	[SerializeField] Transform top;
	[SerializeField] Transform bot;
	[Header("only when used by the player: ")]
	[SerializeField] float KnockBackPower;
	float currentKnockBack = 0;

	bool isTriggerActive;
	int myDmg;
	int hitCounter;

	public Transform Transform => transform;

	//! to remove:
	bool redGizmo;

	[field: SerializeField] public WeaponsSO WeaponsSO { get; set; }
	private void Start()
	{
		top = transform.GetChild(0);
	}

	private void OnEnable()
	{
		WeaponsSO.OnAttack += GetDamage;
		WeaponsSO.AttackEnd += DeactivateTrigger;
		WeaponsSO.LastAttack += ActivateKnockBack;
		WeaponsSO.OnBreak += Break;
	}

	private void OnDisable()
	{
		hitCounter = 0;
		currentKnockBack = 0;
		WeaponsSO.OnAttack -= GetDamage;
		WeaponsSO.AttackEnd -= DeactivateTrigger;
		WeaponsSO.LastAttack -= ActivateKnockBack;
		WeaponsSO.OnBreak -= Break;
	}

	private void Update()
	{
		if (isTriggerActive)
		{
			Collider[] enemyHit = new Collider[20];
			int _NumberOfEnemyes = Physics.OverlapCapsuleNonAlloc(bot.position, top.position, radius, enemyHit);
			if (_NumberOfEnemyes > 0)
			{
				for (int i = 0; i < _NumberOfEnemyes; i++)
				{
					if (enemyHit[i] != null)
					{
						if (enemyHit[i].TryGetComponent(out IDamageable hp))
						{
							hitCounter++;
							hp.TakeDamage(myDmg);
							hp.Knockback(currentKnockBack);

							if (hitCounter >= WeaponsSO.NumberOfUses)
							{
								WeaponsSO.OnBreak?.Invoke();
							}
						}
					}
				}
			}
		}
	}

	private void Break()
	{
		transform.parent = null;
		// play weapon breaking breaking sound 
		gameObject.SetActive(false);
	}

	private void ActivateKnockBack()
	{
		redGizmo = true;
		currentKnockBack = KnockBackPower;
	}


	///<summary>
	/// disables trigger and eneables collider
	///</summary>
	private void DeactivateTrigger()    // OnAttackEnd
	{
		//Debug.Log("deactivate trigger");
		UpdateTrigger(false);
	}

	// OnAttack
	private void GetDamage(int _Dmg)
	{
		myDmg = _Dmg;
		currentKnockBack = 0;
		UpdateTrigger(true);
	}

	void UpdateTrigger(bool x)
	{
		isTriggerActive = x;
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		float dist = Vector3.Distance(bot.position, top.position) / 2;
		Vector3 center = bot.position + bot.up * dist;
		Gizmos.color = Color.green;
		if (redGizmo)
			Gizmos.color = Color.red;
		Gizmos.DrawWireCube(center, new Vector3(radius, dist * 2, radius));
		redGizmo = false;
	}
#endif
}
