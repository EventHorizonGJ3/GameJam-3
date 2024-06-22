using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StacySpawner : MonoBehaviour
{
    //event
    public static Action OnStacy;
    IEnemy enemyToSpawn;

    private void OnEnable()
    {
        OnStacy += SpawnStacy;
    }
    private void OnDisable()
    {
        OnStacy -= SpawnStacy;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        enemyToSpawn = EnemyPooler.Instance.GetBoss(EnemyType.STACY);
        
    }

    void SpawnStacy()
    {
        enemyToSpawn.Transform.position = transform.position;
        enemyToSpawn.Transform.gameObject.SetActive(true);
    }
}
