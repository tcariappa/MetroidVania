using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZoneActivator : MonoBehaviour
{
	[SerializeField]
	TriggerableObject[] objectsToActivate;


	void OnTriggerEnter2D(Collider2D otherColl)
	{
		if (otherColl.gameObject.layer == Alias.LAYER_PC_SOLID)
		{
			foreach (TriggerableObject obj in objectsToActivate)
			{
				obj.triggerMe();
			}

			//After triggering, we deactivate the gameobject
			gameObject.SetActive(false);
		}
	}


	void OnDrawGizmos()
	{
		Gizmos.color = new Color(0.4f, 0.6f, 0.5f);
		for (int i = 0; i < objectsToActivate.Length; i++)
		{
			if (objectsToActivate[i] != null)
				Gizmos.DrawLine(transform.position, objectsToActivate[i].transform.position);
		}
	}
}
