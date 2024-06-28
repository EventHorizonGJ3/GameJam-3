using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AttackG
{
	public AnimatorOverrideController AnimOverrider;
	public int Dmg;
	[HideInInspector] public float clipLenght;

	public void GetLengts ()
	{
		clipLenght = AnimOverrider.animationClips[0].length;
	}
}