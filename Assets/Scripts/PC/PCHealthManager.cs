using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class PCHealthManager : MonoBehaviour
{
	static public PCHealthManager Me;

	public float MaxHitPoints = 100f;
	[SerializeField]
	private float hitDuration = 0.5f;

	public float hp { get; private set; }
	public bool isHit { get; private set; }

	static public event System.Action<float, float> OnDamaged;
	static public event System.Action OnDeath;

	void Awake()
	{
		Me = this;

		//if (SceneManager.GetActiveScene().name == SvgManager.SvgData.currentSceneName)
		//{
		//	hp = SvgManager.SvgData.currHPs;

		//	//DEBUG
		//	if (hp > maxHitPoints || hp <= 0f)
		//	{
		//		Debug.LogWarning("Warning, remaining HP = " + hp + " and max HP = " + maxHitPoints);
		//		hp = maxHitPoints;
		//	}
		//}
		//else
		//{
		//	hp = maxHitPoints;
		//}

		hp = MaxHitPoints;
	}


	void OnEnable()
	{
		PCCollTriggerManager.OnHit += handleOnHit;
        ProximityMine.OnMine += handleOnHit;
        ElectricLine.OnElectric += handleOnHit;
	}


	void OnDisable()
	{
		PCCollTriggerManager.OnHit -= handleOnHit;
        ProximityMine.OnMine -= handleOnHit;
        ElectricLine.OnElectric -= handleOnHit;
    }


	void handleOnHit(float dmg)
	{
		if (!isHit)
		{
			takeDamage(dmg);
			StartCoroutine(coHitCooldown());
		}
	}


	void takeDamage(float dmg)
	{
		hp -= dmg;

		if (OnDamaged != null)
			OnDamaged(dmg, hp);

		if (hp <= 0)
		{
			if (OnDeath != null)
				OnDeath();

			//We unregister from the event OnHit so that we don't send the event OnDeath more than once
			PCCollTriggerManager.OnHit -= handleOnHit;
            ProximityMine.OnMine -= handleOnHit;
            ElectricLine.OnElectric -= handleOnHit;
        }
	}


	IEnumerator coHitCooldown()
	{
		isHit = true;

		float endTime = Time.time + hitDuration;

		do
		{
			yield return null;
		} while (Time.time < endTime);

		isHit = false;
	}

}
