using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
	static List<bool> DefaultUpgradeValues;

	[SerializeField]
	Toggle firstUpgToggle;

	Toggle[] upgradeToggles;
	List<string> upgradeKeys;


	//This executes BEFORE ANY OTHER SCRIPT (thanks to script execution order)
	private void Awake()
	{
		upgradeKeys = new List<string>(UpgradesManager.List.Keys);

		if (DefaultUpgradeValues == null)
			DefaultUpgradeValues = new List<bool>(UpgradesManager.List.Values);
	}


	void Start()
	{
		upgradeToggles = new Toggle[UpgradesManager.List.Count];
		upgradeToggles[0] = firstUpgToggle;

		//instantiate other toggles and add correct labels
		for (int i = 0; i < upgradeToggles.Length; i++)
		{
			if (i > 0)
			{
				Vector3 newTogglePos = firstUpgToggle.transform.position;
				newTogglePos.y -= 40f * i;
				Toggle newToggle = Instantiate<Toggle>(firstUpgToggle, firstUpgToggle.transform.parent);
				newToggle.transform.position = newTogglePos;
				upgradeToggles[i] = newToggle;
			}
			//label
			Text toggleLabel = upgradeToggles[i].GetComponentInChildren<Text>();
			toggleLabel.text = upgradeKeys[i];
			//on/off state
			upgradeToggles[i].isOn = UpgradesManager.List[upgradeKeys[i]];
		}
	}


	public void DoOnClickUpgradeToggle(Toggle toggleClicked)
	{
		string upgName = toggleClicked.GetComponentInChildren<Text>().text;
		if (toggleClicked.isOn)
		{
			UpgradesManager.DoOnUpgradePicked(upgName);
		}
		else
		{
			UpgradesManager.DoOnUpgradeLost(upgName);
		}
	}


	public void DoOnClickDeleteSaveGameBtn()
	{
		SvgManager.DeleteSavegameDEBUG();

		//reset toogles AND savegame with all upgrades at their default values
		for (int i = 0; i < upgradeToggles.Length; i++)
		{
			string upgName = upgradeToggles[i].GetComponentInChildren<Text>().text;
			upgradeToggles[i].isOn = DefaultUpgradeValues[i];
		}
	}

}
