using UnityEngine;
using System.Collections;
using System;

public class SpawnProp : MonoBehaviour
{
	public EnemyHealthManager healthManager;
	public Animator animator;

	public event System.Action OnOpened;


	public void open()
	{
		animator.SetTrigger("goOpen");
	}


	public void onDoneOpening()
	{
		if (OnOpened != null)
			OnOpened();
	}
}
