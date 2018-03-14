using UnityEngine;
using System.Collections;

public class EnemyMoveTarget : EnemyMove
{
	[SerializeField]
	protected Transform target;


	override protected void doOnStart()
	{
		base.doOnStart();
		//if no target specified, it is the player's ship
		if (target == null)
		{
			target = PCController.Me.transform;
		}
	}


	//USE THAT METHOD INSTEAD OF Update() FOR CLASSES THAT DERIVE FROM EnemyMove!
	override protected void doOnUpdate()
	{
		moveToTarget();
	}


	protected override void checkDistanceCondition()
	{
		float d = Vector2.SqrMagnitude(transform.position - target.position);
		if (d < value * value)
		{
			triggerOthersAndFinish();
		}
	}


	void moveToTarget()
	{
		Vector2 diff = (Vector2)target.position - rb.position;
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

}
