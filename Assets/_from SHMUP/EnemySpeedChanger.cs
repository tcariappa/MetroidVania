using UnityEngine;
using System.Collections;

public class EnemySpeedChanger : TriggerableObject
{
	[SerializeField]
	EnemyMovePath moveManager;
	[SerializeField]
	float newSpeed;


	void OnEnable()
	{
		moveManager.speed = newSpeed;

		//disable this script right after having done its duty
		enabled = false;
	}
}
