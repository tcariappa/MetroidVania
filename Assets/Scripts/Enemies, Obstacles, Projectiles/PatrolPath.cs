using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
	public Transform[] wayPoints;
	[SerializeField]
	private bool isClosed = true;
	[SerializeField]
	Color color = new Color(1f, 0.6f, 0f);
	[NonSerialized]
	public int loopWpIndex;


	void Awake()
	{
		//We define the wp which the path will loop to.
		//By default we assume the path doesn't loop, therefore loopWpIndex has an 'invalid' value (-1)
		loopWpIndex = -1;
		//If the path is marked as closed, it will loop to the first wp (index 0)
		if (isClosed)
		{
			loopWpIndex = 0;
		}
		//Otherwise we check if the last wp is one that has already been referenced in the path...
		else
		{
			int lastIndex = wayPoints.Length - 1;
			for (int i = 0; i < lastIndex; i++)
			{
				//...If it's the case, the path will loop to its index
				if (wayPoints[lastIndex] == wayPoints[i])
				{
					loopWpIndex = i;
					break;
				}
			}
		}
	}


	/// <summary>
	/// Draw the vector lines and gizmos to visualize the path in the Editor
	/// </summary>
	void OnDrawGizmos()
	{
		if (wayPoints != null)
		{
			//line from root object to first wp
			Gizmos.color = Color.grey;
			Gizmos.DrawLine(transform.position, wayPoints[0].position);

			//visual on first waypoint
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(wayPoints[0].position, 0.35f);

			//drawing the lines between waypoints
			for (int i = 1; i < wayPoints.Length; i++)
			{
				Gizmos.color = color;
				Gizmos.DrawLine(wayPoints[i - 1].position, wayPoints[i].position);
				Gizmos.color = Color.grey;
				Gizmos.DrawWireSphere(wayPoints[i].position, 0.35f);
			}

			//drawing one additional line btw last and first if closed path
			if (isClosed)
			{
				Gizmos.color = color;
				Gizmos.DrawLine(wayPoints[wayPoints.Length - 1].position, wayPoints[0].position);
			}

		}
	}

}
