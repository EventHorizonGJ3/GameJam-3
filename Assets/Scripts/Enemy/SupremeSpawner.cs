using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupremeSpawner : MonoBehaviour
{
    //event
    public static Action OnFinalBoss;

    IEnemy enemyToSpawn;

    private void OnEnable()
    {
        OnFinalBoss += SpawnFinalBoss;
    }
    private void OnDisable()
    {
        OnFinalBoss -= SpawnFinalBoss;
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        enemyToSpawn = EnemyPooler.Instance.GetBoss(EnemyType.SUPREME);
    }

    void SpawnFinalBoss()
    {
        enemyToSpawn.Transform.position = transform.position;
        enemyToSpawn.Transform.gameObject.SetActive(true);
    }
}
