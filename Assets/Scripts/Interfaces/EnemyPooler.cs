using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooler : MonoBehaviour
{
    public static EnemyPooler Instance;

    public int amount;
    public GameObject prefab;
    public List<IEnemy> managers;
    public List<IEnemy> police;
    public List<IEnemy> army;

    private void Awake()
    {
        Instance = this;
        managers = new List<IEnemy>();
        police = new List<IEnemy>();
        army = new List<IEnemy>();
    }
    public void CreateEnemyPool(Transform transform)
    {


        for (int i = 0; i < amount; i++)
        {
            var enemy = Instantiate(prefab, transform);
            enemy.SetActive(false);
            if (enemy.TryGetComponent(out IEnemy pooledEnemy))
            {
                switch (pooledEnemy.Type)
                {
                    case EnemyType.MANAGER: managers.Add(pooledEnemy); break;
                    case EnemyType.POLICE: police.Add(pooledEnemy); break;
                    case EnemyType.ARMY: army.Add(pooledEnemy); break;
                }
            }
        }
        // ($"Managers: {managers.Count}");
        // ($"Police: {police.Count}");
        // ($"Army: {army.Count}");
    }

    public List<IEnemy> GetEnemy(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.MANAGER: return managers;
            case EnemyType.POLICE: return police;
            case EnemyType.ARMY: return army;
            default: return null;
        }
    }



}
