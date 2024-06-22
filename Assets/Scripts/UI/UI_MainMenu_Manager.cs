using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

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
    [SerializeField] GameObject preselectedOnSettings;
    [Header("Canvas")]
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject mainButtons;
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] GameObject controlsCanvas;
    [SerializeField] GameObject creditsCanvas;
    [SerializeField] GameObject prologueCanvas;

    AudioManager sound;

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
        mainButtons.SetActive(false);
        settingsCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        prologueCanvas.SetActive(false);
        sound = AudioManager.instance;
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
        mainButtons.SetActive(true);
        if (init && usingGamepad) EventSystem.current.SetSelectedGameObject(playButton.transform.gameObject);
        else if(init && !usingGamepad) EventSystem.current.SetSelectedGameObject(null);
        Debug.Log(EventSystem.current);

    }

    public void OptionButton()
    {
        sound.PlaySFX(sound.AudioData.SFX_UIMAIN_ButtonClick);
        mainButtons.SetActive(false);
        settingsCanvas.SetActive(true);
        if(usingGamepad) EventSystem.current.SetSelectedGameObject(preselectedOnSettings.gameObject);
    }
    public void CreditsButton()
    {
        sound.PlaySFX(sound.AudioData.SFX_UIMAIN_ButtonClick);
        mainButtons.SetActive(false);
        creditsCanvas.SetActive(true);
    }
    public void ControlsButton()
    {
        sound.PlaySFX(sound.AudioData.SFX_UIMAIN_ButtonClick);
        mainButtons.SetActive(false);
        controlsCanvas.SetActive(true);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void ReturnToButtons()
    {
        settingsCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        prologueCanvas.SetActive(false);
        mainButtons.SetActive(true);
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }
}
