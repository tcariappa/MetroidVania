using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchForDoor : MonoBehaviour {

    public GameObject[] doors;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.layer == Alias.LAYER_PC_SOLID)
        {
            for(int i=0; i<doors.Length; i++)
            Destroy(doors[i]);
        }
    }
}
