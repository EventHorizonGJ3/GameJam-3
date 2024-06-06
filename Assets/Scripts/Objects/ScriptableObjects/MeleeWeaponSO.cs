using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon/MeleeWeapon")]
public class MeleeWeaponSO : ScriptableObject
{
	[Header("Settings: ")]
	[Tooltip("number of hits you can do before the item breaks\n(should be at least greater then the number of combos)")]
	public int NumberOfHits = 6;

	[Header("Combo: ")]
	public List<AttackG> AttackCombo;

}