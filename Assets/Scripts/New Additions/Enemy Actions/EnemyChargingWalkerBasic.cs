using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChargingWalkerBasic : MonoBehaviour {

    [SerializeField]
    private EnemyInfo enemyInfo;
    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    bool isInitFacingLeft;
    [SerializeField]
    private float chargeRadius = 3.0f;
    [SerializeField]
    private float chargeSpeed = 9.0f;
    [SerializeField]
    private float chargeCooldown = 1.0f;

    GameObject pc;
    private Rigidbody2D rb;
    SpriteRenderer spritey;
    int facingDir;
    bool checkPc = true;
    private Transform leftSensor;
    private Transform rightSensor;
    private Transform bottomRightSensor;
    private Transform bottomLeftSensor;
    private Transform frontSensor;
    private Transform bottomFrontSensor;

    private Transform currentTarget;
    private int chargeDir;
    bool isCharging;
    int movingDir;

    float endTime;
    [SerializeField]
    float knockBackForce = 5f;
    [SerializeField]
    float knockbackDuration = 0.2f;
    Vector2 knockBackVector;

    private void Start()
    {
        pc = GameObject.FindObjectOfType<PCController>().gameObject;
        currentTarget = pc.transform;

        leftSensor = GameObject.Find("Left Sensor").GetComponent<Transform>();
        rightSensor = GameObject.Find("Right Sensor").GetComponent<Transform>();
        bottomRightSensor = GameObject.Find("Bottom Right Sensor").GetComponent<Transform>();
        bottomLeftSensor = GameObject.Find("Bottom Left Sensor").GetComponent<Transform>();
        spritey = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        if(isInitFacingLeft)
        {
            facingDir = Alias.LEFT;
            spritey.flipX = false;
        }
        else
        {
            facingDir = Alias.RIGHT;
            spritey.flipX = true;
        }
        rb.velocity = new Vector2(speed * facingDir, rb.velocity.y);

        enemyInfo.currState = EnemyInfo.States.patrolling;
    }

    void FixedUpdate ()
    {
        if (enemyInfo.currState == EnemyInfo.States.patrolling)
            patrolMove();

        if (enemyInfo.currState == EnemyInfo.States.engaged)
            goCharge();

        if (enemyInfo.currState == EnemyInfo.States.knockback)
            Knockback();

    }

    void flip()
    {
        rb.velocity = Vector2.zero;
        facingDir = -facingDir;
        spritey.flipX = !spritey.flipX;
    }

    void patrolMove()
    {
        frontSensor = facingDir == Alias.LEFT ? leftSensor : rightSensor;
        bottomFrontSensor = facingDir == Alias.LEFT ? bottomLeftSensor : bottomRightSensor;
        chargeDir = ((currentTarget.position.x - transform.position.x) > 0) ? 1 : -1;

        Collider2D rangeColl = Physics2D.OverlapCircle(transform.position, chargeRadius, 1 << Alias.LAYER_PC_TRIGGER);
        Collider2D frontCollHit = Physics2D.OverlapPoint(frontSensor.position, Alias.LAYERMASK_TILEMAP);
        Collider2D bottomCollHit = Physics2D.OverlapPoint(bottomFrontSensor.position, Alias.LAYERMASK_TILEMAP);
        if (frontCollHit != null)
        {
            flip();
        }
        else if (bottomCollHit == null)
        {
            flip();
        }

        rb.velocity = new Vector2(speed * facingDir, rb.velocity.y);

        if (rangeColl != null && checkPc)
        {
            enemyInfo.currState = EnemyInfo.States.engaged;
            isCharging = true;
        }
    }

    void goCharge()
    {
        frontSensor = facingDir == Alias.LEFT ? leftSensor : rightSensor;
        bottomFrontSensor = facingDir == Alias.LEFT ? bottomLeftSensor : bottomRightSensor;

        spritey.flipX = chargeDir == 1 ? true : false;

        Collider2D rangeColl = Physics2D.OverlapCircle(transform.position, chargeRadius, 1 << Alias.LAYER_PC_TRIGGER);
        Collider2D frontCollHit = Physics2D.OverlapPoint(frontSensor.position, Alias.LAYERMASK_TILEMAP);
        Collider2D bottomCollHit = Physics2D.OverlapPoint(bottomFrontSensor.position, Alias.LAYERMASK_TILEMAP);
        if(rangeColl == null)
        {
            if (facingDir != chargeDir)
            {
                spritey.flipX = !spritey.flipX;
            }
            StartCoroutine(stopCharge());
        }
        else if (frontCollHit != null)
        {
            flip();
            StartCoroutine(stopCharge());
        }
        else if (bottomCollHit == null)
        {
            flip();
            StartCoroutine(stopCharge());
        }

        if (isCharging)
        {
            rb.velocity = new Vector2(chargeSpeed * chargeDir, 0);
        }

    }

    IEnumerator stopCharge()
    {
        isCharging = false;
        rb.velocity = Vector2.zero;
        enemyInfo.currState = EnemyInfo.States.patrolling;

        checkPc = false;

        float endTime = Time.time + chargeCooldown;
        do
        {
            yield return null;
        } while (Time.time < endTime);

        checkPc = true;

    }

    void Knockback()
    {
        StartCoroutine("coKnockback");
    }

    IEnumerator coKnockback()
    {
        float delta = pc.transform.position.x - rb.transform.position.x;
        knockBackVector = new Vector2(knockBackForce * Mathf.Sign(-delta), 0);
        rb.AddForce(knockBackVector, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackDuration);
        enemyInfo.currState = EnemyInfo.States.patrolling;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chargeRadius);
    }
}
