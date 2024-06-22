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

    public static bool PlayerIsAttacking;
    public static bool IsHoldingMelee;
    public static bool IsHoldingRanged;
    public static Transform enemyTargetPosition;

    public static bool gameOnPause;

    public static bool usingGamePad;

    private void Awake()
    {
        Application.targetFrameRate = 100;
    }

    private void Start()
    {
        gameOnPause = false; usingGamePad = false;
        InputManager.SwitchPlayerInputs();
        InputManager.ActionMap.UI_Toggle.Enable();
    }
    private void OnEnable()
    {
        InputManager.ActionMap.UI_Toggle.Pause.performed += PauseFunction;
        OnResume += UnPauseByUI;
    }
    private void OnDisable()
    {
        InputManager.ActionMap.UI_Toggle.Pause.performed -= PauseFunction;
        OnResume -= UnPauseByUI;
    }

    void PauseFunction(InputAction.CallbackContext used)
    {
        var usedDevice = used.control;
        if (usedDevice.device is Gamepad) usingGamePad = true; else usingGamePad = false;

        gameOnPause = !gameOnPause;
        
        if(gameOnPause ) InputManager.SwitchToUIInputs();Time.timeScale = 0;
        if(!gameOnPause ) InputManager.SwitchPlayerInputs();Time.timeScale = 1;
        OnPause?.Invoke();

        Debug.Log("uso del gamepad:"+usingGamePad);
    }

    void UnPauseByUI()
    {
        gameOnPause = !gameOnPause;

        if (gameOnPause) InputManager.SwitchToUIInputs(); Time.timeScale = 0;
        if (!gameOnPause) InputManager.SwitchPlayerInputs(); Time.timeScale = 1;
        OnPause?.Invoke();
    }

  
}
