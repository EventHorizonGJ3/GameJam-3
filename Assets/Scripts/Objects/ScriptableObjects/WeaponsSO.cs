using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Combo")]
public class WeaponsSO : ScriptableObject
{
	[Header("Genaral Settings: ")]
	public int NumberOfUses = 6;
	public LayerMask EnemyLayer;

	[Header("Settings Melee: ")]
	public float KnockBackPower;

	[Header("Settings Ranged: ")]
	public bool IsRanged;
	public float RangedRange;

	[Header("Combo: ")]
	public List<AttackG> AttackCombo;

	// Actions: 
	public Action<int> OnAttack;
	public Action<Transform> GetTarget;
	public Action AttackEnd;
	public Action LastAttack;
	public Action OnBreak;
}


