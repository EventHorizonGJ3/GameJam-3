using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    EnemySpawnManager spawnManager;
    List<GameObject> enemyPrefabs;
    [SerializeField] EnemyToSpawn enemyType = EnemyToSpawn.ENEMY_TYPE_1;
    [SerializeField] float spawnDelay = 2f;
    [SerializeField] int enemiesToSpawn;

    enum EnemyToSpawn
    {
        ENEMY_TYPE_1, ENEMY_TYPE_2, ENEMY_TYPE_3, ENEMY_TYPE_4
    }

    private void Awake()
    {
        enemyPrefabs = new List<GameObject>();
    }

    private void Start()
    {
        enemyPrefabs = spawnManager.GetEnemyPrefab((int)enemyType);
        SpawnEnemy();
    }

    private void OnEnable()
    {
        EnemySpawnManager.OnSpawnerReady += GetReferenceToManager;
    }
    private void OnDisable()
    {
        EnemySpawnManager.OnSpawnerReady -= GetReferenceToManager;
    }


    void SpawnEnemy() 
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            foreach (var enemy in enemyPrefabs)
            {
                if(!enemy.activeInHierarchy) 
                {
                    enemy.transform.position = transform.position;
                    enemy.SetActive(true);
                    break;
                }
            }
        }
    }

    void GetReferenceToManager(EnemySpawnManager instance)
    {
        spawnManager = instance;
    }
}
