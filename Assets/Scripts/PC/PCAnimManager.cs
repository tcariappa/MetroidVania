using UnityEngine;
using System.Collections;

public class PCAnimManager : MonoBehaviour
{

	public Animator animator;
	public PCController pc;
   

	// Update is called once per frame
	void Update()
	{
        switch (pc.currState)
		{
			case PCController.State.idle:
				animator.SetInteger("state", 0);
				break;
			case PCController.State.blocked:
				animator.SetInteger("state", 1);
				break;
			case PCController.State.running:          
                animator.SetInteger("state", 2);
                break;
            case PCController.State.regJumping:
				animator.SetInteger("state", 10);
				break;
			case PCController.State.wallJumping:
				animator.SetInteger("state", 11);
				break;
			case PCController.State.falling:
				animator.SetInteger("state", 12);
				break;
			case PCController.State.clingingToWall:
				animator.SetInteger("state", 21);
				break;
			case PCController.State.grabbingLedge:
				animator.SetInteger("state", 24);
				break;
			case PCController.State.hangingLedge:
				animator.SetInteger("state", 24);
				break;
			case PCController.State.climbingLedge:
				animator.SetInteger("state", 25);
				break;
			case PCController.State.dying:
				animator.SetInteger("state", 30);
				break;
            case PCController.State.dashing:
                animator.SetInteger("state", 5);
                break;

            //Unibike State Animations
            case PCController.State.unibikeMove:
                animator.SetInteger("state", 4);
                break;
            case PCController.State.unibikeDashing:
                animator.SetInteger("state", 31);
                break;
            case PCController.State.bounceFall:
                animator.SetInteger("state", 34);
                break;
            case PCController.State.bounceJump:
                animator.SetInteger("state", 33);
                break;
        }
	}

}
