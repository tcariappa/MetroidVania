using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField]
	Slider healthSlider;


	void Start()
	{
		healthSlider.value = PCHealthManager.Me.hp / PCHealthManager.Me.MaxHitPoints;
	}


	private void OnEnable()
	{
		PCHealthManager.OnDamaged += handleOnPCDamaged;
	}


	private void OnDisable()
	{
		PCHealthManager.OnDamaged -= handleOnPCDamaged;
	}


	private void handleOnPCDamaged(float dmg, float hp)
	{
		healthSlider.value = hp / PCHealthManager.Me.MaxHitPoints;
	}
}
