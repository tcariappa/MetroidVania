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
            print("Enemy hit");
            if(OnHitShield != null)
            {
                OnHitShield();
            }
            pc.doShieldHit();
        }
    }
    private void OnTriggerEnter2D(Collider2D colli)
    {
        if (colli.gameObject.layer == Alias.LAYER_ENEMY_PROJECTILES)
        {
            print("Enemy projectile hit");
            BulletEnemy bullet = colli.gameObject.GetComponent<BulletEnemy>();
            bullet.onHitPC();

            if (OnHitShield != null)
            {
                OnHitShield();
            }
            pc.doShieldHit();
        }
    }
}
