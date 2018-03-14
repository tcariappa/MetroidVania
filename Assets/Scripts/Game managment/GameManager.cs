using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
	public enum State
	{
		intro,
		playing,
		gameOver,
		exitingLevel
	}

	public static GameManager Me;

	public float introDuration = 0.7f;
	public float gameOverDuration = 1.6f;
	public float exitingDuration = 1.0f;

	public State currState { get; private set; }
	public float stateStartTime { get; private set; }
	public float stateEndTime { get; private set; }

	bool hasOrderedExit;
	string sceneToLoad;

	//EVENTS
	static public event System.Action OnPlayStart;
	static public event System.Action OnGameOver;
	static public event System.Action OnExitingLevel;


	// Use this for initialization
	void Awake()
	{
		Alias.InitSomeAliases();

		Me = this;

		goIntro();
	}


	//Special "event function" of Unity; executes every time this script becomes enabled
	void OnEnable()
	{
		//registering to events
		MiscInputsManager.OnPressQuit += handleOnPressQuit;
		MiscInputsManager.OnPressPause += handleOnPressPause;
		PCHealthManager.OnDeath += handleOnPCDeath;
		NavigationPoint.OnTriggered += handleOnNavTrigger;
	}


	void OnDisable()
	{
		//unregistering from events (important to free memory)
		MiscInputsManager.OnPressQuit -= handleOnPressQuit;
		PCHealthManager.OnDeath -= handleOnPCDeath;
		NavigationPoint.OnTriggered -= handleOnNavTrigger;
	}


	void Start()
	{
	}


	void handleOnPressQuit()
	{
		//We exit the game
		Application.Quit();
	}


	void handleOnPressPause()
	{
		//check if timeSCale == 0...
		if (Time.timeScale == 0f)
		{
			//...ww unpause the game
			Time.timeScale = 1f;
		}
		else
		{
			//We PAUSE the game
			Time.timeScale = 0f;

			print("Game paused!");//TEST
		}

	}


	void handleOnPCDeath()
	{
		goGameOver();
	}


	void handleOnNavTrigger(NavigationPoint navTrigger)
	{
		//We go to exitingLevel state if the navigation trigger triggered by the PC is an exit
		if (navTrigger.isExit)
		{
			sceneToLoad = navTrigger.exitToScene;

			goExitingLevel();
		}
	}


	// Update is called once per frame
	void Update()
	{
		switch (currState)
		{
			case State.intro:
				doInIntro();
				break;
			case State.playing:
				doInPlay();
				break;
			case State.gameOver:
				doInGameOver();
				break;
			case State.exitingLevel:
				doInExitingLevel();
				break;
		}
	}


	void goIntro()
	{
		currState = State.intro;
		stateStartTime = Time.time;
		stateEndTime = stateStartTime + introDuration;
	}


	void doInIntro()
	{
		if (Time.time >= stateEndTime)
		{
			goPlay();
		}
	}


	void goPlay()
	{
		currState = State.playing;

		if (OnPlayStart != null)
		{
			OnPlayStart();
		}
	}


	void doInPlay()
	{
	}


	void goGameOver()
	{
		currState = State.gameOver;
		stateStartTime = Time.time;
		stateEndTime = stateStartTime + gameOverDuration;

		if (OnGameOver != null)
		{
			OnGameOver();
		}
	}


	void doInGameOver()
	{
		if (Time.time >= stateEndTime)
		{
			if (!string.IsNullOrEmpty(sceneToLoad))
			{
				SceneManager.LoadScene(sceneToLoad);
			}
			else
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}


	void goExitingLevel()
	{
		hasOrderedExit = false;
		currState = State.exitingLevel;
		stateStartTime = Time.time;
		stateEndTime = stateStartTime + exitingDuration;

		if (OnExitingLevel != null)
		{
			OnExitingLevel();
		}
	}


	void doInExitingLevel()
	{
		if (hasOrderedExit)
			return;

		if (Time.time >= stateEndTime)
		{
			hasOrderedExit = true;
			SceneManager.LoadScene(sceneToLoad);
		}
		else
		{
		}
	}

}