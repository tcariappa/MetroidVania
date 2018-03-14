using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
	[SerializeField]
	protected Rigidbody2D rb;
	[SerializeField]
	protected float speed = 2f;
	[SerializeField]
	protected Timer doomTimer;

	void Start()
	{
		//registering to event
		if (doomTimer != null)
			doomTimer.OnTimesUp += handleOnTimesUp;
	}


	private void handleOnTimesUp()
	{
		destroyMyself();
	}


	public void onHitByExplosion()
	{
		destroyMyself();
	}


	void Update()
	{
		//compute the bullet's new position
		Vector2 move = new Vector2(speed * Time.deltaTime, 0f);

		//Local space to global space
		Vector2 newPos = transform.TransformPoint(move);
		//Vector2 newPos = rb.position + move;

		//Update the bullet's position
		transform.position = rb.position = newPos;
	}


	void OnBecameInvisible()
	{
		destroyMyself();
	}


	protected void destroyMyself()
	{
		//destroy this whole gameobject
		Destroy(gameObject);
		//unregistering from event
		if (doomTimer != null)
			doomTimer.OnTimesUp -= handleOnTimesUp;
	}
}