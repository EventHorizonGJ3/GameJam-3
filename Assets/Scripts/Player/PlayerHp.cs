using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour, IDamageable
{
	[field: SerializeField] public float HP { get; set; }
	float maxHP;
	public Transform colliderTransform { get; set; }

	[SerializeField] Image hpBar;

    private void OnEnable()
    {
		maxHP = HP;
		RageBar.OnBerserkHeal += Heal;
    }
    private void OnDisable()
    {
		RageBar.OnBerserkHeal -= Heal;
	}

    private void Heal(float _heal,float _dur)
    {
        if(HP+_heal > maxHP)
        {//   5   -= 6  +    5  -  10 --> 5 -= 11-10 --> 5-= 1 --> 4 so now HP+_Heal = maxHP;
			_heal -= HP + _heal - maxHP;
        }
		StartCoroutine(HealInTime(_heal, _dur));
    }

    private IEnumerator HealInTime(float _heal, float _dur)
    {
		var _healTick = (_heal - HP) / _dur;
		float _target = HP + _heal;
        while (HP < _target)
        {
			HP += _healTick;
			UpdateHpBar();
			yield return null;
        }
		HP = _target;
    }
	void UpdateHpBar()
    {
		hpBar.fillAmount = HP / maxHP;
	}
    public void Knockback(float _Power)
	{}

	public void NoHP()
	{
		//TODO: GameManager.EndGame?.invoke();
		Debug.LogError("OH I'M DIE THANK YOU FOREVER");
	}

	public void TakeDamage(int damage)
	{
		HP -= damage;
		UpdateHpBar();
		//Todo: 
		if (HP <= 0)
		{
			NoHP();
		}
	}
}