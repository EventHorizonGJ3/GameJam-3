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
            boss.bossSO.OnSpawn += boss.Spawn;
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
            boss.bossSO.OnSpawn -= boss.Spawn;
            boss.bossSO.OnHit -= GetHp;
        }
    }

    private void GetMaxHP(float _Hp)
    {
        maxHp = _Hp;
    }

    private void GetHp(float _Hp)
    {
        bosses[currentBoss].hpBarFill.fillAmount = _Hp / maxHp;
        if (_Hp <= 0)
        {
            bosses[currentBoss].hpBarFill.fillAmount = 0;
            bosses[currentBoss].Death();
            OnBossDead?.Invoke();
            currentBoss++;

            if (currentBoss >= bosses.Length)
            {
                GameManager.OnWin?.Invoke();
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


    public void Death()
    {
        UpdateBars(false);
    }
    
    public void Spawn(float _)
    {
        UpdateBars(true);
    }

    void UpdateBars(bool _X)
    {
        hpBarFill.enabled = _X;
        hpBarBackground.enabled = _X;
    }
}

