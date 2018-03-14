﻿using UnityEngine;
using System.Collections;

public class EnemyCollManager : MonoBehaviour
{
	[SerializeField]
	EnemyInfo info;
	[SerializeField]
	EnemyHealthManager healthManager;
	[SerializeField]
	bool isMapDamaging;


	//Called by BulletPC when it hits that enemy
	public void onHitByAttack(float dmg)
	{
		Debug.Log(name + " screams: PC attacked me!");//TEST
		healthManager.gotHitByPCAttack(dmg);
	}


	//Called by PC when it touches that enemy
	public void onCollidedByPC()
	{
		Debug.Log(name + " says: PC collides with me");//TEST
		healthManager.touchPC();
	}


	void OnCollisionStay2D(Collision2D otherCol)
	{
		//check if collide with map
		if (isMapDamaging && otherCol.gameObject.layer == Alias.LAYER_TILEMAP)
			healthManager.gotMapHit();
	}
}
