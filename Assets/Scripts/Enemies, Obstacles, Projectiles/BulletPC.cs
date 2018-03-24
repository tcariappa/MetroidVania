using UnityEngine;
using System.Collections;
using System;

public class BulletPC : BulletBase
{
	[SerializeField]
	Transform explosionVfxRef;

	private float damage;


	public void init(Vector3 pos, Vector3 rot, float spd, float dmg)
	{
		transform.eulerAngles = rot;
		rb.rotation = rot.z;
		rb.position = transform.position = pos;
		speed = spd;
		damage = dmg;
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		//detect collision of player's bullet with the map
		if (col.gameObject.layer == Alias.LAYER_TILEMAP || col.gameObject.layer == Alias.LAYER_BREAKABLE_SURFACE)
		{
			hitMap();
			destroyMyself();
		}
		//check collision of PC's bullet with enemies
		else if (col.gameObject.layer == Alias.LAYER_ENEMIES)
		{
			col.GetComponent<EnemyCollManager>().onHitByAttack(damage);
			destroyMyself();
		}
	}


	void hitMap()
	{
		//instantiate an explosion vfx object
		//Instantiate(explosionVfxRef, transform.position, Quaternion.identity);
	}

}
