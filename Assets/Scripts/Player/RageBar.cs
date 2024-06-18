using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageBar : MonoBehaviour
{
	public static Action Rage;
	public static Action<int> OnBerserkActivate;
	[SerializeField] int BerserkExtraDmg;
	[SerializeField] int healAmount;
	[SerializeField] float fullBarAmount;
	[SerializeField] float berserkCost;
	[SerializeField] Image bar;
	[SerializeField] float ResetTimer;
	float currentRage = 99;
	ActionMap inputs;
	Vector2 lastRage;
	float lastTot;
	Coroutine coroutine;

	private void Awake()
	{
		inputs = InputManager.ActionMap;
		currentRage = 0;
	}
	private void OnEnable()
	{
		inputs.Player.Berserk.performed += ActivateBerserk;
		Rage += AddRage;
	}
	private void OnDisable()
	{
		inputs.Player.Berserk.performed -= ActivateBerserk;
		Rage -= AddRage;
	}

	private void AddRage()
	{
		Debug.Log("WOW");
		lastRage.y = lastRage.x;
		lastRage.x = lastTot;
		lastTot = lastRage.x + lastRage.y;
		currentRage += lastTot;
		if (currentRage > fullBarAmount)
		{
			currentRage = fullBarAmount;
		}
		UpdateBar();

		//coroutine = StartCoroutine(ResetRage());
	}

	// private string ResetRage()
	// {

	// }

	private void UpdateBar()
	{
		bar.fillAmount = Mathf.Lerp(0, 1, currentRage / fullBarAmount);
	}

	private void ActivateBerserk(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		if (currentRage - berserkCost < 0)
			return;

		currentRage -= berserkCost;
		OnBerserkActivate?.Invoke(BerserkExtraDmg);
	}
}
