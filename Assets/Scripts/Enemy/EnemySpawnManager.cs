using System;

using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    //TODO: events to spwan different enemy or different amount
    public static Action<EnemySpawnManager> OnSpawnerReady;

    [Header("Prefabs")]
    [SerializeField] GameObject enemyType1;
    [SerializeField] GameObject enemyType2;
    [SerializeField] GameObject enemyType3;
    [SerializeField] GameObject enemyType4;
    [Header("Parameters")]
    [SerializeField] int amountToPool;

    List<GameObject> enemiesType1Prefab;
    List<GameObject> enemiesType2Prefab;
    List<GameObject> enemiesType3Prefab;
    List<GameObject> enemiesType4Prefab;
    Dictionary<int, List<GameObject>> enemies;

    enum EnemyType
    {
        ENEMY_TYPE_1, ENEMY_TYPE_2, ENEMY_TYPE_3, ENEMY_TYPE_4
    }

    private void Awake()
    {
        enemiesType1Prefab = new List<GameObject>();
        enemiesType2Prefab = new List<GameObject>();
        enemiesType3Prefab = new List<GameObject>();
        enemiesType4Prefab = new List<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            GameObject tmp1 = Instantiate(enemyType1);
            tmp1.transform.position = transform.position;
            tmp1.transform.parent = this.transform;
            tmp1.SetActive(false);
            enemiesType1Prefab.Add(tmp1);

            GameObject tmp2 = Instantiate(enemyType2);
            tmp2.transform.position = transform.position;
            tmp2.transform.parent = this.transform;
            tmp2.SetActive(false);
            enemiesType2Prefab.Add(tmp2);

            GameObject tmp3 = Instantiate(enemyType3);
            tmp3.transform.position = transform.position;
            tmp3.transform.parent = this.transform;
            tmp3.SetActive(false);
            enemiesType3Prefab.Add(tmp3);

            GameObject tmp4 = Instantiate(enemyType4);
            tmp4.transform.position = transform.position;
            tmp4.transform.parent = this.transform;
            tmp4.SetActive(false);
            enemiesType4Prefab.Add(tmp4);
        }

        enemies = new Dictionary<int, List<GameObject>>
        {
            { (int)EnemyType.ENEMY_TYPE_1, enemiesType1Prefab },
            { (int)EnemyType.ENEMY_TYPE_2, enemiesType2Prefab },
            { (int)EnemyType.ENEMY_TYPE_3, enemiesType3Prefab },
            { (int)EnemyType.ENEMY_TYPE_4, enemiesType4Prefab }
        };

        // Give reference
        OnSpawnerReady?.Invoke(this);
    }

    

    public List<GameObject> GetEnemyPrefab(int index)
    {
        return enemies[index];
    }

}
