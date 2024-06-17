using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;

    //Buttons
    [SerializeField] Button button;


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
        }
            
        if(!GameManager.gameOnPause)
        {
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
        }
            
    }

    public void TestTrigger(BaseEventData eventData)
    {
        button.transform.localScale *= 2;
    }
    public void TestTrigger2(BaseEventData eventData)
    {
        button.transform.localScale /=  2;
    }


}
