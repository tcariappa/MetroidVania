using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamEnemyMove : MonoBehaviour {

    [SerializeField]
    float amplitude;
    [SerializeField]
    float angularSpeed;

    bool mustOscillate = true;
    float defaultAngle;
    float currDelta;


    void Start()
    {
        defaultAngle = transform.eulerAngles.z;
        currDelta = 0f;
        mustOscillate = true;
        StartCoroutine(coOscillate());
    }

    IEnumerator coOscillate()
    {
        do
        {
            currDelta += angularSpeed * Time.deltaTime;
            if (currDelta > amplitude / 2f)
            {
                currDelta = amplitude / 2f;
                angularSpeed *= -1;
            }
            else if (currDelta < -amplitude / 2f)
            {
                currDelta = -amplitude / 2f;
                angularSpeed *= -1;
            }
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, defaultAngle + currDelta);
            yield return null;
        } while (mustOscillate);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, defaultAngle);
    }
}
