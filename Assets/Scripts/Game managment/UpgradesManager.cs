using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Class that contains the list of upgrades and their respective status (unlocked or not), and sends an event when an upgrade has just been unlocked
/// </summary>
public class UpgradesManager : MonoBehaviour
{
	static public event System.Action<string> OnUnlocked;

    //Set this to true or false however you want it to be at the start of game.
    static public Dictionary<string, bool> List = new Dictionary<string, bool>()
    {
		//true if unlocked
		{ "wall jump", true },
        { "bounce", true},
        { "fireball 1", false },
        { "unibike", true },
        { "dash", true },
        { "melee", true },
        { "slam", true },
        { "keycard1", false },
        { "keycard2", false },
        { "keycard3", false },
        { "keycard4", false },
        { "keycard5", false },
        { "bluekeycard", false },
        { "chiptohimself" , false }, { "meetalec" , false }, { "backatthejunction" , false }
    };


    /// <summary>
    /// Overwrite List with the saved values
    /// </summary>
    /// <param name="svgList">The saved upgrade list passed as parameter</param>
    static public void UpdateFromSavedData(List<SvgSerializableUpgrade> svgList)
	{
		List = new Dictionary<string, bool>();

		foreach (SvgSerializableUpgrade upg in svgList)
		{
			List.Add(upg.name, upg.status);
		}
	}


	//Don't use that in your code
	static public void DoOnUpgradePicked(string upgradeName)
	{
		if (!List.ContainsKey(upgradeName))
		{
			Debug.LogErrorFormat("<color=red>ERROR: upgrade name doesn't correspond to any key in UpgradesManage.List</color>");//DEBUG
			return;
		}

		List[upgradeName] = true;

		if (OnUnlocked != null)
			OnUnlocked(upgradeName);
	}


	/// <summary>
	/// 4MENU: DEBUG For use in Debug Menu (you might be able to use it in game though)
	/// </summary>
	static public void DoOnUpgradeLost(string upgradeName)
	{
		if (!List.ContainsKey(upgradeName))
		{
			Debug.LogErrorFormat("<color=red>ERROR: upgrade name doesn't correspond to any key in UpgradesManage.List</color>");//DEBUG
			return;
		}

		List[upgradeName] = false;

		if (OnUnlocked != null)
			OnUnlocked(upgradeName);
	}
}
