using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BossesSo", menuName = "BossesSo", order = 0)]
public class BossesSo : ScriptableObject
{
	public Image hpBar;
	public float hpBarHight;
	public float hpBarWidth;
	[HideInInspector] public float StartHP;

	public Action<float> OnHit;
	public Action<float> OnSpawn;

}