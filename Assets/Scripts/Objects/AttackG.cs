using UnityEngine;
[System.Serializable]
public class AttackG
{
	public AnimatorOverrideController AnimOverrider;
	[Tooltip("at what percentege of the animation does the attack reach it's end ")]
	public float PeakAnimPercent = 0.6f;
	public float AttackRadius;
	public int Dmg;
}