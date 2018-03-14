using UnityEngine;
using System.Collections;

public abstract class TriggeringObject : TriggerableObject
{
	[SerializeField]
	protected TriggerableObject[] componentsToTrigger;
	bool mustDisableEventually = true;


	protected void triggerOthersAndFinish()
	{
		foreach (TriggerableObject obj in componentsToTrigger)
		{
			obj.triggerMe();
		}

		if (mustDisableEventually)
			enabled = false;
	}


	protected void mayDisable()
	{
		if (mustDisableEventually)
			enabled = false;
	}
}
