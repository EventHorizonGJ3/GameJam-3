using System.Collections;
using UnityEngine;
public class BossMovement : EnemyMovement
{
	[SerializeField] BossesSo bossSo;

	private void Awake()
	{
		TryGetComponent(out agent);
	}

	private void OnEnable()
	{
		GameManager.OnPause += Pause;
        GameManager.OnWin += Endgame;
        GameManager.OnLose += Endgame;
        bossSo.OnSpawn?.Invoke(HP);
	}

	private void OnDisable()
	{
		GameManager.OnPause -= Pause;
        GameManager.OnWin -= Endgame;
        GameManager.OnLose -= Endgame;
        canMove = true;
		isKnockbacked = false;
	}

	private void Pause()
	{
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
		combo.CheckAttack(out canMove);
	}

	public override void Knockback(float _Power)
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

	public override void TakeDamage(float _Dmg)
	{
		HP -= _Dmg;
		Score.OnDmg?.Invoke(_Dmg);
		RageBar.OnRage?.Invoke();
		bossSo.OnHit?.Invoke(HP);
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

	public override void NoHP()
	{
		isKnockbacked = false;
		knockbackTimer = 0;
		canMove = true;
		StopAllCoroutines();
		GameManager.EnemyDeath?.Invoke();
		EnemyPoolerOnDeath.GetEnemyPosOnDeath?.Invoke(transform.position + Vector3.up * 0.5f);
		gameObject.SetActive(false);
	}

	protected override IEnumerator HitStager()
	{
		isStaggered = false;
		yield return new WaitForSeconds(stagerDur);
		isStaggered = true;
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