using System;
using UnityEngine;
using UnityEngine.UI;

public class BossHp : MonoBehaviour
{
    public static Action OnBossDead;

    [SerializeField] Bosses[] bosses;
    int currentBoss;
    float maxHp;

    private void OnEnable()
    {

        foreach (var boss in bosses)
        {
            boss.hpBarFill.enabled = false;
            boss.hpBarBackground.enabled = false;
            boss.bossSO.OnSpawn += GetMaxHP;
            boss.bossSO.OnHit += GetHp;
        }
    }

    private void OnDisable()
    {
        foreach (var boss in bosses)
        {
            boss.hpBarFill.enabled = false;
            boss.hpBarBackground.enabled = false;
            boss.bossSO.OnSpawn -= GetMaxHP;
            boss.bossSO.OnHit -= GetHp;
        }
    }

    private void GetMaxHP(float _Hp)
    {
        maxHp = _Hp;
        UpdateBarSettings(true);
    }

    private void UpdateBarSettings(bool _x)
    {
        bosses[currentBoss].hpBarFill.enabled = _x;
        bosses[currentBoss].hpBarBackground.enabled = _x;
    }

    private void GetHp(float _Hp)
    {
        bosses[currentBoss].hpBarFill.fillAmount = _Hp / maxHp;
        if (_Hp <= 0)
        {
            OnBossDead?.Invoke();
            bosses[currentBoss].hpBarFill.fillAmount = 0;
            UpdateBarSettings(false);
            currentBoss++;

            if (currentBoss >= bosses.Length)
            {
                bosses[currentBoss].hpBarFill.fillAmount = 0;

                UpdateBarSettings(false);

                GameManager.OnWin();
                gameObject.SetActive(false);
            }
        }
    }
}

[System.Serializable]
public class Bosses
{
    public BossesSo bossSO;
    public Image hpBarFill;
    public Image hpBarBackground;
}

