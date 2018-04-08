using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineMovement : MonoBehaviour {

    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    bool isInitFacingLeft;

    [SerializeField] private Transform leftSensor;
    [SerializeField] private Transform rightSensor;
    [SerializeField] private Transform bottomRightSensor;
    [SerializeField] private Transform bottomLeftSensor;
    private Transform frontSensor;
    private Transform bottomFrontSensor;
    int facingDir;

    Rigidbody2D rb;
    SpriteRenderer spritey;

    private void Start()
    {
        spritey = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        if (isInitFacingLeft)
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
    }

    void Update()
    {
        frontSensor = facingDir == Alias.LEFT ? leftSensor : rightSensor;
        bottomFrontSensor = facingDir == Alias.LEFT ? bottomLeftSensor : bottomRightSensor;

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
    }

    void flip()
    {
        rb.velocity = Vector2.zero;
        facingDir = -facingDir;
        spritey.flipX = !spritey.flipX;
    }
}
