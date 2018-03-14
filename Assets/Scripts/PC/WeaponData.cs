using UnityEngine;
using System.Collections;


[System.Serializable]
public class WeaponData
{
	public WeaponType type;
	public float[] damageAtLvl = { 5f, 5f, 5f, 6f, 6f };
	public float[] speedAtLvl = { 5f, 5f, 5f, 6f, 6f };
	public float[] fireDelayAtLvl = { 0.15f, 0.15f, 0.12f, 0.12f, 0.1f };
	public BulletPC bulletRef;
	//public Laser laserRef;


	public float getSpeed(int lvl)
	{
		if (lvl >= speedAtLvl.Length)
			lvl = speedAtLvl.Length - 1;

		return speedAtLvl[lvl];
	}


	public float getDamage(int lvl)
	{
		if (lvl >= damageAtLvl.Length)
			lvl = damageAtLvl.Length - 1;

		return damageAtLvl[lvl];
	}


	public float getFireDelay(int lvl)
	{
		if (lvl >= fireDelayAtLvl.Length)
			lvl = fireDelayAtLvl.Length - 1;

		return fireDelayAtLvl[lvl];
	}
}


public enum WeaponType
{
	ProjectileA,
	ProjectileB
}

