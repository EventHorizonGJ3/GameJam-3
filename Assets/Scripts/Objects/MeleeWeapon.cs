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
		WeaponsSO.OnBreak += DisableThySelf;
	}

	private void OnDisable()
	{
		hitCounter = 0;
		currentKnockBack = 0;
		WeaponsSO.OnAttack -= GetDamage;
		WeaponsSO.AttackEnd -= DeactivateTrigger;
		WeaponsSO.LastAttack -= ActivateKnockBack;
		WeaponsSO.OnBreak -= DisableThySelf;
	}

    private void Update()
    {
        if (isTriggerActive)
        {
			Collider[] enemyHit = new Collider[10];
			int _NumberOfEnemyes = Physics.OverlapCapsuleNonAlloc(bot.position, top.position, radius, enemyHit);
			if(_NumberOfEnemyes > 0)
            {
				foreach (Collider x in enemyHit)
                {
					if( x != null)
                    {
						if (x.TryGetComponent(out IDamageable hp))
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
			currentKnockBack = 0;
		}
    }

    private void DisableThySelf()
	{
		gameObject.SetActive(false);
	}

	private void ActivateKnockBack()
	{
		currentKnockBack = KnockBackPower;
	}

	//private void OnTriggerEnter(Collider _Other)
	//{
	//	if (_Other.TryGetComponent(out IDamageable hp))
	//	{
	//		hitsCounter++;
	//		hp.TakeDamage(myDmg);
	//		hp.Kncokback(currentKncokBack);
	//		if (hitsCounter >= WeaponsSO.NumberOfUses)
	//		{
	//			WeaponsSO.OnBreak?.Invoke();
	//		}
	//	}
	//}

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

	void UpdateTrigger(bool x)
	{
		isTriggerActive  = x;
	}

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
		float dist = Vector3.Distance(bot.position,top.position)/2;
		Vector3 center = bot.position + bot.up * dist;
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(center, new Vector3(radius, dist * 2, radius));
    }
#endif
}
