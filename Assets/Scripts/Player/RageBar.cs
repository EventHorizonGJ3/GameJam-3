using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RageBar : MonoBehaviour
{
	[Header("Berserk settings: ")]
	[SerializeField] float berserkDuration;
	[SerializeField] float berserkCost;
	[SerializeField] float berserkCooldown;
	[SerializeField] int BerserkExtraDmg;
	[SerializeField] float healAmount;
	[Tooltip("make this < or = to berserkDuration")]
	[SerializeField] float healDuration;

	[Header("RageBar settings: ")]
	[SerializeField] Image rageBar;
	[SerializeField] float fullBarAmount;

	[Tooltip("time after wich it starst decresing")]
	[SerializeField] float barResetTimer;

	[Tooltip("quanto toglie ogni secondo")]
	[SerializeField] float barResetDecrese;

	[Tooltip("increments by value barReserDecrese every seconds the bar resets")]
	[SerializeField] float secondExtraDecrese;

	public static Action OnRage;
	public static Action<int> OnBerserkExtraDmg;
	public static Action<float, float> OnBerserkHeal;

	float lastBerserkTime;
	float currentBar = 0;
	float startDecrese;
	ActionMap inputs;
	Vector2 lastRage;
	float lastTot = 1;
	bool isBerserkActive;
	Coroutine resetCoroutine;

	private void Awake()
	{
		inputs = InputManager.ActionMap;
		currentBar = 0;
		startDecrese = barResetDecrese;
		rageBar.fillAmount = currentBar / fullBarAmount;
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
			if (Time.time - lastBerserkTime >= berserkDuration)
			{
				isBerserkActive = false;
			}
		}

		rageBar.fillAmount = currentBar / fullBarAmount;
	}
	private void AddRage()
	{
		if (isBerserkActive)
			return;

		if (resetCoroutine != null)
		{
			StopCoroutine(resetCoroutine);
			barResetDecrese = startDecrese;
			resetCoroutine = null;
		}

		lastRage.y = lastRage.x;
		lastRage.x = lastTot;
		lastTot = lastRage.x + lastRage.y;
		currentBar += lastTot;

		if (currentBar > fullBarAmount)
		{
			currentBar = fullBarAmount;
		}

		resetCoroutine = StartCoroutine(ResetRage());
	}

	private IEnumerator ResetRage()
	{
		yield return new WaitForSeconds(barResetTimer);

		lastRage = Vector2.zero;
		lastTot = 1;

		while (currentBar > 0)
		{
			currentBar -= barResetDecrese * Time.deltaTime;
			barResetDecrese += secondExtraDecrese * Time.deltaTime;
			yield return null;
		}
		barResetDecrese = startDecrese;

		currentBar = 0;
	}

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
		OnBerserkHeal?.Invoke(healAmount, healDuration);
	}
}
