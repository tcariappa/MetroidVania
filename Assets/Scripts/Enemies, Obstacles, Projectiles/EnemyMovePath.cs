using UnityEngine;
using System.Collections;
using System;

public class EnemyMovePath : EnemyMove
{
	[NonSerialized]
	public Spawner mySpawner;
	[SerializeField]
	PatrolPath path;

	int currTgtWpIndex = 0;
	bool isPostPath = false;
	Vector2 moveDir;


	public void initFromSpawner(Spawner spawner)
	{
		endCondition = BehaviorChangeConditions.NA;
		mySpawner = spawner;
		currTgtWpIndex = 1;
		isPostPath = false;

		//Positionning enemy at the first waypoint's coordinates
		if (mySpawner != null)
		{
			path = mySpawner.path;
			rb.position = transform.position = path.wayPoints[0].position;
		}

		//Orienting enemy in the direction of the path, if there are more than one waypoints
		//if (path.Length > 1)
		//{
		//	transform.LookAt(path[currTargetIndex]);
		//	rb.rotation = transform.eulerAngles.z;
		//}
	}


	//protected override void doOnStart()
	//{
	//	isPostPath = false;
	//	currTgtWpIndex = 0;
	//}


	/// <summary>
	/// USE THAT METHOD INSTEAD OF Update() FOR CLASSES THAT DERIVE FROM EnemyMove!
	/// </summary>
	override protected void doOnUpdate()
	{
		if (!isPostPath)
		{
			if (path != null)
			{
				moveOnPath();
			}
			else
			{
				goPostPath();
			}
		}
		else
		{
			movePostPath();
		}
	}


	void moveOnPath()
	{
		float moveAmount = speed * Time.fixedDeltaTime;

		//HAVE WE REACHED THE WAYPOINT?
		//If there is still a waypoint as target, we check if we are close enough to it to consider we have reached it, and target the next index
		if (currTgtWpIndex < path.wayPoints.Length)
		{
			bool hasReachedTgt = false;
			Vector2 v = rb.position - (Vector2)path.wayPoints[currTgtWpIndex].position;

			if (info.isFlying)
			{
				if (v.sqrMagnitude < moveAmount * moveAmount)
					hasReachedTgt = true;
			}
			else
			{
				if (Mathf.Abs(v.x) < moveAmount)
					hasReachedTgt = true;
			}

			if (hasReachedTgt)
			{
				//if we were not at the last wp, we increment the target wp index
				if (currTgtWpIndex < path.wayPoints.Length - 1)
				{
					currTgtWpIndex++;
				}
				//otherwise, if the path loops to a wp, we aim at it
				else if (path.loopWpIndex > -1)
				{
					currTgtWpIndex = path.loopWpIndex;
				}
				//else, we exit the path behavior
				else
				{
					//If the path is finished we activate the next EnemyMove behavior
					goPostPath();
					return;
				}
			}
		}

		//ACTUALLY MOVING THE ENEMY
		Vector2 diff = (Vector2)path.wayPoints[currTgtWpIndex].position - rb.position;
		Vector2 vel;

		if (info.isFlying)
		{
			vel = diff.normalized * speed;
		}
		else
		{
			if (diff.x > 0)
				vel = new Vector2(speed, rb.velocity.y);
			else 
				vel = new Vector2(-speed, rb.velocity.y);
		}

		rb.velocity = vel;
	}


	void goPostPath()
	{
		isPostPath = true;

		//if no EnemyMove behavior has been specified in the trigger list, we keep the current script enabled and will move straight in the current direction and speed
		if (!hasNextMoveBehavior)
		{
			moveDir.Normalize(); //we normalize moveDir so that we can use it as a pure direction (vector length of 1) henceforth
		}

		//We still move the enemy during this frame
		movePostPath();

		//And we trigger the other objects
		triggerOthersAndFinish();
	}


	void movePostPath()
	{
		Vector2 move = moveDir * (speed * Time.deltaTime);
		transform.position = rb.position = (Vector2)transform.position + move;
	}


	void OnDrawGizmos()
	{
		if (path != null)
		{
			Gizmos.color = Color.grey;
			Gizmos.DrawLine(transform.position, path.transform.position);
		}
	}

}
