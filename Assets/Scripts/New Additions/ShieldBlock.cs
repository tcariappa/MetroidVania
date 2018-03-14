using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBlock : MonoBehaviour {
    PCController pc;
    public static event System.Action OnHitShield;

    private void Awake()
    {
        pc = GameObject.Find("PC").GetComponent<PCController>();
    }

    /*private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.layer == Alias.LAYER_ENEMIES)
        {
            pc.doShieldHit();
            Debug.Log("Collision Detected");
        }
    }*/
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == Alias.LAYER_ENEMIES)
        {
            if(OnHitShield != null)
            {
                OnHitShield();
            }
            pc.doShieldHit();
        }
    }
}
