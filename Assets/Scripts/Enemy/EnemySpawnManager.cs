using System;

using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    //TODO: events to spwan different enemy or different amount
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
    [SerializeField] EnemyPoolerSO EnemyPooler;



    Dictionary<EnemyType, GameObject> EnemyDictionary;

    enum EnemyType
    {
        MANAGER, POLICE, ARMY, STACY, SUPREME
    }

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



        foreach (EnemyType enemyType in EnemyDictionary.Keys)
        {
            if (enemyType == EnemyType.STACY) break;
            EnemyPooler.amount = amountToPool;
            EnemyPooler.prefab = EnemyDictionary[enemyType];
            EnemyPooler.CreateEnemyPool(transform);
        }



    }






}
