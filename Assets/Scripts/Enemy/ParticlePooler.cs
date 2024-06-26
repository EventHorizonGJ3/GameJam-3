using System.Collections.Generic;
using UnityEngine;

public class ParticlePooler<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected GameObject particleToSpawn;
    [SerializeField] protected int numberToSpawn;
    protected List<T> enemyParticles = new();


    protected virtual void Awake()
    {
        for (int i = 0; i < numberToSpawn - 1; i++)
        {
            GameObject o = Instantiate(particleToSpawn, transform);
            enemyParticles.Add(o.GetComponent<T>());
            o.SetActive(false);
        }
    }


    protected virtual void SpawnAtPos(Vector3 pos)
    {
        foreach (var ePar in enemyParticles)
        {
            if (ePar.gameObject.activeInHierarchy == false)
            {
                ePar.gameObject.SetActive(true);
                ePar.transform.position = pos;
                break;
            }
        }
    }
}
