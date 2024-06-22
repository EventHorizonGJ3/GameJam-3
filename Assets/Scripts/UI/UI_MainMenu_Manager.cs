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
    [Header("References: BUTTONS")]
    [SerializeField] TMP_Text init_text;
    [SerializeField] RectTransform playButton;
    [SerializeField] RectTransform optionsButton;
    [SerializeField] RectTransform controlsButton;
    [SerializeField] RectTransform creditsButton;
    [SerializeField] RectTransform quitButton;
    [Header("Canvas")]
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject controlsMenu;
    [SerializeField] GameObject creditsMenu;
    [SerializeField] GameObject prologueCanvas;



    bool init;
    bool usingGamepad;

    private void Awake()
    {
        InputManager.ActionMap.UI_Toggle.Enable();
        InputManager.ActionMap.Player.Disable();
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

    private void Start()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        prologueCanvas.SetActive(false);
        
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
        InputManager.ActionMap.UI_Toggle.Disable();
    }

    void InitMenu()
    {
        //TODO: play a sound for pressing start
        init_text.gameObject.SetActive(false);
        mainMenu.SetActive(true);
        if (init && usingGamepad) EventSystem.current.SetSelectedGameObject(playButton.transform.gameObject);
        else if(init && !usingGamepad) EventSystem.current.SetSelectedGameObject(null);
        Debug.Log(EventSystem.current);

    }
}
