using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour {

<<<<<<< HEAD
=======
    [SerializeField]
    private float conveyorBeltSpeed = 5.0f;
    [SerializeField]
    private bool isRight = false;
    private int direction = 1;
    private Vector2 addMove;
    [SerializeField]
    private LayerMask layers;

    private void Awake()
    {
        direction = isRight ? 1 : -1;
        addMove = new Vector2(conveyorBeltSpeed * direction, 0f);
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == Alias.LAYER_PC_TRIGGER)
        {
            coll.gameObject.GetComponentInParent<PCController>().defaultMove = addMove;
        }
        //else if (coll.gameObject.layer != (Alias.LAYER_PC_SOLID | Alias.LAYER_PC_TRIGGER))
        //{
        //    coll.attachedRigidbody.velocity = addMove;
        //}
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.layer != layers)
        { 
            coll.attachedRigidbody.velocity = addMove;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == Alias.LAYER_PC_TRIGGER)
        {
            coll.gameObject.GetComponentInParent<PCController>().defaultMove = Vector2.zero;
        }
        //else if (coll.gameObject.layer != (Alias.LAYER_PC_SOLID | Alias.LAYER_PC_TRIGGER))
        //{
        //    coll.attachedRigidbody.velocity = Vector2.zero;
        //}
    }
>>>>>>> master
}
