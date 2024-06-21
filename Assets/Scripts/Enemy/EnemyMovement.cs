using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class EnemyMovement : MonoBehaviour, IEnemy, IDamageable
{
	[Header("References:")]
	NavMeshAgent agent;
	public Transform colliderTransform { get; set; }
	public Transform Transform { get => transform; }

	[field: Header("Settings: ")]
	[field: SerializeField] public float HP { get; set; }
	[SerializeField] EnemyType enemyType;

	[Header("Stager Settings: ")]
	[SerializeField] float stagerDur;


	public EnemyType Type { get => enemyType; private set => enemyType = value; }
	[Header("knockback Settings")]
	[SerializeField] float knockbackDur = 3;
	[SerializeField] float backRayHight;
	[SerializeField] float backRayLenght;
	[SerializeField] LayerMask obstacleLayer;
	[SerializeField] EnemyCombo combo;
	float backPower;
	float knockbackTimer = 0;
	bool isKnockbacked;
	Vector3 startPos;
	Vector3 endPos;
	Vector3 dir;
	bool canMove = true;
	Coroutine stager;


	private void Awake()
	{
		TryGetComponent(out agent);
	}

	private void OnEnable()
	{
		GameManager.OnPause += Pause;
	}

	private void OnDisable()
	{
		GameManager.OnPause += Pause;
	}

	private void Pause()
	{
		if (GameManager.gameOnPause)
		{
			agent.SetDestination(transform.position);
		}
	}

	private void Update()
	{
		if (GameManager.gameOnPause)
			return;

		if (isKnockbacked)
		{
			if (knockbackTimer < knockbackDur)
			{
				knockbackTimer += Time.deltaTime;
				transform.position = Vector3.Lerp(startPos, endPos, knockbackTimer / knockbackDur);
			}
			else
			{
				isKnockbacked = false;
				transform.position = endPos;
				backPower = 0;
				agent.enabled = true;
			}
			return;
		}
		if (canMove)
		{
			agent.SetDestination(GameManager.enemyTargetPosition.position);
		}
		else
		{
			agent.SetDestination(transform.position);
		}
		transform.LookAt(GameManager.enemyTargetPosition);
		combo.CheckAttack(out canMove);
	}

	public void Knockback(float _Power)
	{
		if (_Power <= 0)
			return;
		isKnockbacked = false;

		knockbackTimer = 0;
		startPos = transform.position;
		Vector3 _ColliderStartPos = colliderTransform.position;
		dir = (startPos - _ColliderStartPos).normalized;
		dir.y = 0;
		backPower = _Power;
		endPos = startPos + dir * _Power;

		isKnockbacked = true;
		// ("kncokback");
	}

	public void NoHP()
	{
		isKnockbacked = false;
		knockbackTimer = 0;
		canMove = true;
		StopAllCoroutines();
		gameObject.SetActive(false);
	}

	public void TakeDamage(int _Dmg)
	{
		Debug.Log(_Dmg);
		HP -= _Dmg;
		Score.OnDmg?.Invoke(_Dmg);
		RageBar.OnRage?.Invoke();
		if (stager != null)
		{
			StopCoroutine(stager);
			stager = null;
		}
		stager = StartCoroutine(HitStager());

		if (HP <= 0)
		{
			NoHP();
		}
	}

	private IEnumerator HitStager()
	{
		canMove = false;
		yield return new WaitForSeconds(stagerDur);
		canMove = true;
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position + Vector3.up * backRayHight, dir * backRayLenght);
		Gizmos.color = Color.magenta;
		Debug.DrawRay(transform.position + Vector3.up * backRayHight, dir * backPower);
	}

#endif

}

// states -> chaseing and attacking 
