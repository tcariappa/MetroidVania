using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharge : MonoBehaviour {

    [SerializeField] EnemyInfo enemyInfo;
    [SerializeField] private float chargeRadius = 3.0f;
    [SerializeField] private float chargeSpeed = 9.0f;
    [SerializeField] private float chargeCooldown = 1.0f;

    Transform currentTarget;
    GameObject pc;
    Rigidbody2D rb;
    bool checkPc;
    bool isCharging;

    private void Start()
    {
        pc = GameObject.FindObjectOfType<PCController>().gameObject;
        currentTarget = pc.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Collider2D rangeColl = Physics2D.OverlapCircle(transform.position, chargeRadius, 1 << Alias.LAYER_PC_TRIGGER);

        if (rangeColl != null)
        {
            isCharging = true;
            enemyInfo.currState = EnemyInfo.States.engaged;
        }

        if(enemyInfo.currState == EnemyInfo.States.engaged)
        {
            goCharge();
        }
    }

    void goCharge()
    {
        Vector2 tgtPos = pc.transform.position;
        Vector2 rbDiff = tgtPos - rb.position;
        Vector2 moveDirection = rbDiff.normalized;
 
        if (isCharging)
        {
            rb.velocity = moveDirection * chargeSpeed;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chargeRadius);
    }
}
