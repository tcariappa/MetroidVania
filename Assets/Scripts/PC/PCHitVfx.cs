using UnityEngine;

/// <summary>
/// Provides visual feedback on the PC after it has been hit to communicate temporary invincibility.
/// Note: this behavior relies entirely on PCHealthManager as it matches its hit cooldown.
/// </summary>
public class PCHitVfx : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer spriteRenderer;


	private void Update()
	{
		if (!PCHealthManager.Me.isHit)
		{
			if (!spriteRenderer.enabled)
				spriteRenderer.enabled = true;
		}
		//blinks every frame
		else
		{
			spriteRenderer.enabled = !spriteRenderer.enabled;
		}
	}

}
