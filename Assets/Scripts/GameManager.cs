using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class GameManager : MonoBehaviour
{
	//Events

	public static Action OnPause;
	public static Action OnResume;
	public static Action OnWin;
	public static Action OnLose; // done
	public static Action EnemyDeath;


	public static bool PlayerIsAttacking;
	public static bool IsHoldingMelee;
	public static bool IsHoldingRanged;
	public static Transform enemyTargetPosition;

	public static bool gameOnPause;

	public static bool usingGamePad;

	//Stats
	public static int enemyKilled;

	// score ?

	private void Awake()
	{
		Application.targetFrameRate = 100;
	}

	private void Start()
	{
		gameOnPause = false; usingGamePad = false;
		InputManager.SwitchPlayerInputs();
		InputManager.ActionMap.UI_Toggle.Enable();
		Time.timeScale = 1f;
	}
	private void OnEnable()
	{
		InputManager.ActionMap.UI_Toggle.Pause.performed += PauseFunction;
		OnResume += UnPauseByUI;
		EnemyDeath += OnEnemyDeath;
		OnWin += EndGame;
		OnLose += EndGame;
	}
	private void OnDisable()
	{
		InputManager.ActionMap.UI_Toggle.Pause.performed -= PauseFunction;
		OnResume -= UnPauseByUI;
		EnemyDeath -= OnEnemyDeath;
		OnWin -= EndGame;
        OnLose -= EndGame;
    }

	private void OnEnemyDeath()
	{
		enemyKilled++;
	}

	void PauseFunction(InputAction.CallbackContext used)
	{
		var usedDevice = used.control;
		if (usedDevice.device is Gamepad) usingGamePad = true; else usingGamePad = false;

		gameOnPause = !gameOnPause;

		if (gameOnPause) InputManager.SwitchToUIInputs(); Time.timeScale = 0; 
		if (!gameOnPause) InputManager.SwitchPlayerInputs(); Time.timeScale = 1;
		OnPause?.Invoke();

		//Debug.Log("uso del gamepad:" + usingGamePad);
	}

	void UnPauseByUI()
	{
		gameOnPause = !gameOnPause;

		if (gameOnPause) InputManager.SwitchToUIInputs(); Time.timeScale = 0;
		if (!gameOnPause) InputManager.SwitchPlayerInputs(); Time.timeScale = 1;
		OnPause?.Invoke();
	}

	void EndGame()
	{
        InputManager.SwitchToUIInputs();
		Time.timeScale = 0;
    }


}
