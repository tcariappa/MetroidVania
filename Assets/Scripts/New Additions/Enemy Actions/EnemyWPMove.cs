using UnityEngine;
using System.Collections;

public class EnemyWPMove : MonoBehaviour{

    [SerializeField] private float speed = 5;
    [SerializeField] Transform[] trgts;

    Rigidbody2D rb;
    Transform tgt;
    SpriteRenderer sr;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        tgt = trgts[0];
    }

    private void FixedUpdate()
    {

        Vector2 delta = (Vector2)tgt.position - rb.position;
        Vector2 move = delta.normalized;

        float dist = delta.magnitude;
        sr.flipX = tgt.position.x > transform.position.x ? true : false;
        if(dist < 0.25f)
        {
            doWhenHitWaypoint();
        }

        rb.velocity = new Vector2(move.x * speed, 0);
    }

    void doWhenHitWaypoint()
    {

        tgt = tgt == trgts[0] ? trgts[1] : trgts[0];
    }
}
