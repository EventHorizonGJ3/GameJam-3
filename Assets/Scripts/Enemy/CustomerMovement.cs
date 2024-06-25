using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMovement : EnemyMovement
{
	[SerializeField] Transform[] endPoints;
	Transform endPoint;

	private void Awake()
	{
		TryGetComponent(out agent);
		startHP = HP;

	}

	private void OnEnable()
	{
		GameManager.OnPause += Pause;
		float minDist = 999999f;
		foreach (var point in endPoints)
		{
			float dist = Vector3.Distance(transform.position, point.position);
			if (dist < minDist)
			{
				minDist = dist;
				endPoint = point;
			}
		}

		agent.SetDestination(endPoint.position);
	}

	private void OnDisable()
	{
		HP = startHP;
		GameManager.OnPause += Pause;
		canMove = true;
		isKnockbacked = false;
	}


	public override void Knockback(float _Power)
	{
		base.Knockback(_Power);
	}
	public override void NoHP()
	{
		isStaggered = false;
		isKnockbacked = false;
		knockbackTimer = 0;
		canMove = true;
		StopAllCoroutines();
		EndStats.EnemyDeath?.Invoke();
		gameObject.SetActive(false);
	}
	protected override IEnumerator HitStager()
	{
		return base.HitStager();
	}
	public override void TakeDamage(float _Dmg)
	{
		base.TakeDamage(_Dmg);
	}
	private void Pause()
	{
		if (GameManager.gameOnPause)
		{
			agent.SetDestination(transform.position);
		}
		else
		{
			agent.SetDestination(endPoint.position);
		}
	}
}
