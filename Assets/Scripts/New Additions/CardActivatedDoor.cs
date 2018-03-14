using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActivatedDoor : MonoBehaviour {

    [SerializeField]
    [Range(1,4)]
    private int doorSecurityLevel = 0;
    [SerializeField]
    [Tooltip("Can the door be unlocked by blue keycards.")]
    private bool blueCardDoor = false;
    [SerializeField]
    [Tooltip("Can the door be unlocked by security level cards.")]
    private bool keycardDoor = false;

    private void Awake()
    {
        if(blueCardDoor && !keycardDoor)
        {
            doorSecurityLevel = 5;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (doorSecurityLevel == 4 && UpgradesManager.List["keycard4"])
        {
            gameObject.SetActive(false);
        }
        else if (doorSecurityLevel == 3 && UpgradesManager.List["keycard3"])
        {
            gameObject.SetActive(false);
        }
        else if (doorSecurityLevel == 2 && UpgradesManager.List["keycard2"])
        {
            gameObject.SetActive(false);
        }
        else if (doorSecurityLevel == 1 && UpgradesManager.List["keycard1"])
        {
            gameObject.SetActive(false);
        }
        else if (blueCardDoor && UpgradesManager.List["bluekeycard"])
        {
            gameObject.SetActive(false);
        }
    }
}
