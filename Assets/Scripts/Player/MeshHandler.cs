using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshHandler : MonoBehaviour
{
	[Header("Parameters")]
	[SerializeField] float rotationSpeed;
	[Header("Animators")]
	[SerializeField] RuntimeAnimatorController meleeAnimator;
	[SerializeField] RuntimeAnimatorController rangedAnimator;

	Animator anim;
	int currentComboStep;
	int maxComboSteps;
	bool isAttacking;
	bool canReceiveInput;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	private void Start()
	{
		anim.runtimeAnimatorController = meleeAnimator;
		maxComboSteps = 3;
	}

	private void FixedUpdate()
	{
		if (InputManager.IsMoving(out Vector3 direction))
		{
			Quaternion rotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * rotationSpeed);
		}
	}

	public void StartCombo()
	{
		if (anim.runtimeAnimatorController == meleeAnimator && !isAttacking)
		{
			currentComboStep = 0;
			isAttacking = true;
			PlayAnimationByIndex(currentComboStep);
			StartCoroutine(ComboCoroutine());
		}
		else if (isAttacking)
		{
			canReceiveInput = true;
		}
	}
	public void PlayAnimationByIndex(int index)
	{
		ResetTriggers();
		if (index == 0)
		{
			anim.SetTrigger("Attack1");
		}
		else if (index == 1)
		{
			anim.SetTrigger("Attack2");
		}
		else if (index == 2)
		{
			anim.SetTrigger("Attack3");
		}
		canReceiveInput = false;
	}


	private IEnumerator ComboCoroutine()
	{
		while (isAttacking)
		{
			AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);


			while (stateInfo.normalizedTime < 0.8f)
			{
				stateInfo = anim.GetCurrentAnimatorStateInfo(0);
				yield return null;

				if (GameManager.PlayerIsAttacking && canReceiveInput)
				{
					AdvanceCombo();
					yield break;
				}
			}


			while (stateInfo.normalizedTime < 1f)
			{
				stateInfo = anim.GetCurrentAnimatorStateInfo(0);
				yield return null;
			}


			isAttacking = false;
			anim.SetTrigger("Idle");
		}
	}

	private void AdvanceCombo()
	{
		if (currentComboStep < maxComboSteps - 1)
		{
			currentComboStep++;
		}
		else
		{
			currentComboStep = 0;
		}
		PlayAnimationByIndex(currentComboStep);
		StartCoroutine(ComboCoroutine());
	}

	private void ResetTriggers()
	{
		anim.ResetTrigger("Attack1");
		anim.ResetTrigger("Attack2");
		anim.ResetTrigger("Attack3");
		anim.ResetTrigger("Idle");
		canReceiveInput = true;
	}
}
