using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon")]
public class WeaponsSO : ScriptableObject
{
	public List<AttackG> AttackCombo;
	[Tooltip("number of hits you can do before the item breaks")]
	public int NumberOfHits;
	public Action OnAttack;
	public Action OnBreak;
}

[System.Serializable]
public class AttackG
{
	public AnimatorOverrideController AnimOverrider;
	public float Dmg;
}
