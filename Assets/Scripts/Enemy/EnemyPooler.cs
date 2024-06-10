using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooler : MonoBehaviour
{
   public static EnemyPooler Instance;

    [Header("Prefabs")]
    [SerializeField] GameObject enemyType1;
    [SerializeField] GameObject enemyType2;
    [SerializeField] GameObject enemyType3;
    [SerializeField] GameObject enemyType4;
    [Header("Parameters")]
    [SerializeField] int amountToPool;

    List<GameObject> enemiesPrefab;

    private void Awake()
    {
        enemiesPrefab = new List<GameObject>
        {
            enemyType1,
            enemyType2,
            enemyType3,
            enemyType4
        };

        foreach (GameObject enemy in enemiesPrefab)
        {
            for (int i = 0; i < amountToPool; i++)
            {
                GameObject tmp = Instantiate(enemy);
                tmp.transform.position = transform.position;
                tmp.transform.parent = this.transform;
                tmp.SetActive(false);
            }
        }
    }
}
