using System;

using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    
    public static Action<EnemySpawnManager> OnSpawnerReady;

    [Header("Prefabs Base Enemnies")]
    [SerializeField] GameObject enemy_Manager;
    [SerializeField] GameObject enemy_Police;
    [SerializeField] GameObject enemy_Army;
    [Header("Prefab Bosses")]
    [SerializeField] GameObject enemy_Stacy;
    [SerializeField] GameObject enemy_Supreme;
    [Header("Parameters")]
    [SerializeField] int amountToPool;
    [Header("Ref")]





    Dictionary<EnemyType, GameObject> EnemyDictionary;


    private void Awake()
    {
        EnemyDictionary = new Dictionary<EnemyType, GameObject>
        {
            {EnemyType.MANAGER, enemy_Manager },
            {EnemyType.POLICE, enemy_Police },
            {EnemyType.ARMY, enemy_Army },
            {EnemyType.STACY, enemy_Stacy },
            {EnemyType.SUPREME, enemy_Supreme }
        };




    }

    private void Start()
    {
        if (EnemyPooler.Instance == null) //Pull all base enemies
        {
            Debug.LogError("EnemyPooler instance is null");
            return;
        }
        foreach (EnemyType enemyType in EnemyDictionary.Keys)
        {
            if (enemyType == EnemyType.STACY) break;
            EnemyPooler.Instance.amount = amountToPool;
            EnemyPooler.Instance.prefab = EnemyDictionary[enemyType];
            EnemyPooler.Instance.CreateEnemyPool(transform);
        }

        EnemyPooler.Instance.PoolBosses(enemy_Stacy, enemy_Supreme, transform); //Pool bosses

        
    }
}
public enum EnemyType
{
    MANAGER, POLICE, ARMY, STACY, SUPREME
}










