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
	[SerializeField, Range(0.1f, 0.9f)] float multPerHits; //ByEma minRaange 0.1
	float totMult;
	[SerializeField] GameObject canvas;
	float score = 0;
	float totDmg;
	float numberOfHits;
	[SerializeField] TextMeshProUGUI scoreTxt, totDmgTxt, numberOfHitsTxt;
	public bool UseScoreBar;
	[SerializeField] Image bar;
	Coroutine coroutine;
	public static Action<float> OnScoreChanged;
	public static Action<float> OnDmg;
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

	void GetScore(float _Dmg)
	{
		canvas.SetActive(true);
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
			coroutine = null;
		}

		totDmg += _Dmg;
		numberOfHits++;
		totMult = 1 + numberOfHits * multPerHits;
		UpdateNumbers();
		coroutine = StartCoroutine(Timer());
	}
	void UpdateNumbers()
	{

		totDmgTxt.text = string.Format("{0:0.0}", totDmg); //Byema remove second digit
		numberOfHitsTxt.text = string.Format("{0:0.0}", totMult); //Byema remove second digit
		//numberOfHitsTxt.text = (int)totMult % totMult == 0 ? totMult.ToString() : totMult.ToString("0.00");
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

		score += totDmg * totMult;
		OnScoreChanged?.Invoke(score);

		scoreTxt.text = string.Format("{0:0.00}", score);
		totDmg = 0;
		numberOfHits = 0;
		totMult = 0;
		time = 0;
		UpdateNumbers();
		while (time < dissapearAfterTime)
		{
			time += Time.deltaTime;
			yield return null;
		}
		canvas.SetActive(false);
	}
}
