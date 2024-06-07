using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] float offset = 0.2f;
    [SerializeField] WeaponsSO Weapon;

    private void Start()
    {
        Invoke("SpawnWeapon", 3);
    }

    void SpawnWeapon()
    {
        List<IPickable> AllWeapons = WeaponsPooler.SharedInstance.GetWeaponList();
        foreach (IPickable weapon in AllWeapons)
        {
            if(weapon.WeaponsSO.WeaponID == Weapon.WeaponID && !weapon.Transform.gameObject.activeInHierarchy)
            {
                float Y = SetSpawnHeight(weapon.Transform);
                weapon.Transform.position = new Vector3(transform.position.x, Y , transform.position.z);
                break;
                
            }
        }
    }

    float SetSpawnHeight(Transform obj)
    {
        float floorY = 0;
        Ray ray = new(transform.position, Vector3.down);
        if(Physics.Raycast(ray,out RaycastHit hit) )
        {
            floorY = hit.point.y;
        }
        obj.transform.gameObject.SetActive(true);
        float Height = obj.GetComponent<Collider>().bounds.extents.y;
        float targetY = floorY + Height + offset;
        return targetY;
    }
       
        
        
        

}
