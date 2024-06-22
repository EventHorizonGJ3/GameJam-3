using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponDropper : MonoBehaviour
{
    WeaponsSO droppableWeapon;
    IDamageable hp;
    int init;
    [SerializeField] LayerMask mapFloor;

    private void Awake()
    {
        var x = GetComponent<EnemyCombo>();
        droppableWeapon = x.defaultWeapon.WeaponSo;
        init = 0;
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


                float targetY = SetSpawnHeight(weapon.Transform);
                weapon.Transform.position = new(transform.position.x, targetY, transform.position.z);
                weapon.Transform.gameObject.SetActive(true);


                break;

            }
        }
    }

    float SetSpawnHeight(Transform obj)
    {
        float floorY = 0;
        Ray ray = new(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 50, mapFloor))
        {
            floorY = hit.point.y;
        }
        obj.transform.gameObject.SetActive(true);
        float Height = obj.GetComponent<Collider>().bounds.extents.y;
        float targetY = floorY + Height + 0.1f;
        return targetY;
    }
}
