using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour, IWeaponSpawner
{
    [Header("Parameters")]
    [SerializeField][Tooltip("Altezza dal suolo")] float offset = 0.2f;
    [SerializeField] float respawnCooldown = 10f;
    [Header("References)")]
    [SerializeField] WeaponsSO weaponToSpawn;
    [SerializeField] LayerMask ground;


    GameObject loadedWeapon;
    bool canSpawn;




    private void Start()
    {
        canSpawn = true;


        AutoCorrectPosition();
        SpawnWeapon();


    }

    void SpawnWeapon()
    {
        List<IPickable> AllWeapons = WeaponsPooler.SharedInstance.GetWeaponList();
        foreach (IPickable weapon in AllWeapons)
        {
            if (weapon.MyWeapon.WeaponSo == weaponToSpawn && !weapon.Transform.gameObject.activeInHierarchy && canSpawn)
            {
                float targetY = SetSpawnHeight(weapon.Transform);
                weapon.Transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
                loadedWeapon = weapon.Transform.gameObject;
                loadedWeapon.transform.parent = this.transform;
                canSpawn = false;
                break;

            }
        }
    }



    float SetSpawnHeight(Transform obj)
    {
        float floorY = 0;
        Ray ray = new(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            floorY = hit.point.y;
        }
        obj.transform.gameObject.SetActive(true);
        float Height = obj.GetComponent<Collider>().bounds.extents.y;
        float targetY = floorY + offset;
        return targetY;
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(respawnCooldown);
        canSpawn = true;
        SpawnWeapon();
    }

    void AutoCorrectPosition()
    {
        Ray ray = new(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 50, ground))
        {
            Collider hitCollider = hit.collider;
            Vector3 colliderCenter = hitCollider.bounds.center;
            transform.position = new(colliderCenter.x, colliderCenter.y + 1, colliderCenter.z);
        }
    }

    public void StartRespawn()
    {

        StartCoroutine(StartCooldown());
    }


}
