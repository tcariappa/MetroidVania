using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyWP : MonoBehaviour {

    [SerializeField] EnemyInfo enemyInfo;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private float speed = 5;
    [SerializeField] Transform[] trgts;

    SpriteRenderer sr;
    Transform tgt;
    int currentWpIndex;

    void Start ()
    {
        currentWpIndex = 0;

        sr = gameObject.GetComponent<SpriteRenderer>();
        tgt = trgts[0];
        //tgt = FindObjectOfType<PCController>().transform;
	}

    private void FixedUpdate()
    {
        if (enemyInfo.currState == EnemyInfo.States.patrolling)
            patrolMove();
    }

    void patrolMove()
    {
        Vector2 tgtPos = tgt.position;
        Vector2 rbDiff = tgtPos - rb.position;
        Vector2 moveDirection = rbDiff.normalized;

        sr.flipX = tgt.position.x < transform.position.x ? true : false;

        rb.velocity = moveDirection * speed;
        //rb.velocity = ((Vector2)tgt.position - rb.position).normalized * speed;  ALTERNATIVE METHOD

        float dist = (tgtPos - rb.position).magnitude;
        if(dist < 0.5f)
        {
            doWhenHitObject();
        }
        //Do it with OnTriggerEnter2D and make the waypoints on TransparetnFX layer.
    }

    void doWhenHitObject()
    {
        currentWpIndex++;
        int lastIndex = trgts.Length - 1;
        if (currentWpIndex > lastIndex)
        {
            currentWpIndex = 0;
        }

        tgt = trgts[currentWpIndex];
    }
}
