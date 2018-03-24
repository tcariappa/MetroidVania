using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDoor : MonoBehaviour {

    public GameObject[] switches;
    bool[] isUnlocked;
    bool doorUnlock = false;

    private void Start()
    {
        //doorUnlock = false;
        isUnlocked = new bool[switches.Length];
        for( var i = 0; i<switches.Length; i++)
        {
            isUnlocked[i] = switches[i].GetComponent<MultiDoorSwitch>().isActivated;
        }
    }
    public void checkUnlock()
    {
        for (var i = 0; i < switches.Length; i++)
        {
            isUnlocked[i] = switches[i].GetComponent<MultiDoorSwitch>().isActivated;
        }

        foreach(bool check in isUnlocked)
        {
            if (!check)
            {
                doorUnlock = false;
                break;
            }
            else doorUnlock = true;
        }
        unlockDoor();
    }
    void unlockDoor()
    {
        if (doorUnlock)
        {
            Destroy(gameObject);
        }
    }
}
