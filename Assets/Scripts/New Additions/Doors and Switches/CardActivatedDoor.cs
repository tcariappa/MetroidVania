using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActivatedDoor : MonoBehaviour {

    [SerializeField]
    [Range(1,5)]
    private int doorSecurityLevel = 0;
    [SerializeField]
    [Tooltip("Can the door be unlocked by blue keycards.")]
    private bool blueCardDoor = false;
    [SerializeField]
    [Tooltip("Can the door be unlocked by security level cards.")]
    private bool keycardDoor = false;
    [SerializeField]
    [Tooltip("Is it a horizontal or vertical door")]
    private bool isHorizontal = false;
    [SerializeField]
    private float distance = 5.0f;

    bool startDoor = false;
    private float horizontalDistance;
    private float verticalDistance;
    Vector2 moveDoor;

    private bool unlockDoor = false;

    private Animator anim;

    private void OnEnable()
    {
        PCInputsManager.OnPressInteract += handleOnPressInteract;
        PCInputsManager.OnReleaseInteract += handleOnReleaseInteract;
    }

    private void handleOnReleaseInteract()
    {
        unlockDoor = false;
    }

    private void handleOnPressInteract()
    {
        unlockDoor = true;
    }

    private void Start()
    {
        horizontalDistance = verticalDistance = 5;
        if (isHorizontal)
        {
            verticalDistance = 0;
        }
        else horizontalDistance = 0;

        moveDoor = new Vector2(horizontalDistance, verticalDistance);
        anim = gameObject.GetComponent<Animator>();
    }

    private void OnValidate()
    {
        if((blueCardDoor && !keycardDoor) || (!blueCardDoor && !keycardDoor))
        {
            doorSecurityLevel = 6;
        }
    }

    private void openDoor()
    {
        anim.SetTrigger("Door Open");
        StartCoroutine(coOpenDoor());
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.layer == Alias.LAYER_PC_TRIGGER && unlockDoor)
        {
            if (doorSecurityLevel == 5 && UpgradesManager.List["keycard5"])
            {
                openDoor();
            }
            else if (doorSecurityLevel == 4 && UpgradesManager.List["keycard4"])
            {
                openDoor();
            }
            else if (doorSecurityLevel == 3 && UpgradesManager.List["keycard3"])
            {
                openDoor();
            }
            else if (doorSecurityLevel == 2 && UpgradesManager.List["keycard2"])
            {
                openDoor();
            }
            else if (doorSecurityLevel == 1 && UpgradesManager.List["keycard1"])
            {
                openDoor();
            }
            else if (blueCardDoor && UpgradesManager.List["bluekeycard"])
            {
                openDoor();
            }
        }
    }

    private void FixedUpdate()
    {
        if(startDoor)
        transform.Translate(moveDoor * Time.deltaTime);
    }

    IEnumerator coOpenDoor()
    {
        yield return new WaitForSeconds(0.75f);

        startDoor = true;

        float endTime = Time.time + distance;
        do
        {
            yield return null;
        } while (Time.time < endTime);

        startDoor = false;
        //gameObject.SetActive(false);
    }
}
