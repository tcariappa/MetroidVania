using UnityEngine;
using System.Collections;
using System;

public class Explosion : MonoBehaviour
{
	[SerializeField]
	bool isDamagingPlayer;
	[SerializeField]
	bool isDamagingEnemies;
	[SerializeField]
	bool isDestroyEnemyBullets;
	[SerializeField]
	bool isDestroyPCBullets;
	[SerializeField]
	float radius = 5f;
	[SerializeField]
	float damage = 50f;
	[SerializeField]
	float lifespan;

	static public event System.Action<float> OnHitPlayerShip;

	int layerMask;


	void Awake()
	{
		//bitwise operations to construct the layer mask to define what collision layers will be checked by the explosion
		layerMask = 0;
		if (isDamagingPlayer)
			layerMask = 1 << Alias.LAYER_PC_SOLID;
		if (isDamagingEnemies)
			layerMask |= 1 << Alias.LAYER_ENEMIES;
		if (isDestroyEnemyBullets)
			layerMask |= 1 << Alias.LAYER_ENEMY_PROJECTILES;
		if (isDestroyPCBullets)
			layerMask |= 1 << Alias.LAYER_PC_PROJECTILES;
	}


	void Start()
	{
		explode();

		StartCoroutine(coLive());
	}


	private void explode()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);

		foreach (Collider2D c in colliders)
		{
			if (c.gameObject.layer == Alias.LAYER_ENEMIES)
			{
				c.gameObject.GetComponent<EnemyCollManager>().onHitByAttack(damage);
			}
			else if (c.gameObject.layer == Alias.LAYER_PC_SOLID)
			{
				if (OnHitPlayerShip != null)
				OnHitPlayerShip(damage);
			}
			else if (c.gameObject.layer == Alias.LAYER_ENEMY_PROJECTILES)
			{
				c.gameObject.GetComponent<BulletEnemy>().onHitByExplosion();
			}
			else if (c.gameObject.layer == Alias.LAYER_PC_PROJECTILES)
			{
				c.gameObject.GetComponent<BulletEnemy>().onHitByExplosion();
			}
		}
	}


	IEnumerator coLive()
	{
		yield return new WaitForSeconds(lifespan);

		Destroy(gameObject);
	}

}
