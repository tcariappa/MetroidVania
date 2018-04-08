using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Handles inputs that are not used for PC control
/// </summary>
public class MiscInputsManager : MonoBehaviour
{
	//definition of a simple event that is called OnPressQuit; this one is just a message
	static public System.Action OnPressQuit;
	static public System.Action OnPressPause;
	static public System.Action OnPressAction;//4DIALOGS
    static public System.Action OnReleaseAction;

    void Update()
	{
		updateQuitInput();
		updatePauseInput();
		updateActionInput();//4DIALOGS

		updateDEBUG();//DEBUG
	}


	void updateQuitInput()
	{
		if (Input.GetButtonDown("Back") || Input.GetKeyDown(KeyCode.Escape))
		{
			//Sending OnPressQuit event (It's just a messqge)
			if (OnPressQuit != null)
				OnPressQuit();
		}
	}


	void updatePauseInput()
	{
		//If the user presses Pause virtual button or P key...
		if (Input.GetButtonDown("Pause") || Input.GetKeyDown(KeyCode.P))
		{
			//...we send OnPressPause event
			if (OnPressPause != null)
				OnPressPause();
		}
	}


	//4DIALOGS
	void updateActionInput()
	{
		if (Input.GetButtonDown("Interact"))
		{
			if (OnPressAction != null)
				OnPressAction();         
		}

        if(Input.GetButtonUp("Interact"))
        {
            if (OnReleaseAction != null)
                OnReleaseAction();
        }
	}


	/// <summary>
	/// DEBUG TEST
	/// </summary>
	void updateDEBUG()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			//Slowing time down
			if (Time.timeScale != 1f)
			{
				Time.timeScale = 1f;
				Debug.Log("Setting time flow to normal");
			}
			else
			{
				Time.timeScale = 0.2f;
				Debug.Log("Slowing time down for debug");
			}
		}
	}

}
