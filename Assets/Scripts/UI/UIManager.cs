using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;

    //Refs
    [SerializeField] GameObject resumeButton;

    private void OnEnable()
    {
        GameManager.OnPause += PauseFunction;
    }
    private void OnDisable()
    {
        GameManager.OnPause -= PauseFunction;
    }
    private void Start()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    void PauseFunction()
    {
        if (GameManager.gameOnPause)
        {
            pauseMenu.SetActive(true);
            if(GameManager.usingGamePad) EventSystem.current.SetSelectedGameObject(resumeButton);
            else EventSystem.current.SetSelectedGameObject(null);
        }
            
        if(!GameManager.gameOnPause)
        {
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
        }
            
    }



}
