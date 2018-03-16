using UnityEngine;
using System.Collections;


/// <summary>
/// Interprets collisions between PC and map
/// (and potentially handles collisions between PC and entities that can never be passed through; no example right now)
/// </summary>
public class PCCollSolidManager : MonoBehaviour
{
	public PCSensor sensorBottomLeft;
	public PCSensor sensorBottomRight;
	public PCSensor sensorLeftLow;
	public PCSensor sensorLeftHigh;
	public PCSensor sensorLeftLedge;
	public PCSensor sensorRightLow;
	public PCSensor sensorRightHigh;
	public PCSensor sensorRightLedge;
	public PCController pc;

	public bool isTouchingRightTile { get; private set; }
	public bool isTouchingLeftTile { get; private set; }
	public bool isForRightLedgeGrab { get; private set; }
	public bool isForLeftLedgeGrab { get; private set; }
	public GroundedStates groundedState;
	public bool isInAir { get; private set; }


	/// <summary>
	/// Called by enemy things when they hit the PC
	/// </summary>
	//public void onHitByEnemyObject(Enemies enemy, float dmg)
	//{
	//	if (OnHit != null)
	//		OnHit(dmg);
	//}


	void FixedUpdate()
	{
		//check for grabbing ledge
		if (pc.facingDir == Alias.RIGHT)
		{
			if (sensorRightHigh.isCollTile && !sensorRightLedge.isCollTile)
			{
				isForRightLedgeGrab = true;
			}
			else
			{
				isForRightLedgeGrab = false;
			}
			isForLeftLedgeGrab = false;
		}
		else
		{
			if (sensorLeftHigh.isCollTile && !sensorLeftLedge.isCollTile)
			{
				isForLeftLedgeGrab = true;
			}
			else
			{
				isForLeftLedgeGrab = false;
			}
			isForRightLedgeGrab = false;
		}

		//check if collide wall on the left
		if (sensorLeftLow.isCollTile && sensorLeftHigh.isCollTile)
		{
			isTouchingLeftTile = true;
		}
		else
		{
			isTouchingLeftTile = false;
		}

		//check if collide wall on the right
		if (sensorRightLow.isCollTile && sensorRightHigh.isCollTile)
		{
			isTouchingRightTile = true;
		}
		else
		{
			isTouchingRightTile = false;
		}

		//check if collide floor
		if (sensorBottomLeft.isCollTile && sensorBottomRight.isCollTile)
		{
			groundedState = GroundedStates.bothSides;
		}
		else if (sensorBottomLeft.isCollTile || sensorBottomRight.isCollTile)
		{
			groundedState = GroundedStates.oneSideOnly;
		}
		else
		{
			groundedState = GroundedStates.no;
		}

		//check if collide nothing
		if (groundedState == GroundedStates.no && !isTouchingLeftTile && !isTouchingRightTile && !isForLeftLedgeGrab && !isForRightLedgeGrab)
		{
			isInAir = true;
		}
		else
		{
			isInAir = false;
		}
	}


	//void computeSlope()
	//{
	//	//2 main cases:
	//	//Both bottom sensors touch a tile;
	//	//in that case we compute the slope of the line defined by both hit points above bottom sensors.
	//	//(This case is important when at the bottom of a slope going up; to move smoothly we get the averaged slope.)
	//	//Only one bottom sensor touches a tile;
	//	//in this case we get the slope from the normal of the single hit point above or under the bottom sensor that is in front of the PC.

	//	//Both bottom sensors collide with tilemap
	//	if (sensorBottomLeft.isCollTile && sensorBottomRight.isCollTile)
	//	{
	//		//We cast a line from above each bottom sensor, to the sensor, and we store hit point's position.
	//		Vector2 hitPtL = castLineFromAbove(sensorBottomLeft.transform.position);
	//		Vector2 hitPtR = castLineFromAbove(sensorBottomRight.transform.position);

	//		//compute the slope: a = (yB - yA) / (xB - xA)
	//		slope = (hitPtR.y - hitPtL.y) / (hitPtR.x - hitPtL.x);
	//		slopeVector = hitPtR - hitPtL;
	//	}

	//	//Only one bottom sensor collides with tilemap
	//	else
	//	{
	//		//we always check above and under the bottom sensor that is in front of the PC
	//		Vector2 startPt;
	//		startPt = pc.facingDir == Alias.RIGHT ? sensorBottomRight.transform.position : sensorBottomLeft.transform.position;
	//		startPt.y += 0.4f;
	//		//We cast a ray downward
	//		RaycastHit2D hit = Physics2D.Raycast(startPt, Vector2.down, 0.8f, Alias.LAYERMASK_TILEMAP);
	//		//if the ray hits a tile, we use the tile's normal to get the slope
	//		if (hit)
	//		{
	//			Vector2 tangent = new Vector2(hit.normal.y, -hit.normal.x);
	//			slope = tangent.y / tangent.x;
	//			slopeVector = tangent;
	//		}
	//		//if there is nothing under the sensor, it means the PC is facing a cliff. In that case it behaves like on flat ground
	//		else
	//		{
	//			slope = 0f;
	//			slopeVector = Vector2.right;
	//		}
	//	}
	//}


	//Vector2 castLineFromAbove(Vector2 endPt)
	//{
	//	Vector2 startPt = new Vector2(endPt.x, endPt.y + 0.4f);
	//	RaycastHit2D hit = Physics2D.Linecast(startPt, endPt, Alias.LAYERMASK_TILEMAP);
	//	if (!hit)
	//	{
	//		Debug.LogError("There should be a hit point with the tilemap between " + startPt + " and " + endPt);//DEBUG
	//		return Vector2.zero;
	//	}
	//	return hit.point;
	//}


	/// <summary>
	/// Called by PCController to get the exact position of the ledge when the PC is about to go grabbingLedge 
	/// </summary>
	public Vector2 findLedge(int pcDir, out Vector2 refPos)
	{
		//get the correct sensors
		Transform sensor1;
		Transform sensor2;

		if (pcDir == Alias.RIGHT)
		{
			sensor1 = sensorRightLedge.transform;
			sensor2 = sensorRightHigh.transform;
		}
		else
		{
			sensor1 = sensorLeftLedge.transform;
			sensor2 = sensorLeftHigh.transform;
		}

		refPos = sensor1.position;

		//cast a ray from sensor1 to sensor2 (i.e. downward) to find the floor (i.e. the ledge)
		RaycastHit2D hit = Physics2D.Linecast(sensor1.position, sensor2.position, Alias.LAYERMASK_TILEMAP | Alias.LAYERMASK_BREAKABLE_SURFACE);
		if (!hit)
			Debug.LogError("BUG: There should be a ledge under " + sensor1);//DEBUG

		return hit.point;
	}
}


public enum GroundedStates
{
	no = 0,
	bothSides = 1,
	oneSideOnly = 2,
}
