using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IBreakable
{

	[field: SerializeField] public float HP { get; set; }
	[field: SerializeField] public GameObject BrokenObj { get; set; }
	[SerializeField] bool dealOneDmg;

	public bool IsBroken { get; set; }
	public Transform colliderTransform { get => transform; set => colliderTransform = colliderTransform; }

	public void Break()
	{
		if (IsBroken) return;
		IsBroken = true;
		gameObject.SetActive(false);

	}


	public void NoHP()
	{
		Break();
	}

	public void TakeDamage(int damage)
	{
		if (dealOneDmg)
			HP--;
		else
			HP -= damage;

		Score.OnDmg?.Invoke(damage);
		RageBar.OnRage?.Invoke();

		if (HP <= 0)
		{
			NoHP();
		}
	}
	public void Knockback(float _Power)
	{ }
}
