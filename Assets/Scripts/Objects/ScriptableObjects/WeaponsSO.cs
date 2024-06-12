using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Combo")]
public class WeaponsSO : ScriptableObject
{

	[Header("Settings Melee: ")]
	[Tooltip("number of hits you can do before the item breaks\n(should be at least greater then the number of combos)")]
	public int WeaponID;
	public int NumberOfUses = 6;
	//public GameObject Prefab;

	[Header("Settings Ranged")]
	public bool IsRanged;
	public float Range;

	[Header("Combo: ")]
	public List<AttackG> AttackCombo;

	// Actions: 
	public Action<int> OnAttack;
	public Action<Transform> GetTarget;
	public Action AttackEnd;
	public Action LastAttack;
	public Action OnBreak;
}


