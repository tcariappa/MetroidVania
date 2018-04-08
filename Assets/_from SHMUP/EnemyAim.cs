using UnityEngine;
using System.Collections;

public class EnemyAim : TriggerableObject
{
	[SerializeField]
	EnemyShoot shootManager; //to update shootManager.shootingAngle
	[SerializeField]
	float maxDelta = 60f; //max target angle from the initialAngle
	[SerializeField] Transform target;

	float initialAngle;
	float ccwAngleMax, cwAngleMax;


	void Awake()
	{
		if (target == null)
		{
			target = PCController.Me.transform;
		}

		//define min & max angles
		initialAngle = shootManager.shootingAngle;

		ccwAngleMax = initialAngle + maxDelta;
		ccwAngleMax %= 360f;
		if (ccwAngleMax < 0)
			ccwAngleMax += 360f;
		cwAngleMax = initialAngle - maxDelta;
		cwAngleMax %= 360f;
		if (cwAngleMax < 0)
			cwAngleMax += 360f;
	}


	void Update()
	{
		if (target != null)
		{
			//if the aim if for the shoot angle
			if (shootManager != null)
			{
				Vector2 v = target.position - transform.position;
				float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
				shootManager.shootingAngle = clampAngle(angle);
			}
		}
	}


	/// <summary>
	/// clamp angle to remain within authorized values; the angle must always be smaller than ccwAngleMax and larger than cwAngleMax
	/// </summary>
	float clampAngle(float angle)
	{
		//Make sure angle is between 0 and 360 degrees
		angle %= 360f;
		if (angle < 0)
			angle += 360f;

		//clamp angle if needed
		if (ccwAngleMax > cwAngleMax)
		{
			if (angle > ccwAngleMax)
				angle = ccwAngleMax;
			else if (angle < cwAngleMax)
				angle = cwAngleMax;
		}
		else
		{
			if (angle > ccwAngleMax && angle < cwAngleMax)
			{
				if (Mathf.DeltaAngle(angle, ccwAngleMax) < Mathf.DeltaAngle(angle, cwAngleMax))
					angle = ccwAngleMax;
				else
					angle = cwAngleMax;
			}
		}

		return angle;
	}
}
