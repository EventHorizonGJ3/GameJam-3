using System;
using TMPro;
using UnityEngine;

public class EndStats : MonoBehaviour
{
	public static Action EnemyDeath;
	float score;

	[SerializeField] TextMeshProUGUI ScoreTxT;
	[SerializeField] TextMeshProUGUI killsTxT;

	int killedEnemies;
	private void OnEnable()
	{
		Score.OnScoreChanged += UpdateScore;
		killedEnemies = 0;
		GameManager.OnLose += UpdateText;
		GameManager.OnWin += UpdateText;
		EnemyDeath += OnEnemyDeath;
	}
	private void OnDisable()
	{
		Score.OnScoreChanged -= UpdateScore;
		EnemyDeath -= OnEnemyDeath;
		GameManager.OnLose -= UpdateText;
		GameManager.OnWin -= UpdateText;
	}

	private void UpdateScore(float _Score)
	{
		score = _Score;
	}

	private void UpdateText()
	{
		string scoreString = score.ToString();
		string killsString = killedEnemies.ToString();

		killsTxT.text = "Kills: " + killsString;
		ScoreTxT.text = "Score: " + scoreString;
	}

	private void OnEnemyDeath()
	{
		killedEnemies++;
	}
}
