using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageBar : MonoBehaviour
{
	public static Action OnRage;
	public static Action<int> OnBerserkExtraDmg;
	public static Action<float, float> OnBerserkHeal;
	[Header("Berserk settings: ")]
	[SerializeField] float berserkDuration;
	[SerializeField] float berserkCooldown;
	[SerializeField] int BerserkExtraDmg;
	[SerializeField] float healAmount;
	[Tooltip("make this < or = to berserkDuration")]
	[SerializeField] float healDuration;

    [Header("RageBar settings: ")]
	[SerializeField] Image bar;
	[SerializeField] float fullBarAmount;
	[SerializeField] float berserkCost;
	[SerializeField] float barResetTimer;
	

    float lastBerserkTime;
	float currentBar = 0;
	ActionMap inputs;
	Vector2 lastRage;
	float lastTot = 1;
	bool isBerserkActive;
	Coroutine coroutine;

	private void Awake()
	{
		inputs = InputManager.ActionMap;
		currentBar = 0;
	}
	private void OnEnable()
	{
		inputs.Player.Berserk.performed += ActivateBerserk;
		OnRage += AddRage;
	}
	private void OnDisable()
	{
		inputs.Player.Berserk.performed -= ActivateBerserk;
		OnRage -= AddRage;
	}
    private void Update()
    {
        if (isBerserkActive)
        {
			if(Time.time-lastBerserkTime >= berserkDuration)
            {
				isBerserkActive = false;
            }
        }
		bar.fillAmount = currentBar / fullBarAmount;
	}
    private void AddRage()
 	{
		if (isBerserkActive)
			return;

		Debug.Log("WOW");
		lastRage.y = lastRage.x;
		lastRage.x = lastTot;
		lastTot = lastRage.x + lastRage.y;
		currentBar += lastTot;
		if (currentBar > fullBarAmount)
		{
			currentBar = fullBarAmount;
		}

		//coroutine = StartCoroutine(ResetRage());
	}

	// private string ResetRage()
	// {

	// }

	

	private void ActivateBerserk(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		if (Time.time - lastBerserkTime < berserkCooldown)
			return;

		if (currentBar - berserkCost < 0)
			return;
		currentBar -= berserkCost;
		lastBerserkTime = Time.time;
		isBerserkActive = true;
		OnBerserkExtraDmg?.Invoke(BerserkExtraDmg);
		OnBerserkHeal?.Invoke(healAmount,healDuration);
	}
}
