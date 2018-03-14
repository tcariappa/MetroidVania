using UnityEngine;
using System.Collections;


public class CamFollow : MonoBehaviour
{
	public enum state
	{
		regular
	}

	[SerializeField]
	private Vector2 offsetReg = new Vector2(1f, 1f);
	[SerializeField]
	private float smoothTime = 0.5f;

	float myZ;
	state currState;
	//GameManager gameMngr;
	PCController pc;
	private Vector2 _vel4Damp;


	void Awake()
	{
		//gameMngr = GameManager.Me;
	}


	// Use this for initialization
	void Start()
	{
		myZ = transform.position.z;

		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PCController>();

		//init cam pos
		float xTgt = pc.transform.position.x + offsetReg.x * pc.facingDir;
		Vector3 newPos = new Vector3(xTgt, pc.transform.position.y + offsetReg.y, myZ);
		transform.position = newPos;

		goRegular();
	}


	// Had to move camera in FixedUpdate (instead of Update or LateUpdate) to avoid jitter with the main character. I Don't like this, but not much choice...
	void LateUpdate()
	{
		if (currState == state.regular) {
			followPC();
		}
	}


	void goRegular()
	{
		currState = state.regular;
	}


	void followPC()
	{
		/* There are two ways to avoid camera jitter when smoothly following a character with a dynamic Rigidbody:
		 * 1) Update camera's position at screen refresh intervals i.e. Update() or LateUpdate(), and set the Interpolate property of character's rigidbody to "interpolate".
		 *    We may use Time.smoothDeltaTime instead of Time.deltaTime whenever we want to access the framerate, the movement seems even cleaner.
		 * 2) Update camera's position along with character's, i.e. at FixedUpdate().
		 *    It works, but I don't like it as the camera isn't a physics object.
		 */

		Vector2 tgt = new Vector2(pc.transform.position.x + offsetReg.x * pc.facingDir, pc.transform.position.y + offsetReg.y);

		//Smoothed movement
		Vector2 newPos2D = Vector2.SmoothDamp(transform.position, tgt, ref _vel4Damp, smoothTime, 200f, Time.smoothDeltaTime);

		transform.position = new Vector3(newPos2D.x, newPos2D.y, transform.position.z);
	}

}
