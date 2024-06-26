using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	//events
	public static Action<bool> OnHint;

	[SerializeField] GameObject pauseMenu;
	[SerializeField] GameObject settingsMenu;
	[SerializeField] RectTransform menuHolder;
	[SerializeField] GameObject controlsMenu;
	Vector3 menuHolderOriginPos;

	//Refs
	[SerializeField] List<GameObject> allMainButtons = new List<GameObject>();
	[SerializeField] GameObject preselectedOnSetting;
	[SerializeField] GameObject preselectedOnWin;
	[SerializeField] GameObject preselectedOnLose;
	[SerializeField] GameObject finalCanvas;
	[SerializeField] GameObject winScreen;
	[SerializeField] GameObject loseScreen;
	[SerializeField] GameObject statsImage;
	[SerializeField] GameObject hintText;
	[Header("Stats")]
	//stats
	[SerializeField] TMP_Text enemiesKilled;
	[SerializeField] TMP_Text totalScore;
	[SerializeField] TMP_Text gameTime;

	float score;


	private void Awake()
	{
		menuHolderOriginPos = menuHolder.anchoredPosition;
	}


	private void OnEnable()
	{
		GameManager.OnPause += PauseFunction;
		GameManager.OnLose += LoseScreen;
		GameManager.OnWin += WinScreen;
		Score.OnScoreChanged += GetScore;
		OnHint += HintFunction;

	}
	private void OnDisable()
	{
		GameManager.OnPause -= PauseFunction;
		GameManager.OnLose -= LoseScreen;
		GameManager.OnWin -= WinScreen;
		Score.OnScoreChanged -= GetScore;
        OnHint -= HintFunction;
    }



	private void GetScore(float obj)
	{
		score = obj;
	}

	private void Start()
	{
		pauseMenu.SetActive(false);
		settingsMenu.SetActive(false);
		finalCanvas.SetActive(false);
		hintText.SetActive(false);
	}

	void PauseFunction()
	{
		if (GameManager.gameOnPause)
		{
			pauseMenu.SetActive(true);
			MainButtonsSetActive(true);
			StartCoroutine(MenuLerpIn());
			if (GameManager.usingGamePad) EventSystem.current.SetSelectedGameObject(allMainButtons[0]);
			else EventSystem.current.SetSelectedGameObject(null);
			AudioManager.instance.PlaySFX(AudioManager.instance.AudioData.SFX_OpenPauseMenu);
		}

		if (!GameManager.gameOnPause)
		{
			StartCoroutine(MenuLerpOut());
			settingsMenu.SetActive(false);
			EventSystem.current.SetSelectedGameObject(null);
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

	void HintFunction(bool value)
	{
		hintText.SetActive(value);
	}



	public void ActivateSettingsMenu()
	{
		MainButtonsSetActive(false);
		settingsMenu.SetActive(true);
		if (GameManager.usingGamePad)
			EventSystem.current.SetSelectedGameObject(preselectedOnSetting);
		AudioManager.instance.PlaySFX(AudioManager.instance.AudioData.SFX_UI_Select);
	}

	public void BackToPause()
	{
		MainButtonsSetActive(true);
		settingsMenu.SetActive(false);
		EventSystem.current.SetSelectedGameObject(allMainButtons[0]);
		AudioManager.instance.PlaySFX(AudioManager.instance.AudioData.SFX_UI_Select);
	}

	public void ResumeGame()
	{
		GameManager.OnResume?.Invoke();
		AudioManager.instance.PlaySFX(AudioManager.instance.AudioData.SFX_UI_Select);
	}

	public void GoToMainMenu()
	{
		SceneManager.LoadScene(0);
		AudioManager.instance.PlaySFX(AudioManager.instance.AudioData.SFX_UI_Select);
	}

	public void Retry()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		AudioManager.instance.PlaySFX(AudioManager.instance.AudioData.SFX_UI_Select);
	}

	void LoseScreen()
	{
		finalCanvas.SetActive(true);
		winScreen.SetActive(false);
		loseScreen.SetActive(true);
		if (GameManager.usingGamePad) EventSystem.current.SetSelectedGameObject(preselectedOnLose);
		ShowStats();
	}
	void WinScreen()
	{
		finalCanvas.SetActive(true);
		winScreen.SetActive(true);
		loseScreen.SetActive(false);
		if (GameManager.usingGamePad) EventSystem.current.SetSelectedGameObject(preselectedOnWin);
		ShowStats();
	}

	void ShowStats()
	{
		statsImage.SetActive(true);
		enemiesKilled.text = "Enemy Killed: " + GameManager.enemyKilled.ToString();
		gameTime.text = "Your Time: " + Time.time.ToString();
		totalScore.text = "Score: " + score; // aggiungere variabile dello score
	}


}
