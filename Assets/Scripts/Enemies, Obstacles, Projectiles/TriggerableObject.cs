using UnityEngine;
using System.Collections;

public abstract class TriggerableObject : MonoBehaviour
{
	public void triggerMe()
	{
		gameObject.SetActive(true);
		enabled = true;
	}
}
