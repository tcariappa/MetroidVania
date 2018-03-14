using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PCShootManager : MonoBehaviour
{
	[SerializeField]
	WeaponSpawnSystem[] weaponSpawnSystems;
	[SerializeField]
	WeaponData[] weapons;

	//static public event System.Action OnStopLaser;

	int currWpnIdx;
	int currWeaponLvl;
	bool isFiringBullets;
	//bool isFiringLaser;
	float lastFireTime = -1f;


	// Use this for initialization
	void Awake()
	{
		isFiringBullets = false;
		//isFiringLaser = false;
		lastFireTime = -99f;
		currWpnIdx = 0;
		currWeaponLvl = 0;
	}


	void OnEnable()
	{
		GameManager.OnPlayStart += handleOnPlayStart;
		PCHealthManager.OnDeath += handleOnPCDeath;
		//PCInputsManager.OnPressFire1 += handleOnPressFire1;
        PCInputsManager.OnReleaseFire1 += handleOnReleaseFire1;
		//PCInputsManager.OnReleaseFire2 += handleOnReleaseFire2;
	}


	void OnDisable()
	{
		GameManager.OnPlayStart -= handleOnPlayStart;
		PCHealthManager.OnDeath -= handleOnPCDeath;
		//PCInputsManager.OnPressFire1 -= handleOnPressFire1;
		//PCInputsManager.OnPressFire2 -= handleOnPressFire2;
	}


	private void handleOnPlayStart()
	{
	}


	private void handleOnPCDeath()
	{
		//mayStopLaser();
	}


	private void handleOnPressFire1()
	{
		switch (weapons[currWpnIdx].type)
		{
			case WeaponType.ProjectileA:

				if (UpgradesManager.List["fireball 1"] == true)
				{
					if (!isFiringBullets)
						startFiringBullets();
				}
				break;

			case WeaponType.ProjectileB:
				if (!isFiringBullets)
					startFiringBullets();
				break;
		}
	}

	private void handleOnReleaseFire1()
	{
		//mayStopLaser();
		isFiringBullets = false;
	}

	void startFiringBullets()
	{
		//isFiringLaser = false;
		isFiringBullets = true;
		StartCoroutine(coFireBullets());
	}


	IEnumerator coFireBullets()
	{
		do
		{
			if (Time.time >= lastFireTime + weapons[currWpnIdx].getFireDelay(currWeaponLvl))
			{
				//fire bullets from all weapon's spawn points
				Transform[] spawnPts = weaponSpawnSystems[currWpnIdx].getSpawnPoints(currWeaponLvl);
				foreach (Transform pt in spawnPts)
				{
					BulletPC bullet = Instantiate<BulletPC>(weapons[currWpnIdx].bulletRef);
					bullet.init(pt.position, pt.eulerAngles, weapons[currWpnIdx].getSpeed(currWeaponLvl), weapons[currWpnIdx].getDamage(currWeaponLvl));
				}
				//update next fire time
				lastFireTime = Time.time;
			}
			yield return null;
		} while (isFiringBullets);
	}


	//void startLaser()
	//{
	//	isFiringBullets = false;
	//	isFiringLaser = true;

	//	Transform[] spawnPts = weaponSpawnSystems[currWpnIdx].getSpawnPoints(currWeaponLvl);
	//	foreach (Transform pt in spawnPts)
	//	{
	//		//Laser laser = Instantiate<ShipLaser>(weapons[currWpnIdx].laserRef);
	//		//laser.init(pt, weapons[currWpnIdx].getDamage(currWeaponLvl));
	//	}
	//}


	//void mayStopLaser()
	//{
	//	if (isFiringLaser)
	//	{
	//		isFiringLaser = false;
	//		if (OnStopLaser != null)
	//		{
	//			OnStopLaser();
	//		}
	//	}
	//}


	int getWeaponIndex(WeaponType weapon)
	{
		for (int i = 0; i < weapons.Length; i++)
		{
			if (weapons[i].type == weapon)
			{
				return i;
			}
		}

		return -1; //normally never executed (but required by the compiler)
	}


	void increaseLvl()
	{
		currWeaponLvl += 1;
	}

}
