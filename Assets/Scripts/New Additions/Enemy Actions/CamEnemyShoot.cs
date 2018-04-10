using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamEnemyShoot : MonoBehaviour {

    EnemyShoot Shooter;

    private void Start()
    {
        Shooter = GetComponentInParent<EnemyShoot>();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.layer == Alias.LAYER_PC_TRIGGER)
        {
            Shooter.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == Alias.LAYER_PC_TRIGGER)
        {
            Shooter.StopAllCoroutines();
            Shooter.enabled = false;
        }
    }
}
