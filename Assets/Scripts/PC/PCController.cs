using UnityEngine;
using System.Collections;
using System;

public class PCController : MonoBehaviour
{
    public enum State
    {
        idle,
        blocked,
        running,
        dashing,
        regJumping,
        wallJumping,
        falling,
        clingingToWall,
        grabbingLedge,
        hangingLedge,
        climbingLedge,
        dying,
        exiting,
        unibikeIdle,
        unibikeMove,
        unibikeDashing,
        unibikeJumping,
        unibikeFall,
        unibikeBounceFall,
        unibikeBounceJump,
        bounceFall,
        bounceJump,
        meleeAttack,
        knockBack,
        slamming,
        shielding
    }

    static public PCController Me;
    MeleeAttack melee;
    UnibikeHealthManager uniHealth;

    //Unity user-editable variables
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private PCCollSolidManager collMngr;
    [SerializeField]
    private PCInputsManager inputs;
    [SerializeField]
    private FlippableObject[] objectsToFlip;
    [SerializeField]
    [Tooltip("Speed while on foot.")]
    private float runSpeed = 7f;
    [SerializeField]
    [Tooltip("Speed while on Unibike.")]
    private float bikeSpeed = 11f;
    [SerializeField]
    [Tooltip("How much the character can be controlled in air after jumping.")]
    private float regJumpXInputInfluence = 10f;
    [SerializeField]
    [Tooltip("How much the character can be controlled in air after jumping while on unibike.")]
    private float unibikeJumpXInputInfluence = 10f;
    [SerializeField]
    [Tooltip("How much the character can be controlled in air after wall jumping.")]
    private float wallJumpXInputInfluence = 10f;
    [SerializeField]
    [Tooltip("How much the character can be controlled in air while falling.")]
    private float fallXInputInfluence = 20f;
    [SerializeField]
    [Tooltip("Regular first jump height.")]
    private float regJumpImpulse = 18.5f;
    [SerializeField]
    [Tooltip("Bounce ability height.")]
    private float bounceImpulse = 20f;
    [SerializeField]
    [Tooltip("Power of wall jump.")]
    private float wallJumpImpulse = 900f;
    [SerializeField]
    [Tooltip("Angle at which character wall jumps.")]
    private float wallJumpAngle = 60f;
    [SerializeField]
    /// <summary>
    /// Time during which a wall jump is locked against air control
    /// </summary>
    private float wallJumpLockDuration = 0.12f;
    [SerializeField]
    [Tooltip("How long the character can hold on to a wall.")]
    private float holdWallDuration = 1f;
    [SerializeField]
    [Tooltip("The duration of the entire ledge climbing action.")]
    private float climbLedgeDuration = 0.3f;
    [SerializeField]
    [Tooltip("Maximum number of times the player can bounce on the unibike.")]
    private int maxBounces = 1;
    [SerializeField]
    [Tooltip("The time for how long a player can dash.")]
    private float dashTime = 0.25f;
    [SerializeField]
    [Tooltip("The cooldown time between each dash.")]
    private float dashCooldown = 1.0f;
    [SerializeField]
    [Tooltip("The speed at which the player can dash for the dash time specified.")]
    private float dashSpeed = 15.0f;
    [SerializeField]
    [Tooltip("The speed at which the player can dash on the unibike for the dash time specified.")]
    private float unibikeDashSpeed = 15.0f;
    [SerializeField]
    [Tooltip("The time till which a player is knocked back.")]
    private float knockbackDuration = 0.2f;
    [SerializeField]
    [Tooltip("The force at which the player is knocked back.")]
    private float knockForce = 5.0f;
    [SerializeField]
    [Tooltip("The force at which the player slams towards the ground.")]
    private float slamSpeed = 10.0f;
    [SerializeField]
    [Tooltip("The area of effect of the slam on ground.")]
    private float slamArea = 5.0f;
    [SerializeField]
    [Tooltip("Damage caused by player slam ability")]
    private float slamDamage = 10.0f;
    [SerializeField]
    [Tooltip("The layer that the slam will effect.")]
    private LayerMask slamLayers;
    [SerializeField]
    [Tooltip("The max number of hits shield can take.")]
    private int shieldMaxHits = 5;
    [SerializeField]
    [Tooltip("The time the shield takes to regenerate hitpoints.")]
    private float shieldRegenTime = 2.0f;

    //Public properties protected against external writing
    public State currState { get; private set; }
    public int facingDir { get; private set; } //+1 PC moves right, -1 PC moves left, 0 PC idle
    public Vector2 defaultMove {get; set;} //value that's always added to make sure that it moves in conveyor direction

    //Private variables
    int movingDir;
    bool isWallJumpLocked;
    /// <summary>
    /// if hasJustJumped is true we won't ground the PC if its sensors haven't left the ground yet right after a Jump
    /// </summary>
    bool hasJustJumped;
    bool hasJustDashed;
    Vector2 wallJumpVector;
    int jumpDir;
    float endTime;
    bool canCatchWall;
    Vector2 hangingAtLedgePos;
    Vector2 ledgePos;
    bool isJumpOrdered;
    float maxXSpeedInAir;
    bool isHit;
    float slope;
    Vector2 slopeVector;
    int remainingBounces;
    bool isBike;
    bool isDashOrdered = false;
    bool isBounceOrdered = false;
    bool isSlamOrdered = false;
    Vector2 knockBackDirection;
    //Slam Components
    Rigidbody2D enemyRb;
    Vector2 enemyKnockback; //Enemy knockback vector for the slam.
    Vector2 centre; //centre for the slam down.
    //Shield variables
    EdgeCollider2D shieldColl;
    SpriteRenderer shieldSprite;
    bool isShield = false;
    int shieldHitsLeft;
    bool isShieldRegen = false;
    [HideInInspector]
    public bool isShieldButtonPressed = false;

    void Awake()
    {
        Me = this;

        currState = State.idle;
        isWallJumpLocked = false;
        hasJustJumped = false;
        isBike = false;

        movingDir = Alias.STILL;

        //by default PC looks right (dir == 1)
        facingDir = Alias.RIGHT;

        wallJumpVector = new Vector2(wallJumpImpulse * Mathf.Cos(wallJumpAngle * Mathf.Deg2Rad), wallJumpImpulse * Mathf.Sin(wallJumpAngle * Mathf.Deg2Rad));
        jumpDir = Alias.STILL;

        shieldColl = GameObject.Find("ShieldCollider").GetComponent<EdgeCollider2D>();
        shieldSprite = GameObject.Find("ShieldCollider").GetComponent<SpriteRenderer>();
        uniHealth = gameObject.GetComponent<UnibikeHealthManager>();
    }


    void OnEnable()
    {
        PCInputsManager.OnPressJump += handleOnPressJump;
        PCInputsManager.OnPressUnibike += handleOnPressUnibike;
        PCInputsManager.OnPressDash += handleOnPressDash;
        PCInputsManager.OnPressSlam += handleOnPressSlam;
        PCInputsManager.OnPressShield += handleOnPressShield;
        PCInputsManager.OnReleaseShield += handleOnReleaseShield;
        PCHealthManager.OnDeath += handleOnDeath;
        PCHealthManager.OnDamaged += handleOnDamaged;
        NavigationPoint.OnTriggered += handleOnNavTrigger;
    }

    void OnDisable()
    {
        PCInputsManager.OnPressJump -= handleOnPressJump;
        PCInputsManager.OnPressUnibike -= handleOnPressUnibike;
        PCInputsManager.OnPressDash -= handleOnPressDash;
        PCInputsManager.OnPressShield -= handleOnPressShield;
        PCInputsManager.OnReleaseShield -= handleOnReleaseShield;
        PCHealthManager.OnDeath -= handleOnDeath;
        PCHealthManager.OnDamaged -= handleOnDamaged;
        NavigationPoint.OnTriggered -= handleOnNavTrigger;
    }


    // Use this for initialization
    void Start()
    {
        canCatchWall = UpgradesManager.List["wall jump"];
        remainingBounces = maxBounces;
        melee = GameObject.Find("MeleeCollider").GetComponent<MeleeAttack>();
        shieldHitsLeft = shieldMaxHits;

        shieldColl.enabled = false;
        shieldSprite.enabled = false;
    }


    private void handleOnPressJump()
    {
        isJumpOrdered = true;
    }

    private void handleOnPressDash()
    {
        if (UpgradesManager.List["dash"] && !hasJustDashed && (currState == State.running || currState == State.idle || currState == State.unibikeIdle || currState == State.falling || currState == State.regJumping ||
            currState == State.bounceJump || currState == State.bounceFall || currState == State.wallJumping || currState == State.unibikeBounceJump))
            isDashOrdered = true;
        else Debug.Log("Dash is locked");
    }

    private void handleOnPressSlam()
    {
        if (UpgradesManager.List["slam"] && (currState == State.regJumping || currState == State.falling || 
            currState == State.unibikeJumping || currState == State.unibikeFall || currState == State.wallJumping))
            isSlamOrdered = true;
    }

    private void handleOnPressUnibike()
    {
        if (UpgradesManager.List["unibike"])
            isBike = !isBike;
        else Debug.Log("Unibike is locked");
    }

    private void handleOnDamaged(float dmgTaken, float remainingHp)
    {
        if(currState != State.slamming)
            goKnockback();            
    }

    void handleOnPressShield()
    {
        if(shieldHitsLeft > 0)
        {
            isShield = true;
            goShield();
        }
    }

    void handleOnReleaseShield()
    {
        if (isShield)
        {
            isShield = false;
            goShield();
        }
    }

    private void handleOnDeath()
    {
        goDying();
    }


    void handleOnNavTrigger(NavigationPoint navTrigger)
    {
        //PC goes to exiting level if the navigation trigger is an exit
        if (navTrigger.isExit)
        {
            goExiting();
        }
    }

    /// <summary>
    /// Teleports the PC to the indicated location and orients it as ordered. Public method because PCPositioner calls it.
    /// </summary>
    public void instantReposition(Vector2 pos, bool mustFaceLeft)
    {
        rb.position = pos;
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);

        if (mustFaceLeft)
        {
            faceLeft();
        }
    }

    public void keycardCollected()
    {
        if (UpgradesManager.List["keycard4"])
        {
            UpgradesManager.DoOnUpgradePicked("keycard5");
        }
        else if (UpgradesManager.List["keycard3"])
        {
            UpgradesManager.DoOnUpgradePicked("keycard4");
        }
        else if (UpgradesManager.List["keycard2"])
        {
            UpgradesManager.DoOnUpgradePicked("keycard3");
        }
        else if (UpgradesManager.List["keycard1"])
        {
            UpgradesManager.DoOnUpgradePicked("keycard2");
        }
        else if (!UpgradesManager.List["keycard1"])
        {
            UpgradesManager.DoOnUpgradePicked("keycard1");
        }
    }

    // FixedUpdate is called once per physics tick
    void FixedUpdate()
    {
        centre = gameObject.transform.position;
        //Special states
        if (currState == State.dying)
        {
            doInDying();
            return;
        }
        else if (currState == State.exiting)
        {
            doInExiting();
            return;
        }

        if (currState == State.dashing)
        {
            doDash();
        }

        if (currState == State.unibikeDashing)
        {
            doUnibikeDash();
        }

        if (currState == State.knockBack)
        {
            doInKnockback();
        }

        if (currState == State.slamming)
        {
            doInSlam();
        }
        //On ground
        if (currState == State.idle || currState == State.running || currState == State.blocked 
            || currState == State.unibikeMove || currState == State.shielding || currState == State.unibikeIdle)
        {
            doInGroundStates();
        }

        //In air
        else if (currState == State.regJumping || currState == State.falling || currState == State.wallJumping || currState == State.unibikeBounceFall ||
            currState == State.bounceJump || currState == State.unibikeJumping || currState == State.unibikeFall || currState == State.bounceFall || currState == State.unibikeBounceJump )
             {
                doInAirStates();
             }

        //Other controllable states
        if (currState == State.grabbingLedge)
        {
            grabLedge();
        }
        else if (currState == State.hangingLedge)
        {
            hangAtLedge();
        }
        else if (currState == State.climbingLedge)
        {
            climbLedge();
        }
        else if (currState == State.clingingToWall)
        {
            clingToWall();
        }

        //Processing Jump. WARNING, only HERE we can process Jump in PCController!
        if (isJumpOrdered)
        {
            if (currState == State.idle || currState == State.unibikeIdle || currState == State.running || currState == State.blocked)
            {
                jumpReg();
            }
            else if (currState == State.clingingToWall && UpgradesManager.List["wall jump"])
            {
                wallJump();
            }
            else if (currState == State.unibikeMove)
            {
                jumpUnibikeReg();
            }
            else if (UpgradesManager.List["bounce"] && (currState == State.regJumping || currState == State.falling || currState == State.unibikeBounceJump||
                currState == State.unibikeJumping || currState == State.unibikeFall || currState == State.bounceJump) && (remainingBounces > 0))
            {
                isBounceOrdered = true;
            }
            //When hanging ledge we check also for an input direction; if it's opposite to the direction the PC is facing, the PC walljumps.
            //If it's down, the PC falls. Otherwise the PC climbs on the ledge.
            //NOTE: eventually we will store climb order during grabbingLedge too, so that when the PC has grabbed the ledge, it will immediately climb.
            else if (currState == State.hangingLedge)
            {
                if (facingDir == Alias.RIGHT && inputs.movingDir == Alias.LEFT && UpgradesManager.List["wall jump"])
                {
                    wallJump();
                }
                else if (facingDir == Alias.LEFT && inputs.movingDir == Alias.RIGHT && UpgradesManager.List["wall jump"])
                {
                    wallJump();
                }
                else
                {
                    goClimbLedge();
                }
            }
            //in any case we reset the jump order because we processed it
            isJumpOrdered = false;
        }

        if (isDashOrdered)
        {
            if (currState == State.idle || currState == State.unibikeIdle || currState == State.running || currState == State.regJumping || 
                currState == State.bounceJump || currState == State.falling || currState == State.wallJumping || currState == State.unibikeBounceJump)
            {
                goDash();
            }
            else if (currState == State.unibikeMove || currState == State.unibikeFall || currState == State.unibikeJumping)
            {
                goUnibikeDash();
            }

            isDashOrdered = false;
        }

        if(isSlamOrdered)
        {
            goSlam();
            isSlamOrdered = false;
        }
    }
    //END FixedUpdate

    void doInGroundStates()
    {
        if (collMngr.groundedState != GroundedStates.no)
        {
            if (!isBike)
            {
                if (inputs.movingDir == Alias.RIGHT)
                {
                    if (!collMngr.isTouchingRightTile)
                    {
                        if (currState == State.running)
                            run();
                        else
                            goRunning();
                    }
                    else
                    {
                        goBlocked();
                    }
                    //making sure PC faces right
                    faceRight();
                }
                else if (inputs.movingDir == Alias.LEFT)
                {
                    if (!collMngr.isTouchingLeftTile)
                    {
                        if (currState == State.running)
                            run();
                        else
                            goRunning();
                    }
                    else
                    {
                        goBlocked();
                    }
                    //making sure PC faces left
                    faceLeft();
                }
                else
                {
                    goIdle();
                }
            }
            //if on bike
            else
            {
                if (inputs.movingDir == Alias.RIGHT)
                {
                    if (!collMngr.isTouchingRightTile)
                    {
                        if (currState == State.unibikeMove)
                            moveOnBike();
                        else
                            goMoveOnBike();
                    }
                    else
                    {
                        goBlocked();
                    }
                    //making sure PC faces right
                    faceRight();
                }
                else if (inputs.movingDir == Alias.LEFT)
                {
                    if (!collMngr.isTouchingLeftTile)
                    {
                        if (currState == State.unibikeMove)
                            moveOnBike();
                        else
                            goMoveOnBike();
                    }
                    else
                    {
                        goBlocked();
                    }
                    //making sure PC faces left
                    faceLeft();
                }
                else
                {
                    goUnibikeIdle();
                }
            }
        }
        else
        { //if PC not grounded
            goFalling();
        }
    }


    void doInAirStates()
    {
        //if still in air or has just jumped
        if (collMngr.groundedState == GroundedStates.no || hasJustJumped)
        {
            if (rb.velocity.y < 0)
            {
                if (isBike && !isBounceOrdered)
                    currState = State.unibikeFall;
                else if (isBike && isBounceOrdered)
                    currState = State.unibikeBounceFall;
                else if (isBounceOrdered)
                    currState = State.bounceFall;
                else currState = State.falling;
            }

            if (currState == State.falling || currState == State.bounceFall)
            {
                doInFall();
            }
            else if (currState == State.unibikeFall || currState == State.unibikeBounceFall)
            {
                doInUnibikeFall();
            }
            else if (currState == State.regJumping)
            {
                doInRegJump();
            }
            else if (currState == State.unibikeJumping)
            {
                doInUnibikeJump();
            }
            else if (currState == State.wallJumping)
            {
                doInWallJump();
            }
            else if (currState == State.unibikeBounceJump)
            {
                doInUnibikeBounceJump();
            }
            else if (currState == State.bounceJump)
            {
                doInBounceJump();
            }

        }
        //if landed
        else
        {
            if (isBounceOrdered)
            {
                if (isBike)
                    unibikeBounceJump();
                else bounceJump();
            }
            else if (inputs.movingDir != Alias.STILL)
            {
                goRunning();
                isBounceOrdered = false;
            }
            else
            {
                if (!isBike)
                    goIdle();
                else goUnibikeIdle();
            }
        }
    }

    void doInKnockback()
    {
        if (Time.time > endTime)
        {
            if (collMngr.isInAir)
            {
                goFalling();
            }
            else
            {
                goIdle();
            }
        }
    }

    void goRunning()
    {
        applyGravity();

        canCatchWall = UpgradesManager.List["wall jump"];
        remainingBounces = maxBounces;

        currState = State.running;

        run();
    }

    void run()
    {
        movingDir = inputs.movingDir;

        //if there's no input we don't move the PC
        if (movingDir == Alias.STILL) return;

        Vector2 move;

        computeSlope();

        //if on flat ground (or in front of a cliff) we move the PC horizontally in the direction of movingDir
        if (slope > -0.02f && slope < 0.02f)
        {
            move = new Vector2(inputs.moveInput.x, 0f);
        }
        //if on slope we move the PC in the direction of the slope
        else
        {
            move = (new Vector2(1f, slope)).normalized;
            move *= inputs.moveInput.x;
            //Depending on the input direction and the general direction of slope we move the PC up or down:
            //if slope and PC's direction are of the same sign, PC goes up. If slope and PC's direction signs are opposite, PC goes down.
            if ((slope > 0f && movingDir == Alias.RIGHT) || (slope < 0f && movingDir == Alias.LEFT))
            {
                if (move.y < 0f)
                    move.y = -move.y;
            }
            else
            {
                if (move.y > 0f)
                    move.y = -move.y;
            }
        }

        move *= runSpeed;
        rb.velocity = move + defaultMove;

        //TEST
        //Vector3 endPt = rb.position + move;
        //lineRendererTEST.SetPosition(0, rb.position);
        //lineRendererTEST.SetPosition(1, endPt);
    }

    void moveOnBike()
    {
        movingDir = inputs.movingDir;

        //if there's no input we don't move the PC
        if (movingDir == Alias.STILL) return;

        Vector2 move;

        move = new Vector2(inputs.moveInput.x, 0f);

        move *= bikeSpeed;
        rb.velocity = move + defaultMove;

    }

    void goMoveOnBike()
    {
        applyGravity();

        currState = State.unibikeMove;

        moveOnBike();

    }

    public void goMeleeAttack()
    {
        if (melee.collOn)
        {
            currState = State.meleeAttack;
        }
        else
        {
            goIdle();
        }
    }

    void goDash()
    {
        cancelGravity();
        rb.velocity = Vector2.zero;
        endTime = Time.time + dashTime;
        currState = State.dashing;
        doDash();
    }

    void goUnibikeDash()
    {
        cancelGravity();
        rb.velocity = Vector2.zero;
        endTime = Time.time + dashTime;
        currState = State.unibikeDashing;
        doUnibikeDash();
    }

    void goSlam()
    {
        if(collMngr.isInAir)
        {
            rb.velocity = new Vector2(0, -slamSpeed);
        }

        currState = State.slamming;
    }

    void goShield()
    {
        if (isShield)
        {
            shieldColl.enabled = true;
            shieldSprite.enabled = true;
        }
        else
        {
            shieldColl.enabled = false;
            shieldSprite.enabled = false;
            goIdle();
        }
    }


    void doDash()
    {
        if (Time.time < endTime && ((facingDir == Alias.RIGHT && !collMngr.isTouchingRightTile) || (facingDir == Alias.LEFT && !collMngr.isTouchingLeftTile)))
        {
            rb.velocity = new Vector2(dashSpeed * facingDir, 0);
        }
        else
        {
            StartCoroutine(coDashCooldown());
            goIdle();
        }
    }

    void doUnibikeDash()
    {
        if (Time.time < endTime && ((facingDir == Alias.RIGHT && !collMngr.isTouchingRightTile) || (facingDir == Alias.LEFT && !collMngr.isTouchingLeftTile)))
        {
            rb.velocity = new Vector2(unibikeDashSpeed * facingDir, 0);
        }
        else
        {
            goIdle();
        }
    }

    void goBlocked()
    {
        applyGravity();
        movingDir = Alias.STILL;

        remainingBounces = maxBounces;

        currState = State.blocked;

        doInBlockedOrIdle();
    }

    void goKnockback()
    {
        endTime = Time.time + knockbackDuration;
        rb.velocity = Vector2.zero;
        knockBackDirection = new Vector2(knockForce * (-facingDir), knockForce);
        rb.AddForce(knockBackDirection, ForceMode2D.Impulse);
        currState = State.knockBack;
    }

    void goIdle()
    {
        applyGravity();
        rb.velocity = Vector2.zero;
        movingDir = Alias.STILL;

        canCatchWall = UpgradesManager.List["wall jump"];
        remainingBounces = maxBounces;

        currState = State.idle;

        doInBlockedOrIdle();
    }

    void doInBlockedOrIdle()
    {
        rb.velocity = defaultMove;
    }

    void goUnibikeIdle()
    {
        applyGravity();
        rb.velocity = Vector2.zero;
        movingDir = Alias.STILL;

        canCatchWall = UpgradesManager.List["wall jump"];
        remainingBounces = maxBounces;

        currState = State.unibikeIdle;

        doInBlockedOrIdle();
    }

    void jumpReg()
    {
        applyGravity();
        remainingBounces = maxBounces;

        rb.velocity = new Vector2(rb.velocity.x, 0f); //to have a jump that looks the same all the time we first reset the current vertical velocity
        Vector2 impulse = new Vector2(0f, regJumpImpulse);
        rb.AddForce(impulse, ForceMode2D.Impulse);

        currState = State.regJumping;

        StartCoroutine(coLockJump());
    }

    void jumpUnibikeReg()
    {
        applyGravity();
        remainingBounces = maxBounces;

        rb.velocity = new Vector2(rb.velocity.x, 0f); //to have a jump that looks the same all the time we first reset the current vertical velocity
        Vector2 impulse = new Vector2(0f, regJumpImpulse);
        rb.AddForce(impulse, ForceMode2D.Impulse);

        currState = State.unibikeJumping;

        StartCoroutine(coLockJump());
    }

    void doInRegJump()
    {
        moveInAir();
    }

    void doInUnibikeJump()
    {
        moveUnibikeInAir();

    }

    void doInSlam()
    {
        
        if(!collMngr.isInAir)
        {
            goIdle();
            //Collider2D[] colls = Physics2D.OverlapCircleAll(centre, slamArea, 1 << LayerMask.NameToLayer("Enemies"));
            Collider2D[] colls = Physics2D.OverlapCircleAll(centre, slamArea, slamLayers);

            for (var i=0; i < colls.Length; i++)
            {
                print("Detected " + colls[i].name);
                if (colls[i].gameObject.layer == Alias.LAYER_ENEMIES)
                {
                    enemyRb = colls[i].gameObject.GetComponent<Rigidbody2D>();
                    float delta = rb.position.x - enemyRb.position.x;
                    enemyKnockback = new Vector2(knockForce * Mathf.Sign(-delta), knockForce);
                    enemyRb.AddForce(enemyKnockback, ForceMode2D.Impulse);
                    colls[i].gameObject.GetComponent<EnemyCollManager>().onHitByAttack(slamDamage);
                }
                else Destroy(colls[i].gameObject);
            }
        }
    }

    void wallJump()
    {
        applyGravity();

        if (facingDir == Alias.RIGHT)
        {
            faceLeft();
            jumpDir = Alias.LEFT;
        }
        else if (facingDir == Alias.LEFT)
        {
            faceRight();
            jumpDir = Alias.RIGHT;
        }
        Vector2 impulse = new Vector2(wallJumpVector.x * facingDir, wallJumpVector.y);
        rb.AddForce(impulse, ForceMode2D.Impulse);

        currState = State.wallJumping;

        StartCoroutine(coLockWallJump());
    }


    void doInWallJump()
    {
        if (!isWallJumpLocked)
        {
            moveInAir();
        }
    }


    void bounceJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f); //to have an airJump that looks the same all the time we first reset the current vertical velocity
        Vector2 impulse = new Vector2(0f, bounceImpulse);
        rb.AddForce(impulse, ForceMode2D.Impulse);

        remainingBounces--;

        isBounceOrdered = false;
        currState = State.bounceJump;

        StartCoroutine(coLockJump());

        //applyGravity();
    }

    void unibikeBounceJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f); //to have an airJump that looks the same all the time we first reset the current vertical velocity
        Vector2 impulse = new Vector2(0f, bounceImpulse);
        rb.AddForce(impulse, ForceMode2D.Impulse);

        remainingBounces--;

        isBounceOrdered = false;
        currState = State.unibikeBounceJump;

        StartCoroutine(coLockJump());

        //applyGravity();
    }

    public void doShieldHit()
    {
        if (shieldHitsLeft > 0)
        {
            shieldHitsLeft--;
        }
        if (shieldHitsLeft == 0)
        {
            handleOnReleaseShield();
        }
        if (shieldHitsLeft < shieldMaxHits && !isShieldRegen)
        {
            StartCoroutine(coShieldRegen());
        }
            goKnockback();
    }

    void doInBounceJump()
    {
        moveInAir();
    }
    
    void doInUnibikeBounceJump()
    {
        moveUnibikeInAir();
    }

    void goFalling()
    {
        applyGravity();

        currState = State.falling;
    }


    void doInFall()
    {
        if (collMngr.isForLeftLedgeGrab && facingDir == Alias.LEFT)
        {
            goGrabLedge(Alias.LEFT);
        }
        else if (collMngr.isForRightLedgeGrab && facingDir == Alias.RIGHT)
        {
            goGrabLedge(Alias.RIGHT);
        }
        else if (collMngr.isTouchingLeftTile && facingDir == Alias.LEFT && canCatchWall)
        {
            catchWall(Alias.LEFT);
        }
        else if (collMngr.isTouchingRightTile && facingDir == Alias.RIGHT && canCatchWall)
        {
            catchWall(Alias.RIGHT);
        }
        else
        {
            moveInAir();
        }
    }

    void doInUnibikeFall()
    {
        if (collMngr.isForLeftLedgeGrab && facingDir == Alias.LEFT)
        {
            goGrabLedge(Alias.LEFT);
        }
        else if (collMngr.isForRightLedgeGrab && facingDir == Alias.RIGHT)
        {
            goGrabLedge(Alias.RIGHT);
        }
        else if (collMngr.isTouchingLeftTile && facingDir == Alias.LEFT && canCatchWall)
        {
            catchWall(Alias.LEFT);
        }
        else if (collMngr.isTouchingRightTile && facingDir == Alias.RIGHT && canCatchWall)
        {
            catchWall(Alias.RIGHT);
        }
        else
        {
            moveUnibikeInAir();
        }
    }

    void moveInAir()
    {
        //Depending on state and initial jump direction, horizontal inputs have different influence for air control
        Vector2 force = Vector2.zero;

        //During reg jump, inputs always have an influence
        if (currState == State.regJumping || currState == State.bounceJump)
        {
            force.x = inputs.moveInput.x * regJumpXInputInfluence;
        }
        //during wall jump, if input same direction as wall jump, there's no influence. If input opposite direction, there's an influence
        else if (currState == State.wallJumping)
        {
            float comboDir = (inputs.movingDir == jumpDir) ? Alias.STILL : inputs.moveInput.x;
            force.x = comboDir * wallJumpXInputInfluence;
        }
        else if (currState == State.falling || currState == State.bounceFall)
        {
            maxXSpeedInAir = runSpeed + defaultMove.x;
            force.x = inputs.moveInput.x * fallXInputInfluence;
        }

        rb.AddForce(force);

        //clamp horizontal velocity
       if (rb.velocity.x > maxXSpeedInAir)
        {
            rb.velocity = new Vector2(maxXSpeedInAir, rb.velocity.y);
        }
        else if (rb.velocity.x < -maxXSpeedInAir)
        {
            rb.velocity = new Vector2(-maxXSpeedInAir, rb.velocity.y);
        }

        //reorient PC (with hysteresis) if changing horizontal movement direction
        //NOTE: could also reorient if pressing a same direction for a certain TIME (because even if blocked by a wall, the player might want the PC to still reorient
        if (facingDir == Alias.RIGHT)
        {
            if (rb.velocity.x < 0f)
            {
                movingDir = Alias.LEFT;
                if (rb.velocity.x < -0.5f)
                {
                    faceLeft();
                }
            }
        }
        else
        {
            if (rb.velocity.x > 0f)
            {
                movingDir = Alias.RIGHT;
                if (rb.velocity.x > 0.5f)
                {
                    faceRight();
                }
            }
        }
    }

    void moveUnibikeInAir()
    {
        //Depending on state and initial jump direction, horizontal inputs have different influence for air control
        Vector2 force = Vector2.zero;

        //During reg jump, inputs always have an influence
        if (currState == State.unibikeJumping || currState == State.unibikeBounceJump)
        {
            force.x = inputs.moveInput.x * unibikeJumpXInputInfluence;
        }
        //during wall jump, if input same direction as wall jump, there's no influence. If input opposite direction, there's an influence
        else if (currState == State.wallJumping)
        {
            float comboDir = (inputs.movingDir == jumpDir) ? Alias.STILL : inputs.moveInput.x;
            force.x = comboDir * wallJumpXInputInfluence;
        }
        else if (currState == State.unibikeFall || currState == State.unibikeBounceFall)
        {
            maxXSpeedInAir = bikeSpeed + defaultMove.x;
            force.x = inputs.moveInput.x * fallXInputInfluence;
        }

        rb.AddForce(force);

        //clamp horizontal velocity
        if (rb.velocity.x > maxXSpeedInAir)
        {
            rb.velocity = new Vector2(maxXSpeedInAir, rb.velocity.y);
        }
        else if (rb.velocity.x < -maxXSpeedInAir)
        {
            rb.velocity = new Vector2(-maxXSpeedInAir, rb.velocity.y);
        }

        //reorient PC (with hysteresis) if changing horizontal movement direction
        //NOTE: could also reorient if pressing a same direction for a certain TIME (because even if blocked by a wall, the player might want the PC to still reorient
        if (facingDir == Alias.RIGHT)
        {
            if (rb.velocity.x < 0f)
            {
                movingDir = Alias.LEFT;
                if (rb.velocity.x < -0.5f)
                {
                    faceLeft();
                }
            }
        }
        else
        {
            if (rb.velocity.x > 0f)
            {
                movingDir = Alias.RIGHT;
                if (rb.velocity.x > 0.5f)
                {
                    faceRight();
                }
            }
        }
    }

    void catchWall(int side)
    {
        movingDir = Alias.STILL;
        remainingBounces = maxBounces;

        //set the timer to fall off the wall
        endTime = Time.time + holdWallDuration;

        cancelGravity(true);

        currState = State.clingingToWall;
    }


    void clingToWall()
    {
        if (collMngr.groundedState != GroundedStates.no)
        {
            goIdle();
            return;
        }
        else if (collMngr.isInAir)
        {
            canCatchWall = false;
            goFalling();
            return;
        }

        //if clinging time limit reached or input direction is significantly down, the PC falls
        if (Time.time >= endTime || inputs.moveInput.y < -0.8f)
        {
            //won't be able to catch another wall before landing
            canCatchWall = false;

            goFalling();
            return;
        }
    }


    /// <summary>
    /// Init the state grabbingLedge where the PC is moved to the correct position to grab the ledge that will be detected here 
    /// </summary>
    /// <param name="side">Whether the Ledge is on the right or left of the PC</param>
    void goGrabLedge(int side)
    {
        Vector2 refPos;
        ledgePos = collMngr.findLedge(side, out refPos);

        //Define the position the PC must have when it'll be hangingLedge
        Vector2 delta = ledgePos - refPos; //difference bw PC's current position and the position where the PC will be grabbing the ledge
        hangingAtLedgePos = rb.position + delta;

        remainingBounces = maxBounces;
        cancelGravity(true);

        currState = State.grabbingLedge;
    }


    /// <summary>
    /// Move fast to the correct position to hang at the ledge
    /// </summary>
    void grabLedge()
    {
        rb.position = hangingAtLedgePos;

        currState = State.hangingLedge;

        //Progresive reposition !May not work properly!
        //Vector2 delta = hangingAtLedgePos - rb.position;
        ////if the PC is close enough to the target position, we can go to hangingLedge state
        //if (Mathf.Abs(delta.y) < 0.1f)
        //{
        //	currState = State.hangingLedge;
        //}
        ////otherwise we move toward the target position (by repositioning the rigidbody because it's easier to avoid going beyond the target position thus miss it)
        //else
        //{
        //	Vector2 dir = delta.normalized;
        //	Vector2 move = dir * 15f * Time.fixedDeltaTime;
        //	Vector2 newPos = rb.position + move;
        //	if (newPos.y < hangingAtLedgePos.y)
        //		newPos.y = hangingAtLedgePos.y;

        //	rb.position = newPos;
        //}
    }


    void hangAtLedge()
    {
        //if input direction is significantly down, the PC falls
        if (inputs.moveInput.y < -0.8f)
        {
            //won't be able to catch another wall before landing (but will be able to catch ledges)
            canCatchWall = false;
            goFalling();
        }
    }


    void goClimbLedge()
    {
        cancelGravity(true);

        endTime = Time.time + climbLedgeDuration;

        currState = State.climbingLedge;
    }


    void climbLedge()
    {
        rb.position = ledgePos;

        if (Time.time >= endTime)
        {
            currState = State.idle;
        }
    }


    void goDying()
    {
        currState = State.dying;
    }


    void doInDying()
    {
        //nothing to do as there's no state after dying
    }


    void goExiting()
    {
        currState = State.exiting;
    }


    void doInExiting()
    {
        //TBD
    }


    void computeSlope()
    {
        //2 main cases:
        //Both bottom sensors touch a tile;
        //in that case we compute the slope of the line defined by both hit points above bottom sensors.
        //(This case is important when at the bottom of a slope going up; to move smoothly we get the averaged slope.)
        //Only one bottom sensor touches a tile;
        //in this case we get the slope from the normal of the single hit point above or under the bottom sensor that is in front of the PC.

        //Both bottom sensors collide with tilemap
        if (collMngr.groundedState == GroundedStates.bothSides)
        {
            //We cast a line from above each bottom sensor, to the sensor, and we store hit point's position.
            Vector2 hitPtL = castLineFromAbove(collMngr.sensorBottomLeft.transform.position);
            Vector2 hitPtR = castLineFromAbove(collMngr.sensorBottomRight.transform.position);

            //compute the slope: a = (yB - yA) / (xB - xA)
            slope = (hitPtR.y - hitPtL.y) / (hitPtR.x - hitPtL.x);
            slopeVector = hitPtR - hitPtL;
        }

        //Only one bottom sensor collides with tilemap
        else
        {
            //we always check above and under the bottom sensor that is in front of the PC
            Vector2 startPt;
            startPt = facingDir == Alias.RIGHT ? collMngr.sensorBottomRight.transform.position : collMngr.sensorBottomLeft.transform.position;
            startPt.y += 0.4f;
            //We cast a ray downward
            RaycastHit2D hit = Physics2D.Raycast(startPt, Vector2.down, 0.8f, Alias.LAYERMASK_TILEMAP | Alias.LAYERMASK_BREAKABLE_SURFACE);
            //if the ray hits a tile, we use the tile's normal to get the slope
            if (hit)
            {
                Vector2 tangent = new Vector2(hit.normal.y, -hit.normal.x);
                slope = tangent.y / tangent.x;
                slopeVector = tangent;
            }
            //if there is nothing under the sensor, it means the PC is facing a cliff. In that case it behaves like on flat ground
            else
            {
                slope = 0f;
                slopeVector = Vector2.right;
            }
        }
    }


    Vector2 castLineFromAbove(Vector2 endPt)
    {
        Vector2 startPt = new Vector2(endPt.x, endPt.y + 0.4f);
        RaycastHit2D hit = Physics2D.Linecast(startPt, endPt, Alias.LAYERMASK_TILEMAP | Alias.LAYERMASK_BREAKABLE_SURFACE);
        if (!hit)
        {
            Debug.LogError("There should be a hit point with the tilemap between " + startPt + " and " + endPt);
            return Vector2.zero;
        }
        return hit.point;
    }


    /// <summary>
    /// Serves to prevent grounding the PC back for a very short time after a Jump, in case sensors haven't left the ground yet
    /// </summary>
    IEnumerator coLockJump()
    {
        hasJustJumped = true;

        float endTime = Time.time + (1f / 20f);
        do
        {
            yield return null;
        } while (Time.time < endTime);

        hasJustJumped = false;
    }

    IEnumerator coLockWallJump()
    {
        isWallJumpLocked = true;

        float endTime = Time.time + wallJumpLockDuration;
        do
        {
            yield return null;
        } while (Time.time < endTime);

        isWallJumpLocked = false;
    }

    IEnumerator coDashCooldown()
    {
        hasJustDashed = true;

        float endTime = Time.time + dashCooldown;
        do
        {
            yield return null;
        } while (Time.time < endTime);

        hasJustDashed = false;
    }

    IEnumerator coShieldRegen()
    {
        isShieldRegen = true;
        float endTime = Time.time + shieldRegenTime;
        do
        {
            yield return null;
        } while (Time.time < endTime);

        if (shieldHitsLeft < shieldMaxHits)
        {
            shieldHitsLeft++;
            if(isShieldButtonPressed)
            {
                handleOnPressShield();
            }
            if (shieldHitsLeft < shieldMaxHits)
            {
                StartCoroutine(coShieldRegen());
            }
            else if (shieldHitsLeft == shieldMaxHits)
            {
                isShieldRegen = false;
            }
        }
        else if (shieldHitsLeft == shieldMaxHits)
        {
            isShieldRegen = false;
        }

    }

    void faceLeft()
    {
        facingDir = Alias.LEFT;
        spriteRenderer.flipX = true;

        foreach (FlippableObject o in objectsToFlip)
        {
            o.flip();
        }
    }


    void faceRight()
    {
        facingDir = Alias.RIGHT;
        spriteRenderer.flipX = false;

        foreach (FlippableObject o in objectsToFlip)
        {
            o.unflip();
        }
    }

    public void cancelGravity(bool mustStopToo = false)
    {
        rb.gravityScale = 0f;
        if (mustStopToo)
            rb.velocity = Vector2.zero;
    }


    public void applyGravity()
    {
        rb.gravityScale = 1f;
    }

}