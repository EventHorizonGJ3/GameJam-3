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

	private void Heal(float _heal, float _dur)
	{
		if (HP + _heal > maxHP)
		{//   5   -= 6  +   5   -  10   --> 5 -= 11-10 --> 5-= 1 --> 4 so now HP+_Heal = maxHP;
			_heal -= HP + _heal - maxHP;
		}
		StartCoroutine(HealInTime(_heal, _dur));
	}

	private IEnumerator HealInTime(float _heal, float _dur)
	{

		float _healAmount = 0;
		var _healTick = _heal / _dur;
		while (_healAmount < _heal)
		{
			HP += _healTick * Time.deltaTime;
			_healAmount += _healTick * Time.deltaTime;
			Debug.Log("healAmount = " + _healAmount);
			UpdateHpBar();
			yield return null;
		}
	}
	void UpdateHpBar()
	{
		hpBar.fillAmount = HP / maxHP;
	}
	public void Knockback(float _Power)
	{ }

	public void NoHP()
	{
		GameManager.OnLose?.Invoke();
		Debug.LogError("OH I'M DIE THANK YOU FOREVER");
	}

	public void TakeDamage(float damage)
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
