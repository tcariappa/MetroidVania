using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour {

    [SerializeField] private GameObject obj;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.layer == Alias.LAYER_PC_TRIGGER)
            obj.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == Alias.LAYER_PC_TRIGGER)
            obj.SetActive(false);
    }
}
