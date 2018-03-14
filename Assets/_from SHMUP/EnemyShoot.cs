using UnityEngine;
using System.Collections;

public class EnemyShoot : TriggerableObject
{
	[SerializeField]
	private Animator animatorIfFireAnim;
	[SerializeField]
	private BulletEnemy projectileRef;
	[SerializeField]
	private float delayBtwFire = 1f;
	public float shootingAngle = 180f;


	void OnEnable()
	{
		StartCoroutine(coShooting());
	}


	IEnumerator coShooting()
	{
		while (true)
		{
			//We start the enemy animation to prepare shooting,
			//but it'll be an animation event that will actually trigger the shoot by calling actuallyFire()
			if (animatorIfFireAnim != null)
			{
				animatorIfFireAnim.SetTrigger("goShoot");
			}
			//If no animator is specified, we shoot directly
			else
			{
				actuallyFire();
			}

			yield return new WaitForSeconds(delayBtwFire);
		}
	}


	//called by animation event when the visual is ready to fire
	private void actuallyFire()
	{
		BulletEnemy bullet = Instantiate<BulletEnemy>(projectileRef);
		float angle = transform.eulerAngles.z + shootingAngle;
		bullet.init(transform.position, angle);
	}
}
