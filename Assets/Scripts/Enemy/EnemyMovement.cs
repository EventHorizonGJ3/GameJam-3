using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;


public class EnemyMovement : MonoBehaviour, IEnemy, IDamageable
{
	[Header("References:")]
	protected NavMeshAgent agent;
	public Transform colliderTransform { get; set; }
	public Transform Transform { get => transform; }

	[field: Header("Settings: ")]
	[field: SerializeField] public float HP { get; set; }
	[SerializeField] protected EnemyType enemyType;

	[Header("Stager Settings: ")]
	[SerializeField, Min(1f)] protected float stagerDur;

	protected float startHP;


	public EnemyType Type { get => enemyType; private set => enemyType = value; }
	[Header("knockback Settings")]
	[SerializeField] protected float knockbackDur = 3;
	[SerializeField] protected float backRayHight;
	[SerializeField] protected float backRayLenght;
	[SerializeField] protected LayerMask obstacleLayer;
	[SerializeField] protected EnemyCombo combo;
	protected float backPower;
	protected float knockbackTimer = 0;
	protected bool isKnockbacked;
	protected Vector3 startPos;
	protected Vector3 endPos;
	protected Vector3 dir;
	protected bool canMove = true;
	protected bool isStaggered = false;
	protected Coroutine stager;


	private void Awake()
	{
		TryGetComponent(out agent);
		startHP = HP;
	}

	private void OnEnable()
	{
		GameManager.OnPause += Pause;
		GameManager.OnWin += Endgame;
		GameManager.OnLose += Endgame;
	}

	private void OnDisable()
	{
		HP = startHP;
		GameManager.OnPause -= Pause;
        GameManager.OnWin -= Endgame;
        GameManager.OnLose -= Endgame;
        canMove = true;
		isKnockbacked = false;
	}

	private void Pause()
	{
		if (gameObject.activeInHierarchy == false)
			return;

		if (GameManager.gameOnPause)
		{
			agent.SetDestination(transform.position);
		}
	}

	void Endgame()
	{
		isStaggered = true;
	}
		

	private void Update()
	{
		if (GameManager.gameOnPause)
			return;
		if (isStaggered)
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
		Vector3 _lookAtPos = new Vector3(GameManager.enemyTargetPosition.position.x, transform.position.y, GameManager.enemyTargetPosition.position.z);
		transform.LookAt(_lookAtPos, Vector3.up);
		combo?.CheckAttack(out canMove); //Added "?" ByEma
	}

	public virtual void Knockback(float _Power)
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

	public virtual void NoHP()
	{
		isStaggered = false;
		isKnockbacked = false;
		knockbackTimer = 0;
		canMove = true;
		StopAllCoroutines();
		GameManager.EnemyDeath?.Invoke();
		EnemyPoolerOnDeath.GetEnemyPosOnDeath?.Invoke(transform.position + Vector3.up * 0.5f); ;
		gameObject.SetActive(false);
	}

	public virtual void TakeDamage(float _Dmg)
	{
		//Debug.Log(_Dmg);
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

	protected virtual IEnumerator HitStager()
	{
		isStaggered = true;
		yield return new WaitForSeconds(stagerDur);
		isStaggered = false;
	}

}

// states -> chaseing and attacking 
