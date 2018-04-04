using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityMine : MonoBehaviour {

    [SerializeField]
    private float damage = 20.0f;
    [SerializeField]
    private float mineAoe = 3.5f;
    [SerializeField]
    private Transform explosionVfxRef;
    [SerializeField]
    private bool isTimed = false;
    [SerializeField]
    private float explodeTimer = 2;
    [SerializeField]
    private bool turnsOff;

    bool isInCollider;
    CircleCollider2D areaColl;

    public static event System.Action<float> OnMine;

    void Start()
    {
        areaColl = gameObject.GetComponent<CircleCollider2D>();
        areaColl.radius = mineAoe;
        if (!isTimed)
            explodeTimer = 0.0f;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.layer == Alias.LAYER_PC_TRIGGER)
        {
            isInCollider = true;
            StartCoroutine("coMineExplodeTimer");
        }
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == Alias.LAYER_PC_TRIGGER)
        {
            isInCollider = false;
            if (turnsOff)
            {
                StopCoroutine("coMineExplodeTimer");
            }
        }
    }

    void farewell()
    {
        //instantiate an explosion object
        if (explosionVfxRef != null)
            Instantiate(explosionVfxRef, transform.position, Quaternion.identity);

        //destroy this whole gameobject (including the particle system component)
        //Destroy(gameObject);
        //print("gameObject Destroyed");
        Destroy(transform.parent.gameObject);
    }

    IEnumerator coMineExplodeTimer()
    {
            float endTime = Time.time + explodeTimer;
            do
            {
                yield return null;
            } while (Time.time < endTime);

            if (OnMine != null && isInCollider)
            {
                OnMine(damage);
            }
                farewell();
    }
}
