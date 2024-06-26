using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParticlePooler : MonoBehaviour
{
    [SerializeField] GameObject particleToSpawn;
    [SerializeField] int numberToSpawn;
    List<EnemyParticles> enemyParticles = new();


    private void Awake()
    {
        for (int i = 0; i < numberToSpawn - 1; i++)
        {
            GameObject o = Instantiate(particleToSpawn, transform);
            enemyParticles.Add(o.GetComponent<EnemyParticles>());
            o.SetActive(false);
        }
    }

    private void OnEnable()
    {
        EnemyMovement.GetEnemyPos += SpawnAtPos;
    }

    private void OnDisable()
    {
        EnemyMovement.GetEnemyPos -= SpawnAtPos;
    }

    private void SpawnAtPos(Vector3 pos)
    {
        foreach (var ePar in enemyParticles)
        {
            if(ePar.gameObject.activeInHierarchy == false)
            {
                ePar.gameObject.SetActive(true);
                ePar.transform.position = pos;
                break;
            }
        }
    }
}
