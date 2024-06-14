using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour, IDamageable
{
	[SerializeField] float knockbackDur = 3;
	[field: SerializeField] public int HP { get; set; }
	public Transform colliderTransform { get; set; }

	[Header("Yello ray settings: ")]
	[SerializeField] float rayHight;
	[SerializeField] float rayLenght;
	[SerializeField] LayerMask obstacleLayer;
	NavMeshAgent agent;
	bool isKnockbacked;
	float timer = 0;
	float backPower;
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
			if (Physics.Raycast(transform.position + Vector3.up * rayHight, dir, out RaycastHit hit, rayLenght, obstacleLayer))
			{
				Debug.Log("i hit this: ", hit.transform);
				isKnockbacked = false;
				backPower = 0;
				return;
			}

			if (timer < knockbackDur)
			{
				Debug.Log($"startPos: {startPos} \nendPos: {endPos}");
				timer += Time.deltaTime;
				transform.position = Vector3.Lerp(startPos, endPos, timer / knockbackDur);
			}
			else
			{
				isKnockbacked = false;
				transform.position = endPos;
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
		isKnockbacked = false;

		timer = 0;
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
		Destroy(gameObject);
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
		Gizmos.DrawRay(transform.position + Vector3.up * rayHight, dir * rayLenght);
		Gizmos.color = Color.magenta;
		Debug.DrawRay(transform.position + Vector3.up * rayHight, dir * backPower);
	}

}
