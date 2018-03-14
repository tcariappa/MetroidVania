using UnityEngine;
using System.Collections;


public class PCSfxManager : MonoBehaviour
{
	public AudioClip footstepsClip;
	[Range(0f, 1f)]
	public float footstepsVol = 0.9f;
	[Range(0f, 1f)]
	public float footstepsPitchDelta = 0.2f;

	public AudioClip jumpClip;
	[Range(0f, 1f)]
	public float jumpVol = 0.9f;

	public AudioClip deathAClip;
	[Range(0f, 1f)]
	public float deathAVol = 0.9f;

	AudioSource audioSource;


	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}


	void playFootstep()
	{
		audioSource.pitch = Random.Range(1f - footstepsPitchDelta, 1f + footstepsPitchDelta);
		audioSource.PlayOneShot(footstepsClip, footstepsVol);
	}


	void playJump()
	{
		audioSource.pitch = 1f;
		audioSource.PlayOneShot(jumpClip, jumpVol);
	}


	void playDeathA()
	{
		audioSource.pitch = 1f;
		audioSource.PlayOneShot(deathAClip, deathAVol);
	}

}
