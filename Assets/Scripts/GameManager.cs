using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class GameManager : MonoBehaviour
{
    //Events
    public static Action OnPause;

    public static bool PlayerIsAttacking;
    public static bool IsHoldingMelee;
    public static bool IsHoldingRanged;
    public static Vector3 enemyTargetPosition;

    public static bool gameOnPause;

    private void Awake()
    {
        Application.targetFrameRate = 100;
    }
    private void OnEnable()
    {
        InputManager.ActionMap.UI_Toggle.Pause.performed += PauseFunction;
    }
    private void OnDisable()
    {
        InputManager.ActionMap.UI_Toggle.Pause.performed -= PauseFunction;
    }

    void PauseFunction(InputAction.CallbackContext context)
    {
        gameOnPause = !gameOnPause;
        
        if(gameOnPause ) InputManager.SwitchToUIInputs(); OnPause?.Invoke(); Time.timeScale = 0;
        if(!gameOnPause ) InputManager.SwitchPlayerInputs(); OnPause?.Invoke(); Time.timeScale = 1;
    }

  
}
