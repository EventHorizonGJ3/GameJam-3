using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] RectTransform menuHolder;
    Vector3 menuHolderOriginPos;

    //Refs
    [SerializeField] List<GameObject> allMainButtons = new List<GameObject>();
    [SerializeField] GameObject preselectedOnSetting;




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
            if (GameManager.usingGamePad) EventSystem.current.SetSelectedGameObject(allMainButtons[0]);
            else EventSystem.current.SetSelectedGameObject(null);
        }

        if (!GameManager.gameOnPause)
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
            menuHolder.anchoredPosition = Vector2.Lerp(menuHolderOriginPos, targetPos, elapsedTime / time);
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
            menuHolder.anchoredPosition = Vector2.Lerp(menuHolder.anchoredPosition, targetPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        menuHolder.anchoredPosition = targetPos;
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    void MainButtonsSetActive(bool value)
    {
        foreach (GameObject button in allMainButtons)
            button.SetActive(value);
    }





    public void ActivateSettingsMenu()
    {
        MainButtonsSetActive(false);
        settingsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(preselectedOnSetting);
    }

    public void BackToPause()
    {
        MainButtonsSetActive(true);
        settingsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(allMainButtons[0]);
    }

    public void ResumeGame()
    {
        GameManager.OnResume?.Invoke();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
