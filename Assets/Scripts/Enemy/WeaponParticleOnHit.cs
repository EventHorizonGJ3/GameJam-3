using System;
using UnityEngine;

public class WeaponParticleOnHit : ParticlePooler<ParticleEntity>
{
	public static Action<Vector3> OnHitPos;
	protected override void Awake()
	{
		base.Awake();
	}
	private void OnEnable()
	{
		OnHitPos += SpawnAtPos;
	}
	private void OnDisable()
	{
		OnHitPos -= SpawnAtPos;
	}
	protected override void SpawnAtPos(Vector3 pos)
	{
		base.SpawnAtPos(pos);
	}
}