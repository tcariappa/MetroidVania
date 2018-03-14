//The only thing that this script does, is destroying its GameObject after a time (duration is 'lifespan'). Lightweight version of Countdowner
using UnityEngine;
using System.Collections;

public class SelfDestroyer : MonoBehaviour
{
	[SerializeField]
	float lifespan = 1.5f;

	float startTime;

	// Use this for initialization
	void Start()
	{
		startTime = Time.time;
	}


	// Update is called once per frame
	void Update()
	{
		if (Time.time > startTime + lifespan)
		{
			Destroy(gameObject);
		}
	}
}
