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
            if (prefab.TryGetComponent(out IEnemy pooledEnemy))
            {
                switch (pooledEnemy.Type)
                {
                    case EnemyType.MANAGER: managers.Add(pooledEnemy); break;
                    case EnemyType.POLICE: police.Add(pooledEnemy); break;
                    case EnemyType.ARMY: army.Add(pooledEnemy); break;
                }
            }
        }
        Debug.Log(managers.Count);
        Debug.Log(police.Count);
        Debug.Log(army.Count);  
    }



}
