using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnibikeHealthManager : MonoBehaviour {

    static public UnibikeHealthManager me;

    public float maxUnibikeHitPoints = 100.0f;
    public float currentUnibikeHp { get; private set; }

    private void Awake()
    {
        me = this;
        currentUnibikeHp = maxUnibikeHitPoints;
    }

    void OnEnable()
    {
        ShieldBlock.OnHitShield += handleOnHitShield;
    }

    void OnDisable()
    {
        ShieldBlock.OnHitShield -= handleOnHitShield;
    }

    private void handleOnHitShield()
    {
        unibikeTakeDamage(20.0f);
    }

    private void unibikeTakeDamage(float dmg)
    {
        if (currentUnibikeHp > 0)
        {
            currentUnibikeHp -= dmg;
        }
        else currentUnibikeHp = 0;
    }
}    
