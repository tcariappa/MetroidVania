using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles collisions between PC and other entities that can potentially be passed through (most enemies, enemy projectiles, some obstacles...)
/// </summary>
public class PCCollTriggerManager : MonoBehaviour
{
    PCController pc;

	//Events
	public static event System.Action<float> OnHit;

    private void Start()
    {
        pc = GameObject.Find("PC").GetComponent<PCController>();
    }
    void Update()
	{
		//DEBUG
		simulateHitDEBUG();
	}


	//handle collisions with 'trigger' entities (that can be passed through)
	private void OnTriggerEnter2D(Collider2D otherCol)
	{
		//Check if collide with enemy 
		if (otherCol.gameObject.layer == Alias.LAYER_ENEMIES)
		{
            if (pc.currState != PCController.State.slamming)
            {
                EnemyCollManager enemyColl = otherCol.gameObject.GetComponent<EnemyCollManager>();
                EnemyInfo enemyCollided = otherCol.gameObject.GetComponent<EnemyInfo>();
                enemyColl.onCollidedByPC();
                if (OnHit != null)
                {
                    OnHit(enemyCollided.damageOnContact);
                }
            }
			//depending on the enemy type and state (e.g. is it in fire state?) the PC can be more or less damaged, knocked back, etc.
		}
		else if (otherCol.gameObject.layer == Alias.LAYER_ENEMY_PROJECTILES)
		{
			BulletEnemy bullet = otherCol.gameObject.GetComponent<BulletEnemy>();
			bullet.onHitPC();

			if (OnHit != null)
				OnHit(bullet.damage);
		}
	}

    /// <summary>
    /// DEBUG TEST
    /// </summary>
    void simulateHitDEBUG()
	{
		if (Input.GetKeyDown(KeyCode.H))
		{
			if (OnHit != null)
				OnHit(20f);
		}
	}

}
