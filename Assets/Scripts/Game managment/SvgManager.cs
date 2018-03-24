using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.SceneManagement;
using System.Reflection;


/// <summary>
/// Handles savegame data, and related methods to load, update, and save it
/// </summary>
public class SvgManager : MonoBehaviour
{
	//constants
	const string SavedGameName = "F2MV2018";

	static public SvgSerializableData SvgData; //contains saved data


	void Awake()
	{
		loadSvgData();
	}


	void OnEnable()
	{
		NavigationPoint.OnTriggered += handleOnNavTrigger;
		UpgradesManager.OnUnlocked += handleOnUpgradeUnlocked;
	}


	void OnDisable()
	{
		NavigationPoint.OnTriggered -= handleOnNavTrigger;
		UpgradesManager.OnUnlocked -= handleOnUpgradeUnlocked;
	}


	void handleOnNavTrigger(NavigationPoint navTrigger)
	{
		//if the navigation trigger is an exit, we save next scene's name as well as the corresponding entry point ID
		if (navTrigger.isExit)
		{
			updateSpawnpoint(navTrigger.exitToScene, navTrigger.entryPointID);
		}
		//if the navigation trigger is a checkpoint, we save its ID and the currently loaded scene's name
		else if (navTrigger.isCheckpoint)
		{
			updateSpawnpoint(SceneManager.GetActiveScene().name, navTrigger.iD);
		}
		//updateHPs((int)PCHealthManager.Me.hp);
	}


	void handleOnUpgradeUnlocked(string upgradeName)
	{
		updateUpgradeList();
	}


	/// <summary>
	/// Initializes SvgManager.SvgData with serialized entry from PlayerPrefs (or create a blank instance if no saved game yet)
	/// </summary>
	private void loadSvgData()
	{
		XmlSerializer serializer = new XmlSerializer(typeof(SvgSerializableData));
		using (StringReader reader = new StringReader(PlayerPrefs.GetString(SavedGameName)))
		{
			if (PlayerPrefs.HasKey(SavedGameName))
			{
				SvgData = serializer.Deserialize(reader) as SvgSerializableData;
				//We overwrite the Upgrades list that is used in the game, with the values of the saved data
				UpgradesManager.UpdateFromSavedData(SvgData.upgrades);

				//TEST
#if UNITY_EDITOR
				print("Savegame read from PlayerPrefs: Current scene name = " + SvgData.currentSceneName
				+" | Checkpoint ID = " + SvgData.currCheckpointID); //TEST
				DisplayUnserializedDEBUG(); //TEST
#endif
			}
			else
			{
				print("No savegame => creating a new SvgData in memory and using Upgrades.List currently in memory.");//TEST
				generateNewSvgData(); //As there is no savegame we create a new SvgData (and we fill its upgrades member with Upgrades.List) 
			}
		}
	}


	void generateNewSvgData()
	{
		SvgData = new SvgSerializableData();
		//We initialize a new upgrade list to be saved, with Upgrades.List currently in memory
		SvgData.generateSvgUpgsList();
	}


	void updateSpawnpoint(string sceneName, int checkpointID)
	{
		SvgData.currentSceneName = sceneName;
		SvgData.currCheckpointID = checkpointID;

		saveSvgData();
	}


	//void updateHPs(int hp)
	//{
	//	SvgData.currHPs = hp;

	//	saveSvgData();
	//}


	void updateUpgradeList()
	{
		SvgData.generateSvgUpgsList();

		saveSvgData();
	}


	/// <summary>
	/// Serializes and saves SvgData to PlayerPrefs
	/// </summary>
	void saveSvgData()
	{
		//Serializing svgData and saving it to PlayerPrefs' key
		XmlSerializer serializer = new XmlSerializer(typeof(SvgSerializableData));
		using (StringWriter writer = new StringWriter())
		{
			serializer.Serialize(writer, SvgData);
			print("Saved string: " + writer.ToString());//TEST
			PlayerPrefs.SetString(SavedGameName, writer.ToString());
		}

		//Saving to disk
		PlayerPrefs.Save();
	}


#if UNITY_EDITOR

	//DEBUG
	[UnityEditor.MenuItem("IT Debug Tools/Display savegame data as currently in memory [!RUNTIME ONLY]")]
	static void DisplayUnserializedDEBUG()
	{
		FieldInfo[] fields = SvgData.GetType().GetFields(/*BindingFlags.Public | BindingFlags.Instance*/);
		string str = "SvgData fields as in memory: ";
		foreach (FieldInfo a in fields)
		{
			str += "\nvar " + a.ToString() + ", value = " + a.GetValue(SvgData);
		}
		//special case for 'upgrades' as it's not recognized by GetFields
		str += "\nupgrades: ";
		foreach (SvgSerializableUpgrade upg in SvgData.upgrades)
		{
			str += "\n   name: " + upg.name + ", status: " + upg.status;
		}
		print(str);
	}


	//DEBUG
	[UnityEditor.MenuItem("IT Debug Tools/Display an XML serialized version of SvgData (as stored in PlayerPrefs)")]
	static void DisplaySerializedDEBUG()
	{
		string str = "We display a serialized version of SvgData:";
		XmlSerializer serializer = new XmlSerializer(typeof(SvgSerializableData));
		using (StringWriter writer = new StringWriter())
		{
			serializer.Serialize(writer, SvgData);
			str += "\nSerialized SvgData: " + writer.ToString();
		}
		print(str);
	}


	//DEBUG
	[UnityEditor.MenuItem("IT Debug Tools/Delete savegame data [!WARNING!]")]
	static public void DeleteSavegameDEBUG()
	{
		PlayerPrefs.DeleteKey(SavedGameName);
		print("Deleted " + SavedGameName + " key in PlayerPrefs!!!");
	}

#endif

}