using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName =("EnemyPoolerSO"), fileName =("NewEnemyPooler"))]
public class EnemyPoolerSO : ScriptableObject
{
    public int amount;
    public GameObject prefab;
    private List<GameObject> enemies;


    public void CreateEnemyPool(Transform transform)
    {
        enemies = new List<GameObject>();

        for (int i = 0; i < amount; i++)
        {
            var enemy = Instantiate(prefab, transform);
            enemy.SetActive(false);
            enemies.Add(enemy);
        }

        
    }

    
}
