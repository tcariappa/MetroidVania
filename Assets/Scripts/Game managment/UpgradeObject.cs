using UnityEngine;
using System.Collections;

/// <summary>
/// Script that can be attached to an object with a trigger collider for the PC to pick it up,
/// or another script logic that would call the public method gotcha(), in order to unlock the corresponding upgrade
/// </summary>
public class UpgradeObject : MonoBehaviour
{
	/// <summary>
	/// WARNING: must correspond exactly to an entry of UpgradesManager.List!
	/// </summary>
	public string upgradeName;

	public bool isUnlocked { get; private set; }


	void Awake()
	{
		if (!UpgradesManager.List.ContainsKey(upgradeName))
		{
			Debug.LogErrorFormat("<color=red>ERROR: can't update " + name + ": its upgradeName doesn't correspond to any key in UpgradesManage.List</color>");//DEBUG
			return;
		}

		if (UpgradesManager.List[upgradeName] == true)
		{
			initUnlocked();
		}
		else
		{
			isUnlocked = false;
		}
	}


	/// <summary>
	/// init the object at its unlocked state
	/// </summary>
	void initUnlocked()
	{
		isUnlocked = true;
		enabled = false;
	}


	public void gotcha()
	{
		goUnlocked();

		UpgradesManager.DoOnUpgradePicked(upgradeName);
	}


	void goUnlocked()
	{
		isUnlocked = true;
		enabled = false;
	}


	void OnTriggerEnter2D(Collider2D otherColl)
	{
		if (otherColl.gameObject.layer == Alias.LAYER_PC_SOLID)
		{
			goUnlocked();

			UpgradesManager.DoOnUpgradePicked(upgradeName);
		}
	}

}
