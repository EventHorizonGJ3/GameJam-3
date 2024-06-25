using System;
using TMPro;
using UnityEngine;

public class EndStats : Score
{
	public static Action EnemyDeath;

	[SerializeField] TextMeshProUGUI ScoreTxT;
	[SerializeField] TextMeshProUGUI killsTxT;

	int killedEnemies;
	private void OnEnable()
	{
		killedEnemies = 0;
		GameManager.OnLose += UpdateText;
		GameManager.OnWin += UpdateText;
		EnemyDeath += OnEnemyDeath;
	}
	private void OnDisable()
	{
		EnemyDeath -= OnEnemyDeath;
		GameManager.OnLose -= UpdateText;
		GameManager.OnWin -= UpdateText;
	}

	private void UpdateText()
	{
		ScoreTxT.text = $"Score: {base.score}";
		killsTxT.text = $"Kills: {killedEnemies}";
	}

	private void OnEnemyDeath()
	{
		killedEnemies++;
	}
}
