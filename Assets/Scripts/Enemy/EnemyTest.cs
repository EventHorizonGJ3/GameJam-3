using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour,IDamageable
{
   NavMeshAgent agent;
    bool isKnockbacked;
    float dur = 3;
    float timer = 0;
    float backPower;
    Vector3 startPos;

   [field: SerializeField] public int HP { get ; set ; }

    private void Awake()
    {
        //agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (isKnockbacked)
        {
            if(timer< dur)
            {
                timer += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, startPos - transform.forward * backPower,timer/dur);
            }
            else
            {
                isKnockbacked = false;
                transform.position = startPos - transform.forward * backPower;
                //agent.enabled = true;
            }
        }
        else
        {
            //agent.SetDestination(GameManager.enemyTargetPosition);
        }


    }
    public void Knockback(float _Power)
    {
        isKnockbacked = true;
        startPos = transform.position;
        backPower = _Power;
        //agent.enabled = false;
    }

    public void NoHP()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int _Dmg)
    {
        HP -= _Dmg;
        if (HP <= 0)
        {
            NoHP();
        }
    }

}
