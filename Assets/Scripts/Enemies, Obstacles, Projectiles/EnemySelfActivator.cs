using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelfActivator : TriggeringObject
{
	[SerializeField]
	float activationDistance = 30f;
	Renderer myRenderer;
	Collider2D myCollider;
	Animator myAnimator;

	Transform pcTransform;


	void Start()
	{
		pcTransform = PCController.Me.transform;

		//myRenderer = GetComponent<Renderer>();
		//myCollider = GetComponent<Collider2D>();
		//myAnimator = GetComponent<Animator>();

		//myRenderer.enabled = false;
		//myCollider.enabled = false;
		//myCollider.attachedRigidbody.sleepMode = RigidbodySleepMode2D.StartAsleep;
		//myCollider.attachedRigidbody.Sleep();
		//myAnimator.enabled = false;
	}


	void Update()
	{
		Vector2 meToPc = transform.position - pcTransform.position;
		//if the PC is closer than the activation distance (Note: we compare squared distances so we avoid calculating a computing-heavy square root)
		if (meToPc.sqrMagnitude < activationDistance * activationDistance)
		{
			//myRenderer.enabled = true;
			//myCollider.enabled = true;
			//myCollider.attachedRigidbody.WakeUp();
			//myAnimator.enabled = true;

			triggerOthersAndFinish();
		}
	}
}
