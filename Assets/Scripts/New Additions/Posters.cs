using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posters : MonoBehaviour {
    [SerializeField] private GameObject image;
    bool posterOn;

    void OnEnable()
    {
        MiscInputsManager.OnPressAction += handleOnPressAction;
        MiscInputsManager.OnReleaseAction += handleOnReleaseAction;
    }

    void handleOnPressAction()
    {
        posterOn = true;
    }

    void handleOnReleaseAction()
    {
        posterOn = false;
    }

    void Start ()
    {
        image.SetActive(false);
	}

    private void OnTriggerStay2D(Collider2D coll)
    {
        if(coll.gameObject.layer == Alias.LAYER_PC_TRIGGER)
        {
            if (posterOn && !image.activeInHierarchy)
            {
                image.SetActive(true);
            }
            //else image.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.gameObject.layer == Alias.LAYER_PC_TRIGGER)
        {
            image.SetActive(false);
        }
    }
}
