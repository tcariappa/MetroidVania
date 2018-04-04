using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDoor : MonoBehaviour {

    [SerializeField]
    [Tooltip("Is it a horizontal or vertical door")]
    private bool isHorizontal = false;
    [SerializeField]
    private float distance = 5.0f;
    [SerializeField]
    private float openTime = 0.0f;

    bool closeDoor = false;
    bool startDoor = false;
    private float horizontalDistance;
    private float verticalDistance;
    Vector2 moveDoor;
    public bool doorActive { get; private set; }

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
        //anim.SetTrigger("Door Open");
        StartCoroutine(coOpenDoor());
    }

    private void FixedUpdate()
    {
        if (startDoor)
            transform.Translate(moveDoor * Time.deltaTime);
        else if (closeDoor)
            transform.Translate(-moveDoor * Time.deltaTime);
    }

    IEnumerator coOpenDoor()
    {
        doorActive = true;
        //yield return new WaitForSeconds(0.75f);

        startDoor = true;

        float endTime = Time.time + distance;
        do
        {
            yield return null;
        } while (Time.time < endTime);

        startDoor = false;
        StartCoroutine(coCloseDoor());
    }
    IEnumerator coCloseDoor()
    {
        yield return new WaitForSeconds(openTime);

        closeDoor = true;

        float endTime = Time.time + distance;
        do
        {
            yield return null;
        } while (Time.time < endTime);

        closeDoor = false;
        doorActive = false;
    }
}
