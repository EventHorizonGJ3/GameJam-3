using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Combo")]
public class WeaponsSO : ScriptableObject
{
	public int WeaponID;
	public GameObject Prefab;

	[Header("Settings: ")]
	[Tooltip("number of hits you can do before the item breaks\n(should be at least greater then the number of combos)")]
	public int NumberOfUses = 6;

	[Header("Combo: ")]
	public List<AttackG> AttackCombo;

	// Actions: 
	public Action<int> OnAttack;
	public Action<int> LastAttack;
	public Action OnBreak;

}


	