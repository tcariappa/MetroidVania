using UnityEngine;
using System.Collections;


public class NavigationPoint : MonoBehaviour
{
	private const float SWITCH_RANGE = 5f;

	public bool isCheckpoint = true;
	public bool isExit = false;
	public int iD = 0;
	public string exitToScene;
	public int entryPointID = 0;
	public bool mustPCFaceLeft;

	/// <summary>
	/// true when the PC has been some distance away from it (to avoid trigger when the PC just spawns in it)
	/// </summary>
	bool isOn;

	static public event System.Action<NavigationPoint> OnTriggered;


	void Awake()
	{
		isOn = false;
	}


	void Start()
	{
		//if this nav point is only an entry point, once PCPositioner has positionned the PC (i.e. before the Start() method of NavigationPoint executes), we can deactivate the object
		if (!isCheckpoint && !isExit)
		{
			gameObject.SetActive(false);
		}
	}


	void Update()
	{
		if (!isOn)
		{
			maySwitchOn();
		}
	}


	/// <summary>
	/// Switch ON if PC is far enough from it
	/// </summary>
	void maySwitchOn()
	{
		Vector2 d = transform.position - PCController.Me.transform.position;
		if (d.sqrMagnitude > SWITCH_RANGE * SWITCH_RANGE)
		{
			isOn = true;
		}
	}


	void OnTriggerEnter2D(Collider2D otherColl)
	{
		if (!isOn)
			return;

		if (otherColl.gameObject.layer == Alias.LAYER_PC_SOLID)
		{
			if (OnTriggered != null)
				OnTriggered(this);

			//if it is a checkpoint we switch it off till the PC moves away from it, to avoid saving it repeatedly
			if (!isCheckpoint)
				isOn = false;
		}
	}

}
