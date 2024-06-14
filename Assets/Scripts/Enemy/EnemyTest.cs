using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour, IEnemy
{
    EnemyType enemyType = EnemyType.MANAGER;
    NavMeshAgent agent;

    public EnemyType Type { get => enemyType; private set => enemyType = value; }
    public Transform Transform { get => transform; }

    private void Awake()
    {
        //agent = GetComponent<NavMeshAgent>();
    }
        

    private void Update()
    {
        //agent.SetDestination(GameManager.enemyTargetPosition);
    }
}
