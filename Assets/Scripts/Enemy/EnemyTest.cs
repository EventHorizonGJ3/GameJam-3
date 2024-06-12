using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour, IEnemy
{
    EnemyType enemyType = EnemyType.MANAGER;
    NavMeshAgent agent;

    public EnemyType Type { get => enemyType; private set => enemyType = value; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyType = EnemyType.MANAGER;
    }

    private void Update()
    {
        agent.SetDestination(GameManager.enemyTargetPosition);
    }
}
