using UnityEngine;
using System.Collections;

public class EnemyVictoryTrigger : MonoBehaviour
{
	[SerializeField]
	EnemyHealthManager healthManager;


	void Awake()
	{
		healthManager.OnKilledInstance += handleOnKilledInstance;
	}


	void handleOnKilledInstance()
	{
		//GameManager.GoVictory();
		healthManager.OnKilledInstance -= handleOnKilledInstance;
	}


	void OnDestroy()
	{
		healthManager.OnKilledInstance -= handleOnKilledInstance;
	}
}
