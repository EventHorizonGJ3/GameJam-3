using UnityEngine;
using UnityEngine.AI;


public class EnemyMovement : MonoBehaviour, IEnemy, IDamageable
{
	[Header("References:")]
	NavMeshAgent agent;
	public Transform colliderTransform { get; set; }
	public Transform Transform { get => transform; }

	[field: Header("Settings: ")]
	[field: SerializeField] public int HP { get; set; }
	EnemyType enemyType = EnemyType.MANAGER;
	public EnemyType Type { get => enemyType; private set => enemyType = value; }
	[SerializeField] WeaponsSO weaponScriptable;

	[Header("knockback Settings")]
	[SerializeField] float knockbackDur = 3;
	[SerializeField] float backRayHight;
	[SerializeField] float backRayLenght;
	[SerializeField] LayerMask obstacleLayer;
	float backPower;
	float knockbackTimer = 0;
	bool isKnockbacked;
	Vector3 startPos;
	Vector3 endPos;
	Vector3 dir;

	private void Awake()
	{
		TryGetComponent(out agent);
	}

	private void Update()
	{
		if (isKnockbacked)
		{
			if (Physics.Raycast(transform.position + Vector3.up * backRayHight, dir, out RaycastHit hit, backRayLenght, obstacleLayer))
			{
				Debug.Log("i hit this: ", hit.transform);
				isKnockbacked = false;
				backPower = 0;
				return;
			}

			if (knockbackTimer < knockbackDur)
			{
				Debug.Log($"startPos: {startPos} \nendPos: {endPos}");
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
		}
		else
		{
			weaponScriptable.StartAttack?.Invoke();
			agent.SetDestination(GameManager.enemyTargetPosition.position);
		}


	}
	public void Knockback(float _Power)
	{
		isKnockbacked = false;

		knockbackTimer = 0;
		startPos = transform.position;
		Vector3 _ColliderStartPos = colliderTransform.position;
		dir = (startPos - _ColliderStartPos).normalized;
		dir.y = 0;
		backPower = _Power;

		endPos = startPos + dir * _Power;

		isKnockbacked = true;
		//Debug.Log("kncokback");
	}

	public void NoHP()
	{
		gameObject.SetActive(false);
	}

	public void TakeDamage(int _Dmg)
	{
		//// if (Time.time - lastHitTime < invincibilityTIme)
		//// 	return;
		//// lastHitTime = Time.time; 

		HP -= _Dmg;
		if (HP <= 0)
		{
			NoHP();
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position + Vector3.up * backRayHight, dir * backRayLenght);
		Gizmos.color = Color.magenta;
		Debug.DrawRay(transform.position + Vector3.up * backRayHight, dir * backPower);
	}


}

// states -> chaseing and attacking 
