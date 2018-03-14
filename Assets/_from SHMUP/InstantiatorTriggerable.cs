using UnityEngine;
using System.Collections;

public class InstantiatorTriggerable : TriggerableObject
{
	[SerializeField]
	GameObject objToSpawn;

	void OnEnable()
	{
		Instantiate(objToSpawn, transform.position, transform.rotation);

		enabled = false;
	}
}
