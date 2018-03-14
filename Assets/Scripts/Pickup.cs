using UnityEngine;
using System.Collections;
using System;

public class Pickup : MonoBehaviour
{
	[SerializeField]
	Rigidbody2D rb;
	[SerializeField]
	Animator animator;
	public Pickups type;
	[SerializeField]
	GameObject objToSpawn;
	static public event System.Action<Pickup> OnPickup;

	bool isPicked;


	void Awake()
	{
		isPicked = false;
		enabled = false;
	}


	void OnBecameVisible()
	{
		enabled = true;
	}


	void Start()
	{
		if (type == Pickups.Upgrade)
		{
			if (GetComponent<UpgradeObject>().isUnlocked)
			{
				Destroy(gameObject);
			}
		}

		updateVisual();
	}


	void OnTriggerEnter2D(Collider2D otherColl)
	{
		if (otherColl.gameObject.layer == Alias.LAYER_PC_SOLID)
		{
            gotPicked();

            if (type == Pickups.Keycard)
            {
                otherColl.gameObject.GetComponent<PCController>().keycardCollected();
            }
		}
	}


	void gotPicked()
	{
		isPicked = true;

		if (OnPickup != null)
			OnPickup(this);

		if (objToSpawn != null)
		{
			Instantiate(objToSpawn, transform.position, Quaternion.identity);
		}

		Destroy(gameObject);
	}


	void updateVisual()
	{
		if (animator != null)
			animator.SetInteger("Type", (int)type);
	}
}


public enum Pickups
{
	HP = 0,
	Mana = 1,
	Upgrade = 10,
    Keycard = 11
}
