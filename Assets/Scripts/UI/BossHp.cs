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
            boss.UpdateBars(false);

            boss.bossSO.OnSpawn += boss.Spawn;
            boss.bossSO.OnHit += boss.OnHit;
        }
    }

    private void OnDisable()
    {
        foreach (var boss in bosses)
        {
            boss.UpdateBars(false);

            boss.bossSO.OnSpawn -= boss.Spawn;
            boss.bossSO.OnHit -= boss.OnHit;
        }
    }


}

[System.Serializable]
public class Bosses
{
    public BossesSo bossSO;
    public Image hpBarFill;
    public Image hpBarBackground;
    float maxHp;


    public void Death()
    {
        BossHp.OnBossDead?.Invoke();
        UpdateBars(false);
    }

    public void OnHit(float _HP)
    {
        hpBarFill.fillAmount = _HP / maxHp;
        if (_HP <= 0)
        {
            hpBarFill.fillAmount = 0;
            Death();
        }
    }

    public void Spawn(float _HP)
    {
        maxHp = _HP;
        UpdateBars(true);
    }

    public void UpdateBars(bool _X)
    {
        hpBarFill.enabled = _X;
        hpBarBackground.enabled = _X;
    }
}

