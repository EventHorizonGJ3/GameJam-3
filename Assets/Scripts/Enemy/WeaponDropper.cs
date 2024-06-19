using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDropper : MonoBehaviour
{
    WeaponsSO droppableWeapon;

    int init;

    private void Awake()
    {
        droppableWeapon = GetComponent<EnemyCombo>().defaultWeapon.WeaponSo;
    }

    private void OnDisable()
    {
        init++;
        if (init > 1)
            SpawnOnChance();
    }

    void SpawnOnChance()
    {
        float chance = Random.Range(0, 1);
        Debug.Log("probabilità:" + chance);
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
