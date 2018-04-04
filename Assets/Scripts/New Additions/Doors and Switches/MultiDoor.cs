using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDoor : MonoBehaviour {

    public GameObject[] switches;
    bool[] isUnlocked;
    bool doorCanUnlock = false;

    private bool unlockDoor = false;

    private void OnEnable()
    {
        PCInputsManager.OnPressInteract += handleOnPressInteract;
        PCInputsManager.OnReleaseInteract += handleOnReleaseInteract;
    }

    private void handleOnReleaseInteract()
    {
        unlockDoor = false;
    }

    private void handleOnPressInteract()
    {
        unlockDoor = true;
        if(doorCanUnlock && unlockDoor)
        unlockDoorAction();
    }

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
                doorCanUnlock = false;
                break;
            }
            else doorCanUnlock = true;
        }
        unlockDoorAction();
    }
    void unlockDoorAction()
    {
        if (doorCanUnlock && unlockDoor)
        {
            gameObject.SetActive(false);
        }
    }
}
