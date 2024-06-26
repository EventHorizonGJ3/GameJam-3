using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObjectPooler : MonoBehaviour
{
    public static AudioObjectPooler SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    void Awake()
    {
        SharedInstance = this;
        transform.position = Vector3.zero;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool, transform);
            
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
    public GameObject GetPooledObject()
    {
        foreach (var objToPull in pooledObjects)
        {
            if (!objToPull.activeInHierarchy)
            {
                return objToPull;
            }
        }
        GameObject newObject = Instantiate(objectToPool);
        pooledObjects.Add(newObject);
        return newObject;
    }
}
