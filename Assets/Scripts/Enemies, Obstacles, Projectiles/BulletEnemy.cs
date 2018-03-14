using UnityEngine;
using System.Collections;

public class BulletEnemy : BulletBase
{
	public float damage = 5f;
	[SerializeField]
	Enemies type;


	public void init(Vector3 pos, float rot)
	{
		rb.position = transform.position = pos;
		rb.rotation = rot;
		transform.eulerAngles = new Vector3(0f, 0f, rot);
	}


	public void onHitPC()
	{
		destroyMyself();
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		//detect collision of enemy's bullet with the map
		if (col.gameObject.layer == Alias.LAYER_TILEMAP)
		{
			destroyMyself();
		}
		//check collision of enemy's bullet with PC
		//else if (col.gameObject.layer == Alias.LAYER_PC_SOLID)
		//{
		//	col.GetComponent<PCCollMapManager>().onHitByEnemyObject(type, damage);

		//	destroyMyself();
		//}
	}

}
