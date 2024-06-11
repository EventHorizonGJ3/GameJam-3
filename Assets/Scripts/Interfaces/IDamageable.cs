using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
	public int HP { get; set; }

	public void TakeDamage(int damage);

	///<summary>
	/// deactivate navmesh and use power to applay an impulse force on RB
	/// <param name="_Power"> the kcockback force </param>
	///</summary>
	public void Kncokback(float _Power);

	public void NoHP();
}
