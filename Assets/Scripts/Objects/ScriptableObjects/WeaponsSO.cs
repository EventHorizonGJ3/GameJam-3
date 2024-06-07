using System;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponsSO" ,menuName = "Weapon/weapon")]
public class WeaponsSO : ScriptableObject
{
	public Action OnAttack;
	public Action OnBreak;



	public int WeaponID;
}