using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour, IDamageable
{
	[SerializeField] float invincibilityTIme;
	[SerializeField] float knockbackDur = 3;
	[field: SerializeField] public int HP { get; set; }
	[Header("Yello ray settings: ")]
	[SerializeField] float rayHight;
	[SerializeField] float rayLenght;
	NavMeshAgent agent;
	bool isKnockbacked;
	float timer = 0;
	float backPower;
	Vector3 startPos;
	float lastHitTime;


	private void Awake()
	{
		TryGetComponent(out agent);
	}

	private void Update()
	{
		if (isKnockbacked)
		{
			if (Physics.Raycast(transform.position + Vector3.up * rayHight, -transform.forward, out RaycastHit hit, rayLenght))
			{
				Debug.Log("i hit this: ", hit.transform);
				isKnockbacked = false;
				backPower = 0;
				return;
			}

			if (timer < knockbackDur)
			{
				timer += Time.deltaTime;
				transform.position = Vector3.Lerp(startPos, startPos - transform.forward * backPower, timer / knockbackDur);
			}
			else
			{
				isKnockbacked = false;
				transform.position = startPos - transform.forward * backPower;
				backPower = 0;
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
		Debug.Log("kncokback");
		startPos = transform.position;
		backPower = _Power;
		//agent.enabled = false;
		isKnockbacked = true;
	}

	public void NoHP()
	{
		Destroy(gameObject);
	}

	public void TakeDamage(int _Dmg)
	{
		if (Time.time - lastHitTime < invincibilityTIme)
			return;

		HP -= _Dmg;
		lastHitTime = Time.time;
		if (HP <= 0)
		{
			NoHP();
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position + Vector3.up * rayHight, -transform.forward * rayLenght);
	}

}
