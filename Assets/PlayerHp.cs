using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour, IDamageable
{
	[field: SerializeField] public int HP { get; set; }
	public Transform colliderTransform { get; set; }

	public void Knockback(float _Power)
	{ }

	public void NoHP()
	{
		//TODO: GameManager.EndGame?.invoke();
		Debug.LogError("OH I'M DIE THANK YOU FOREVER");
	}

	public void TakeDamage(int damage)
	{
		HP -= damage;
		//Todo: 
		if (HP <= 0)
		{
			NoHP();
		}
	}
}
