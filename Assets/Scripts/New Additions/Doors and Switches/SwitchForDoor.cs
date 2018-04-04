using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchForDoor : MonoBehaviour {

    public GameObject[] doors;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        if(coll.gameObject.layer == Alias.LAYER_PC_SOLID)
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = Color.cyan ;
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].GetComponent<SwitchOperatedDoors>().openDoor();
            }
        }
    }

}
