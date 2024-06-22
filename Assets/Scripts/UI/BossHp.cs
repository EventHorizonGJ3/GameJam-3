using System;
using UnityEngine;
using UnityEngine.UI;

public class BossHp : MonoBehaviour
{
	[SerializeField] BossesSo[] bosses;
	[SerializeField] Image hpBar;
	int currentBoss;
	float maxHp;

	private void OnEnable()
	{
		hpBar.enabled = false;
		foreach (var boss in bosses)
		{
			boss.OnSpawn += GetMaxHP;
			boss.OnHit += GetHp;
		}
	}

	private void OnDisable()
	{
		foreach (var boss in bosses)
		{
			boss.OnSpawn -= GetMaxHP;
			boss.OnHit -= GetHp;
		}
	}

	private void GetMaxHP(float _Hp)
	{
		maxHp = _Hp;
		UpdateBarSettings();
	}

	private void UpdateBarSettings()
	{
		hpBar.rectTransform.localScale = new Vector3(bosses[currentBoss].hpBarWidth, bosses[currentBoss].hpBarHight);
		hpBar.enabled = true;
	}

	private void GetHp(float _Hp)
	{
		if (_Hp <= 0)
		{
			currentBoss++;
			if (currentBoss >= bosses.Length)
			{
				hpBar.fillAmount = 0;
				hpBar.enabled = false;
				gameObject.SetActive(false);
			}
		}
		hpBar.fillAmount = _Hp / maxHp;
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if (bosses.Length <= 0)
			return;

		float pos = 0;
		for (int i = 0; i < bosses.Length; i++)
		{
			BossesSo boss = bosses[i];
			Gizmos.color = Color.white;
			Gizmos.DrawRay(hpBar.rectTransform.position + Vector3.up * pos, Vector3.up * boss.hpBarHight / 2);
			Gizmos.DrawRay(hpBar.rectTransform.position + Vector3.up * pos, Vector3.down * boss.hpBarHight / 2);

			Gizmos.color = Color.magenta;
			Gizmos.DrawRay(hpBar.rectTransform.position + Vector3.up * pos, Vector3.right * boss.hpBarWidth / 2);
			Gizmos.DrawRay(hpBar.rectTransform.position + Vector3.up * pos, Vector3.left * boss.hpBarWidth / 2);

			if (i + 1 >= bosses.Length)
				break;
			pos += (boss.hpBarHight / 2) + (bosses[i + 1].hpBarHight / 2) + 1;
		}
	}
#endif
}

