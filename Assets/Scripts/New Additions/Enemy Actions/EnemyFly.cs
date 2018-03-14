using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : MonoBehaviour {

    public float speed = 5;
    SpriteRenderer sr;
    public Transform[] trgts;
    public Rigidbody2D rb;
    Transform tgt;
    private int currentWpIndex;

    void Start ()
    {
        currentWpIndex = 0;

        sr = gameObject.GetComponent<SpriteRenderer>();
        tgt = trgts[0];
        //tgt = FindObjectOfType<PCController>().transform;
	}
    private void FixedUpdate()
    {
        Vector2 tgtPos = tgt.position;
        Vector2 rbDiff = tgtPos - rb.position;
        Vector2 moveDirection = rbDiff.normalized;

        rb.velocity = moveDirection * speed;
        //rb.velocity = ((Vector2)tgt.position - rb.position).normalized * speed;  ALTERNATIVE METHOD
    }
    private void Update()
    {

        sr.flipX = tgt.position.x < transform.position.x ? true : false;

        float dist = ((Vector2)tgt.position - rb.position).magnitude;
        if(dist < 0.5f)
        {
            doWhenHitObject();
        }   
    }

    void doWhenHitObject()
    {
        // print("TargetHit");
        //Destroy(gameObject);
        currentWpIndex++;
        int lastIndex = trgts.Length - 1;
        if (currentWpIndex > lastIndex)
        {
            currentWpIndex = 0;
        }

        tgt = trgts[currentWpIndex];
    }
}
