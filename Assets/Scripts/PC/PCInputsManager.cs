using UnityEngine;
using System.Collections;

public class PCInputsManager : MonoBehaviour
{
	public int movingDir { get; private set; }
	public Vector2 moveInput { get; private set; }
	public Vector2 mouseInput { get; private set; }

	public bool useSmoothInputs = false;

	bool areControlsEnabled;
    PCController pc;
	//Events
	public static event System.Action OnPressJump;
	public static event System.Action OnPressMelee;
	public static event System.Action OnReleaseFire1;
	public static event System.Action OnPressDash;
    public static event System.Action OnPressUnibike;
    public static event System.Action OnPressSlam;
    public static event System.Action OnPressShield;
    public static event System.Action OnReleaseShield;
    public static event System.Action OnPressInteract;
    public static event System.Action OnReleaseInteract;


    // Use this for initialization
    void Awake()
	{
		areControlsEnabled = false;
		movingDir = Alias.STILL;
		mouseInput = Vector2.zero;
        pc = gameObject.GetComponent<PCController>();
	}


	void OnEnable()
	{
		GameManager.OnPlayStart += handleOnPlayStart;
		GameManager.OnExitingLevel += handleOnExitingLevel;
		GameManager.OnGameOver += handleOnGameOver;
	}


	void OnDisable()
	{
		GameManager.OnPlayStart -= handleOnPlayStart;
		GameManager.OnExitingLevel -= handleOnExitingLevel;
		GameManager.OnGameOver -= handleOnGameOver;
	}


	private void handleOnPlayStart()
	{
		areControlsEnabled = true;
	}


	private void handleOnExitingLevel()
	{
		areControlsEnabled = false;
	}


	private void handleOnGameOver()
	{
		areControlsEnabled = false;
	}


	// Update is called once per frame
	void Update()
	{
		if (areControlsEnabled)
		{
			updateMoveInputs();
			updateJumpInputs();
			updateFireInputs();

			updateMouseInputs();
		}
	}


	void updateMoveInputs()
	{
		//direction vector
		if (useSmoothInputs)
		{
			moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		}
		else
		{
			moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		}

		//moving direction
		if (moveInput.x > 0.25f)
		{
			movingDir = Alias.RIGHT;
		}
		else if (moveInput.x < -0.25f)
		{
			movingDir = Alias.LEFT;
		}
		else
		{
			movingDir = Alias.STILL;
		}
	}


	void updateJumpInputs()
	{
		//jump
		if (Input.GetButtonDown("Jump"))
		{
			if (OnPressJump != null)
				OnPressJump();
		}
		//Not reset here coz PCController checks the inputs on FixedUpdate(), which might execute twice per frame or once every other frame...
	}


	void updateFireInputs()
	{
        if (Input.GetButtonDown("Attack"))
        {
            if (OnPressMelee != null)
                OnPressMelee();
        }

        if (Input.GetButtonDown("Dash"))
        {
            if (OnPressDash != null)
                OnPressDash();
        }

        if (Input.GetButtonDown("Unibike"))
        {
            if (OnPressUnibike != null)
                OnPressUnibike();
        }

        if(Input.GetButtonDown("Slam"))
        {
            if (OnPressSlam != null)
                OnPressSlam();
        }

        if(Input.GetButtonDown("Shield"))
        {
            pc.isShieldButtonPressed = true;
            if (OnPressShield != null)
                OnPressShield();
        }

        if(Input.GetButtonUp("Shield"))
        {
            pc.isShieldButtonPressed = false;
            if (OnReleaseShield != null)
                OnReleaseShield();
        }

        if(Input.GetAxisRaw("Interact") > 0)
        {
            if (OnPressInteract != null)
                OnPressInteract();
        }
        if (Input.GetAxisRaw("Interact") == 0)
        {
            if (OnReleaseInteract != null)
                OnReleaseInteract();
        }
    }


    void updateMouseInputs()
	{
		mouseInput.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
	}

}
