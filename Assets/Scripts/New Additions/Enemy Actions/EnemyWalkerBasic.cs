using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkerBasic : MonoBehaviour {

    private Rigidbody2D rb;
    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    bool isInitFacingLeft;
    SpriteRenderer spritey;
    int facingDir;
    public Transform leftSensor;
    public Transform rightSensor;


    private void Start()
    {
        leftSensor = GameObject.Find("SensorRight").GetComponent<Transform>();
        rightSensor = GameObject.Find("SensorLeft").GetComponent<Transform>();
        spritey = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        if(isInitFacingLeft)
        {
            facingDir = Alias.LEFT;
            spritey.flipX = true;
        }
        else
        {
            facingDir = Alias.RIGHT;
            spritey.flipX = false;
        }
        rb.velocity = new Vector2(speed * facingDir, rb.velocity.y);
    }

    void FixedUpdate ()
    {
        Transform frontSensor;
        if(facingDir == Alias.RIGHT)
        {
            frontSensor = GameObject.Find("SensorRight").GetComponent<Transform>();
        }
        else
        {
            frontSensor = GameObject.Find("SensorLeft").GetComponent<Transform>();
        }
        //This is another way to do the same thing.. The question mark checks like an if statement.. And the colon is an else.
        //frontSensor = facingDir == Alias.LEFT ? GameObject.Find("SensorLeft").GetComponent<Transform>() : frontSensor = GameObject.Find("SensorLeft").GetComponent<Transform>();

        Collider2D collHit = Physics2D.OverlapPoint(frontSensor.position);
        if (collHit != null)
        {
            flip();
        }
        rb.velocity = new Vector2(speed * facingDir, rb.velocity.y);
    }
    void flip()
    {
        facingDir = -facingDir;
        spritey.flipX = !spritey.flipX;
       //isInitFacingLeft = !isInitFacingLeft;
        //Start();
    }
}
