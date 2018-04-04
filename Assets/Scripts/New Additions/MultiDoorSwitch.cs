using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDoorSwitch : MonoBehaviour {
    public bool isActivated = false;
    [SerializeField]
    private GameObject door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Alias.LAYER_PC_TRIGGER)
        {
            isActivated = true;
            door.GetComponent<MultiDoor>().checkUnlock();
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
