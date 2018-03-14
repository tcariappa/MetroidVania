using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {

    Collider2D attackColliderBox;
    SpriteRenderer spritey;
    [HideInInspector]
    public bool collOn;
    float attackTimer = 2f;
    public float attackCool;
    public float damage = 10;
    public PCController pc;
    
    private void Awake()
    {
        spritey = gameObject.GetComponent<SpriteRenderer>();
        attackColliderBox = gameObject.GetComponent<Collider2D>();
        attackColliderBox.enabled = false;
        spritey.enabled = false;
        collOn = false;
    }

    private void OnEnable()
    {
        PCInputsManager.OnPressMelee += handleOnPressMelee;

    }
    void OnDisable()
    {
        PCInputsManager.OnPressMelee -= handleOnPressMelee;
    }

    private void Update()
    {
        if (collOn)
        {
            pc.goMeleeAttack();

            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
           
            }
            else
            {
                endAttack();
            }
        }
    }

    void handleOnPressMelee()
    {
        if (collOn == false && UpgradesManager.List["melee"] && (pc.currState == PCController.State.idle || pc.currState == PCController.State.running || pc.currState == PCController.State.regJumping || pc.currState == PCController.State.falling
            || pc.currState == PCController.State.falling || pc.currState == PCController.State.bounceFall || pc.currState == PCController.State.bounceJump))
        {
            attackColliderBox.enabled = true;
            spritey.enabled = true;
            collOn = true;
            attackTimer = attackCool;           
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == Alias.LAYER_ENEMIES)
        {
            col.GetComponent<EnemyCollManager>().onHitByAttack(damage);
            endAttack();   
        }
    }

    void endAttack()
    {
        collOn = false;
        attackColliderBox.enabled = false;
        spritey.enabled = false;
        pc.goMeleeAttack();
    }

}
