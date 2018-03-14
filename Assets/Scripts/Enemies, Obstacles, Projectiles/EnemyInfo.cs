using UnityEngine;
using System.Collections;


public class EnemyInfo : MonoBehaviour
{
	public Enemies type;
	public bool isFlying;
	public float damageOnContact = 10f;
}


public enum Enemies
{
	EnemyA,
	EnemyB,
	EnemyC,
	EnemyD,
	EnemyE,
	EnemyF,
	ProjectileA,
	ProjectileB,
	ProjectileC,
	ProjectileD,
	SpawnSiloA,
    SpawnSiloB
}
