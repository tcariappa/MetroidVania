using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonsMovement : MonoBehaviour {

    private Vector2 initialPosition;
    private Rigidbody2D rb;
    private Transform bottomSensor;
    [SerializeField]
    private float chargeSpeed;
    [SerializeField]
    private float returnSpeed;

    private void Awake()
    {
        initialPosition = (Vector2)transform.position;
        rb = GetComponent<Rigidbody2D>();
        bottomSensor = GetComponentInChildren<Transform>();
    }

    private void FixedUpdate()
    {
        Collider2D collHit = Physics2D.OverlapPoint(bottomSensor.position);
        if (collHit != null)
        {
            resetPosition();
        }
    }

    void attackDown()
    {
        rb.velocity = new Vector2(0, -chargeSpeed);
    }

    private void resetPosition()
    {
        print("Resetposition Triggered");
        Vector2 returnPosition = Vector2.MoveTowards(transform.position, initialPosition, returnSpeed * Time.deltaTime);
    }
}
