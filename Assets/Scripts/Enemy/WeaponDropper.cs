using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponDropper : MonoBehaviour
{
    [SerializeField] WeaponsSO droppableWeapon;

    [SerializeField] bool thisIsAnObject;
    int init;
    [SerializeField] LayerMask mapFloor;

    private void Awake()
    {
        Init();
        init = 0;
    }

    private void OnDisable()
    {
        if (thisIsAnObject)
        {
            SpawnOnChance();
            
            return;
        }
        init++;
        if (init > 1)
            SpawnOnChance();
    }

    void SpawnOnChance()
    {
        float chance = Random.Range(0, 1f);
        Debug.Log("probabilità:" + chance);
        if (chance <= 0.5f) DropWeapon();

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

    void Init()
    {
        if (!thisIsAnObject)
        {
            var x = GetComponent<EnemyCombo>();
            droppableWeapon = x.defaultWeapon.WeaponSo;
            Debug.Log("eseguito");
        }
    }






}
