using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
	public Transform Transform { get; }
	public Weapon MyWeapon { get; set; }
	public bool IsEnemyWeapon { get; set; }
}
