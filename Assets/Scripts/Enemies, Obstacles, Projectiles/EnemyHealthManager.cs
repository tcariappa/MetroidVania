using UnityEngine;
using System.Collections;
using System;

public class EnemyHealthManager : MonoBehaviour
{
	[SerializeField]
	EnemyInfo info;
	[SerializeField]
	private Transform explosionVfxRef;
	[SerializeField]  //the following variable WILL be displayed in Unity inspector, EVEN IF IT IS PRIVATE
	private float hitPoints = 20f;
	[SerializeField]
	private float dmgTakenOnPCTouch = 0f;
	[SerializeField]
	private float dmgTakenOnMapHit = 2f;
	[SerializeField]
	Timer doomTimer;

	static public event System.Action<Enemies> OnKilledStatic;
	public event System.Action OnKilledInstance;

	Spawner mySpawner;
	float initialHp;


	void Awake()
	{
		initialHp = hitPoints;
	}


	void OnEnable()
	{
		if (doomTimer != null)
			doomTimer.OnTimesUp += handleOnTimesUp;
	}


	void OnDisable()
	{
		if (doomTimer != null)
			doomTimer.OnTimesUp -= handleOnTimesUp;
	}


	private void handleOnTimesUp()
	{
		//unregister from event
		doomTimer.OnTimesUp -= handleOnTimesUp;

		takeDamage(Mathf.Infinity, false);
	}


	public void register(Spawner spawner)
	{
		mySpawner = spawner;
	}


	public float getHps()
	{
		return hitPoints;
	}


	/// <summary>
	/// Called by EnemyCollider when a PC's attack hits that enemy
	/// </summary>
	public void gotHitByPCAttack(float dmg)
	{
		takeDamage(dmg);
	}


	/// <summary>
	/// Called by EnemyCollider when the PC's body collides with that enemy
	/// </summary>
	public void touchPC()
	{
		if (dmgTakenOnPCTouch > 0)
			takeDamage(dmgTakenOnPCTouch);
	}


	/// <summary>
	/// Called by EnemyCollider when that enemy collides with the level
	/// </summary>
	public void gotMapHit()
	{
		if (dmgTakenOnMapHit > 0)
			takeDamage(dmgTakenOnMapHit * Time.fixedDeltaTime, false);
	}


	void takeDamage(float dmg, bool mayBeAKill = true)
	{
		hitPoints -= dmg;

		if (hitPoints <= 0)
		{
			farewell();
			if (mayBeAKill)
			{
				if (OnKilledStatic != null)
					OnKilledStatic(info.type);

				if (OnKilledInstance != null)
					OnKilledInstance();
			}

			if (mySpawner != null)
				//Whether the enemy is killed by the player or by the environment, it's killed and we inform mySpawner accordingly
				mySpawner.doOnEnemyDestroyed(true, transform.position);
		}
	}


	void farewell()
	{
		//instantiate an explosion object
		if (explosionVfxRef != null)
			Instantiate(explosionVfxRef, transform.position, Quaternion.identity);

		//destroy this whole gameobject (including the particle system component)
		Destroy(gameObject);
	}
}
