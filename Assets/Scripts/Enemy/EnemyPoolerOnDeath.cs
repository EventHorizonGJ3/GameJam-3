using System;
using UnityEngine;

public class EnemyPoolerOnDeath : ParticlePooler<ParticleEntity>
{
	public static Action<Vector3> GetEnemyPosOnDeath;
	private void OnEnable()
	{
		GetEnemyPosOnDeath += SpawnAtPos;
	}

	private void OnDisable()
	{
		GetEnemyPosOnDeath -= SpawnAtPos;
	}

	protected override void Awake()
	{
		base.Awake();
	}
	protected override void SpawnAtPos(Vector3 pos)
	{
		base.SpawnAtPos(pos);
	}

}