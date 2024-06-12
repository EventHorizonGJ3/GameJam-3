using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    EnemyType currentSpawned;
    [SerializeField][Tooltip("Spawn Delay In Real-Time Seconds")] float spawnFrequancy;

    Dictionary<EnemyType, float> ProbabilityDictionary;
    private void Awake()
    {
        ProbabilityDictionary = new Dictionary<EnemyType, float>
        {
            {EnemyType.MANAGER, 1 },
            {EnemyType.POLICE, 0},
            {EnemyType.ARMY, 0},
            {EnemyType.STACY, 0 },
            {EnemyType.SUPREME, 0 }
        };

    }

    

    void StartSpawn()
    {

       
    }
    IEnumerator StartSpawning()
    {
        yield return null;
    }

    void SelectEnemyToSpawn(EnemyType type)
    {
       
    }
}
