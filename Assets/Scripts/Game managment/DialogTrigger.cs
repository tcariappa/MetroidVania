using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
	[SerializeField]
	string dialogName;


	void Awake()
	{
		if (UpgradesManager.List.ContainsKey(dialogName) && UpgradesManager.List[dialogName] == true)
		{
			goUnlocked();
		}
	}


	void goUnlocked()
	{
		gameObject.SetActive(false);
	}


	void OnTriggerEnter2D(Collider2D otherColl)
	{
		if (otherColl.gameObject.layer == Alias.LAYER_PC_SOLID)
		{
			goUnlocked();

			DialogManager.Me.playDialogByName(dialogName);
		}
	}

}
