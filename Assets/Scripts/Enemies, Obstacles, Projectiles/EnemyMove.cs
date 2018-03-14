using System;
using UnityEngine;

/// <summary>
/// Do not try to instantiate; it's the base class for EnemyMovePath and EnemyMoveTarget, for instance
/// </summary>
abstract public class EnemyMove : TriggeringObject
{
	protected Rigidbody2D rb; //rigidbody2d reference
	public float speed = 5f; //speed of enemy
	[SerializeField]
	protected BehaviorChangeConditions endCondition;
	[SerializeField]
	protected float value;

	protected Vector2 initialPos;
	protected bool hasNextMoveBehavior = false;
	protected Vector2 virtualPos;
	protected EnemyInfo info;
	float startTime;
	

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		info = GetComponent<EnemyInfo>();

		int enemyMoveScripts = 0;
		foreach (TriggerableObject obj in componentsToTrigger)
		{
			if (obj != null && obj.GetComponent<EnemyMove>() != null)
			{
				enemyMoveScripts++;
			}
			if (enemyMoveScripts > 0)
			{
				hasNextMoveBehavior = true;
				//if (enemyMoveScripts > 1)
				//	Debug.LogWarningFormat("<color=brown>Component " + this + " has " + enemyMoveScripts + " EnemyMove scripts in its list of components to trigger. (Should have only one.)</color>");//DEBUG
			}
		}
	}


	void Start()
	{
		doOnStart();
	}

	/// <summary>
	/// USE THAT METHOD INSTEAD OF Start() FOR CLASSES THAT WILL DERIVE FROM EnemyMove!
	/// </summary>
	virtual protected void doOnStart() { }


	void OnEnable()
	{
		initialPos = transform.position;
		startTime = Time.time;

		doOnEnable();
	}


	//USE THAT METHOD INSTEAD OF OnEnable() FOR CLASSES THAT WILL DERIVE FROM EnemyMove!
	virtual protected void doOnEnable() { }


	void FixedUpdate()
	{
		doOnUpdate();

		checkChangeBehaviorCondition();
	}


	private void checkChangeBehaviorCondition()
	{
		switch (endCondition)
		{
			case BehaviorChangeConditions.distance:
				checkDistanceCondition();
				break;
			case BehaviorChangeConditions.delay:
				checkDelayCondition();
				break;
		}
	}


	virtual protected void checkDistanceCondition()
	{
		Vector2 meToPc = transform.position - PCController.Me.transform.position;
		//if the PC is closer than the activation distance (using Pythagore theorem)
		//(Note: we compare squared distances so we avoid calculating a computing-heavy square root)
		if (meToPc.sqrMagnitude < value * value)
		{
			triggerOthersAndFinish();
		}
	}


	virtual protected void checkDelayCondition()
	{
		if (Time.time >= startTime + value)
		{
			triggerOthersAndFinish();
		}
	}


	/// <summary>
	/// USE THAT METHOD INSTEAD OF Update() FOR CLASSES THAT WILL DERIVE FROM EnemyMove!
	/// </summary>
	virtual protected void doOnUpdate() { }


	private void justDestroy()
	{
		Destroy(gameObject);
	}

}


public enum BehaviorChangeConditions
{
	NA,
	distance,
	delay
}