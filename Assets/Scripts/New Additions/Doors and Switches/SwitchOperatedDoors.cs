using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOperatedDoors : MonoBehaviour {
    [SerializeField]
    [Tooltip("Is it a horizontal or vertical door")]
    private bool isHorizontal = false;
    [SerializeField]
    private float distance = 5.0f;

    bool startDoor = false;
    private float horizontalDistance;
    private float verticalDistance;
    Vector2 moveDoor;

    private Animator anim;

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

    public void openDoor()
    {
        anim.SetTrigger("Door Open");
        StartCoroutine(coOpenDoor());
    }

    private void FixedUpdate()
    {
        if (startDoor)
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
    }

}
