using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class UI_MainMenu_Manager : MonoBehaviour
{
    //Refs
    [Header("References")]
    [SerializeField] TMP_Text init_text;
    [SerializeField] GameObject mainMenu;
    [SerializeField] RectTransform playButton;
    [SerializeField] RectTransform optionsButton;
    [SerializeField] RectTransform controlsButton;
    [SerializeField] RectTransform creditsButton;
    [SerializeField] RectTransform quitButton;

    bool init;
    bool usingGamepad;

    private void Awake()
    {
        InputManager.SwitchToUIInputs();
        InputManager.ActionMap.UI_Toggle.AnyKey.Enable();
    }

    private void OnEnable()
    {
        InputManager.ActionMap.UI_Toggle.Pause.started += CheckTypeOfDevice;
        InputManager.ActionMap.UI_Toggle.AnyKey.started += CheckTypeOfDevice;
    }
    private void OnDisable()
    {
        InputManager.ActionMap.UI_Toggle.Pause.started -= CheckTypeOfDevice;
        InputManager.ActionMap.UI_Toggle.AnyKey.started -= CheckTypeOfDevice;
    }





    void CheckTypeOfDevice(InputAction.CallbackContext context)
    {
        var device = context.control.device;
        if( device is Gamepad)
        {
            init = true;
            usingGamepad = true;
        }
        else
        {
            init = true;
            usingGamepad = false;
        }
        InitMenu();
        InputManager.ActionMap.UI_Toggle.AnyKey.Disable();
    }

    void InitMenu()
    {
        //TODO: play a sound for pressing start
        init_text.gameObject.SetActive(false);
        mainMenu.SetActive(true);
        if (usingGamepad) EventSystem.current.SetSelectedGameObject(playButton.gameObject);
        
    }
}
