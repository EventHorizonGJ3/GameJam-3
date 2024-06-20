using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] RectTransform menuHolder;
    Vector3 menuHolderOriginPos;

    //Refs
    [SerializeField] GameObject resumeButton;
    [SerializeField] RectTransform pauseRect;

    private void Awake()
    {
        menuHolderOriginPos = menuHolder.anchoredPosition;
    }

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
            StartCoroutine(MenuLerpIn());
            if(GameManager.usingGamePad) EventSystem.current.SetSelectedGameObject(resumeButton);
            else EventSystem.current.SetSelectedGameObject(null);
        }
            
        if(!GameManager.gameOnPause)
        {
            StartCoroutine(MenuLerpOut());
        }
            
    }

    IEnumerator MenuLerpIn()
    {
        float time = 0.2f;
        float elapsedTime = 0;
        Vector2 targetPos = Vector3.zero;
        while (elapsedTime < time)
        {
            menuHolder.anchoredPosition = Vector2.Lerp(menuHolderOriginPos, targetPos, elapsedTime/time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        menuHolder.anchoredPosition = targetPos;
    }

    IEnumerator MenuLerpOut()
    {
        float time = 0.2f;
        float elapsedTime = 0;
        Vector2 targetPos = menuHolderOriginPos;
        while (elapsedTime < time)
        {
            menuHolder.anchoredPosition = Vector2.Lerp(menuHolder.anchoredPosition, targetPos, elapsedTime/time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        menuHolder.anchoredPosition = targetPos;
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }
    
    
    
}
