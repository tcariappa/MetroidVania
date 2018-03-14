using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : MonoBehaviour {

    public float speed = 5;
    SpriteRenderer sr;
    public Transform[] trgts;

    Transform tgt;
    private int currentWpIndex;

    void Start ()
    {
        currentWpIndex = 0;

        sr = gameObject.GetComponent<SpriteRenderer>();
        tgt = trgts[0];
        //tgt = FindObjectOfType<PCController>().transform;
	}
	
	void Update()
    {
        Vector2 tgtPos = tgt.position;
        tgtPos.y += 1.0f;
        Vector2 newPosition = Vector2.MoveTowards(transform.position, tgtPos, speed * Time.deltaTime);
        transform.position = newPosition;

        sr.flipX = tgtPos.x < transform.position.x ? true : false;

        Vector2 diff = tgtPos - newPosition;
        float dist = diff.magnitude;
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
