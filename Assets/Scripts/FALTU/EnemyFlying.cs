using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : MonoBehaviour 
{
	//public Rigidbody2D rb;
	public SpriteRenderer spriteRenderer;
	public float speed; 
	public Transform[] waypoints;


	private Transform currentTarget;
	private int currentWpIndex;

	// Use this for initialization
	void Start () 
	{
		currentWpIndex = 0;
		//target = FindObjectOfType<PCController>().transform;
		currentTarget = waypoints[0];

	}
	
	// Update is called once per frame
	void Update () 
	{
		// STEP 1
		// Move the character towards target at certain speed.

		Vector2 targetPosition = currentTarget.position;
		targetPosition.y += 1f;
		Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

		transform.position = newPosition;

		// STEP 2
		//Changing the direction according to the target.
		if (targetPosition.x > newPosition.x) 
		{
			spriteRenderer.flipX = false ;
		}
		else
		{
			spriteRenderer.flipX = true;
		}

		// STEP 3
		//We detect when we are close enough to the targer.
		Vector2 difference = targetPosition - newPosition;
		float distance = difference.magnitude;

		if (distance < 0.5f) 
		{
			DoWhenHitTargert();
		}

	}

	private void DoWhenHitTargert()
	{
		//update the target so that its the next waypoint.
		//But if we were already at the last waypoint of the path, we loop to the first waypoint.
		currentWpIndex++;
		int lastIndex = waypoints.Length - 1;
	    
		if (currentWpIndex > lastIndex) 
		{
			currentWpIndex = 0;
		}

		currentTarget = waypoints [currentWpIndex];

		//we can actually update the currentTarget.

		print ("Hit");
	
	}


}
