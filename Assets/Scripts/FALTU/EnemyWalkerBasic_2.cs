using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkerBasic_2 : MonoBehaviour 
{

	public Rigidbody2D rb; 
	public SpriteRenderer spriteRenderer;
	[Range (1f, 10f)]
	public float speed;
	public bool isInitFacingLeft; 
	public Transform sensorLeft;
	public Transform sensorRight;

	int facingDir; 

	// Use this for initialization
	void Start () 
	{

		if (isInitFacingLeft) {
			facingDir = Alias.LEFT;
			spriteRenderer.flipX = true;
		} 
		else 
		{
			facingDir = Alias.RIGHT;
		}
			
	}
	
	//Update is called once per frame
	void FixedUpdate () 
	{
		// 1) What sensor is in front of the character?
		Transform frontSensor;
		if (facingDir == Alias.LEFT) 
		{
			frontSensor = sensorLeft;
		} 
		else 
		{
			frontSensor = sensorRight;
		}
		// 2) If there is a tilemap at the position of the sensor?
		Collider2D colliderHit = Physics2D.OverlapPoint(frontSensor.position, Alias.LAYERMASK_TILEMAP);

		// 3) If a collider was hit, it means that the character is facing a wall, 
		if (colliderHit != null) 
		{
			TurnBack();
		}

		rb.velocity = new Vector2 (speed * facingDir , rb.velocity.y);
			
	}

	private void TurnBack()
	{
		facingDir *= -1;
		spriteRenderer.flipX = !spriteRenderer.flipX; 
	}		

}
