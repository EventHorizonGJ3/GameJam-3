using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	[Tooltip("how long it takes for the combo score to drop")]
	[SerializeField] float comboDur;
	[Tooltip("how long it takes for the Gameobject canvas to dissapear")]
	[SerializeField] float dissapearAfterTime;
	[SerializeField] GameObject canvas;
	float score = 0;
	float totDmg;
	float numberOfHits;
	[SerializeField] TextMeshProUGUI scoreTxt, totDmgTxt, numberOfHitsTxt;
	public bool UseScoreBar;
	[SerializeField] Image bar;
	Coroutine coroutine;

	public static Action<int> OnDmg;
	private void OnEnable()
	{
		OnDmg += GetScore;
		if (UseScoreBar == false)
			bar.enabled = false;
	}
	private void OnDisable()
	{
		OnDmg -= GetScore;
	}

	void GetScore(int _Dmg)
	{
		canvas.SetActive(true);
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
			coroutine = null;
		}

		totDmg += _Dmg;
		numberOfHits++;
		UpdateNumbers();
		coroutine = StartCoroutine(Timer());
	}
	void UpdateNumbers()
	{
		totDmgTxt.text = totDmg.ToString();
		numberOfHitsTxt.text = numberOfHits.ToString();
	}
	IEnumerator Timer()
	{
		float time = 0;
		while (time < comboDur)
		{
			time += Time.deltaTime;
			if (UseScoreBar)
				bar.fillAmount = Mathf.Lerp(1, 0, time / comboDur);
			yield return null;
		}

		score += totDmg * numberOfHits;
		scoreTxt.text = $"{score}";
		totDmg = 0;
		numberOfHits = 0;
		UpdateNumbers();
		time = 0;
		while (time < dissapearAfterTime)
		{
			time += Time.deltaTime;
			yield return null;
		}
		canvas.SetActive(false);
	}
}