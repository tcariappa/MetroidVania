using UnityEngine;
using System.Collections;

public class InstantiatorSimple : MonoBehaviour
{
	[SerializeField]
	GameObject objToSpawn;

	void OnEnable()
	{
		Instantiate(objToSpawn, transform.position, transform.rotation);
	}
}
