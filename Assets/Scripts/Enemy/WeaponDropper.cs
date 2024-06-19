using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDropper : MonoBehaviour
{
	WeaponsSO droppableWeapon;
	IDamageable hp;
	int init;

	private void Awake()
	{
		var x = GetComponent<EnemyCombo>();
		droppableWeapon = x.defaultWeapon.WeaponSo;
	}

	private void OnDisable()
	{
		init++;
		if (init > 1)
			SpawnOnChance();
	}

	void SpawnOnChance()
	{
		float chance = Random.Range(0, 1f);
		Debug.Log("probabilit�:" + chance);
		if (chance <= 0.5) DropWeapon();

	}
	void DropWeapon()
	{
		List<IPickable> AllWeapons = WeaponsPooler.SharedInstance.GetWeaponList();
		foreach (IPickable weapon in AllWeapons)
		{
			if (weapon.MyWeapon.WeaponSo == droppableWeapon && !weapon.Transform.gameObject.activeInHierarchy)
			{

				weapon.Transform.position = transform.position;
				weapon.Transform.gameObject.SetActive(true);

				break;

			}
		}
	}
}
