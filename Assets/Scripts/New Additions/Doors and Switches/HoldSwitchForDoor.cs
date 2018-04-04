using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldSwitchForDoor : MonoBehaviour {

    public GameObject[] holdDoors;


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == Alias.LAYER_PC_SOLID)
        {
            for (int i = 0; i < holdDoors.Length; i++)
                holdDoors[i].SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == Alias.LAYER_PC_SOLID)
        {
            for (int i = 0; i < holdDoors.Length; i++)
                holdDoors[i].SetActive(true);
        }
    }
}
