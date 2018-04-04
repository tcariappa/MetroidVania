using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDoorSwitch : MonoBehaviour {

    public GameObject doors;
    BoxCollider2D boxy;

    private void Start()
    {
        boxy = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {

        if(coll.gameObject.layer == Alias.LAYER_PC_SOLID)
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = Color.cyan ;
            doors.GetComponent<TimedDoor>().openDoor();              
        }
    }

    private void Update()
    {
        if (doors.GetComponent<TimedDoor>().doorActive)
        {
            boxy.enabled = false;
        }
        else boxy.enabled = true;
    }
}
