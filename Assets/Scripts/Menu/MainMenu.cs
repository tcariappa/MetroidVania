using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		//check for input to exit the game
		if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.Escape))
		{
			ExitApp();
		}

	}


	public void DoOnClickPlayBtn()
	{
		if (!string.IsNullOrEmpty(SvgManager.SvgData.currentSceneName))
		{
			SceneManager.LoadScene(SvgManager.SvgData.currentSceneName);
		}
		else
		{
			//WARNING: the following number must be the build index of first scene that should be loaded when we click PLAY for the first time
			SceneManager.LoadScene(1);
		}
	}


	public void ExitApp()
	{
		Application.Quit();
	}
}
