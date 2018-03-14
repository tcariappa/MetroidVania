using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;


/// <summary>
/// Type that holds data that are serialized and stored in PlayerPrefs
/// </summary>
[XmlRoot("SavedData")]
public class SvgSerializableData
{
	[XmlElement("Scene")]
	public string currentSceneName;
	[XmlElement("Checkpt")]
	public int currCheckpointID;
	//[XmlElement("Hp")]
	//public int currHPs;
	/// <summary>
	/// List and status of upgrades as saved.
	/// WARNING: do not use to retrieve current status, use UpgradesManager.List instead!
	/// </summary>
	[XmlElement("Upgs")]
	public List<SvgSerializableUpgrade> upgrades { get; private set; }


	//Constructor
	public SvgSerializableData() { }


	/// <summary>
	/// Recreate the List SvgData.upgrades with the list of upgrades in memory (i.e. UpgradesManager.List)
	/// </summary>
	public void generateSvgUpgsList()
	{
		upgrades = new List<SvgSerializableUpgrade>();
		foreach (KeyValuePair<string, bool> entry in UpgradesManager.List)
		{
			upgrades.Add(new SvgSerializableUpgrade(entry.Key, entry.Value));
		}
	}
}


public class SvgSerializableUpgrade
{
	[XmlAttribute("n")]
	public string name;
	[XmlAttribute("s")]
	public bool status;


	//Constructor 1
	public SvgSerializableUpgrade() { }


	//Constructor 2
	public SvgSerializableUpgrade(string myName, bool myStatus)
	{
		name = myName;
		status = myStatus;
	}
}

