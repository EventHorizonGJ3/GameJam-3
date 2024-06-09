using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponsPooler : MonoBehaviour
{
    public static WeaponsPooler SharedInstance;
    private List<GameObject> pooledObjects;
    private List<IPickable> pooledWeapons;
    public List<GameObject> weaponsToPull;
    public int amountToPool;

    void Awake()
    {
        SharedInstance = this;
        pooledObjects = new List<GameObject>();
        pooledWeapons = new List<IPickable>();

        CreatePool();
    }

    void Start()
    {
        
    }

    void CreatePool()
    {
        
        foreach(GameObject weapon in weaponsToPull)
        {
            for (int i = 0; i < amountToPool; i++)
            {
                GameObject tmp = Instantiate(weapon);
                tmp.transform.position = transform.position;
                tmp.SetActive(false);
                if(tmp.TryGetComponent(out IPickable pickable)) pooledWeapons.Add(pickable);
            }
        }
    }


    public List<IPickable> GetWeaponList()
    {
        return pooledWeapons;
    }




  
}
