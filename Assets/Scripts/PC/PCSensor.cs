using UnityEngine;
using System.Collections;

public class PCSensor : MonoBehaviour
{
	public bool isCollTile { get; private set; }
	public SlopeType slopeColl { get; private set; }


	// Use this for initialization
	void Awake()
	{
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		checkCollision();
	}


	void checkCollision()
	{
		Collider2D c = Physics2D.OverlapPoint(transform.position, Alias.LAYERMASK_TILEMAP | Alias.LAYERMASK_BREAKABLE_SURFACE);

		if (c != null)
		{
			if (c.gameObject.layer == Alias.LAYER_TILEMAP || c.gameObject.layer == Alias.LAYER_BREAKABLE_SURFACE)
			{
				isCollTile = true;

				//check for slope
				switch (c.tag)
				{
					case "TileMapSoftSlopeL":
						slopeColl = SlopeType.softLeft;
						break;
					case "TileMapSoftSlopeR":
						slopeColl = SlopeType.softRight;
						break;
					case "TileMapHardSlopeL":
						slopeColl = SlopeType.hardLeft;
						break;
					case "TileMapHardSlopeR":
						slopeColl = SlopeType.hardRight;
						break;
					default:
						slopeColl = SlopeType.none;
						break;
                }
			}
			else
			{
				isCollTile = false;
				slopeColl = SlopeType.none;
			}
		}
		else
		{
			isCollTile = false;
			slopeColl = SlopeType.none;
		}
	}

}


public enum SlopeType
{
	none,
	softLeft,
	softRight,
	hardLeft,
	hardRight
}

