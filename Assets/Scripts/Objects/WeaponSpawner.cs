using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour, IWeaponSpawner
{
    [Header("Parameters")]
    [SerializeField][Tooltip("Altezza dal suolo")] float offset = 0.2f;
    [SerializeField] float respawnCooldown = 10f;
    [Space]
    [SerializeField] bool isOnEnemy;
    [Header("References)")]
    [SerializeField] WeaponsSO weaponToSpawn;

    GameObject loadedWeapon;
    bool canSpawn;

    private void Awake()
    {
        if(TryGetComponent<EnemyMovement>(out _)) isOnEnemy = true; 
        else isOnEnemy = false;
    }
    private void OnDisable()
    {
        if(isOnEnemy) SpawnOnChance();
    }

    private void Start()
    {
        canSpawn = true;
        AutoCorrectPosition();
        if (!isOnEnemy) 
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
        float targetY = floorY + Height + offset;
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
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Collider hitCollider = hit.collider;
            Vector3 colliderCenter = hitCollider.bounds.center;
            transform.position = new(colliderCenter.x, colliderCenter.y + 1, colliderCenter.z);
        }
    }

    public void StartRespawn()
    {
        if(!isOnEnemy)
            StartCoroutine(StartCooldown());
    }

    void SpawnOnChance()
    {
        int chance = Random.Range(0, 1);
        if (chance == 1 ) SpawnWeapon();
    }
}
