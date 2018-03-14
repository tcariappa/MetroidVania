using UnityEngine;
using System.Collections;

public class EnemyMiscPointsHolder : MonoBehaviour
{
	public Transform[] points;

	public void setPointsList(Transform[] list)
	{
		points = list;
	}
}
