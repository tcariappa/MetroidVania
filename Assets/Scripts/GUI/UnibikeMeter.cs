using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnibikeMeter : MonoBehaviour
{
    [SerializeField]
    Slider UnibikeMeterGUI;


	void Start()
	{
		UnibikeMeterGUI.value = PCHealthManager.Me.hp / PCHealthManager.Me.MaxHitPoints;
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
		UnibikeMeterGUI.value = hp / PCHealthManager.Me.MaxHitPoints;
	}
}
