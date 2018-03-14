using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlippableObject : MonoBehaviour
{
	Transform[] children;
	float[] childrenDefaultAngles;

	bool isFlipped = false;


	void Awake()
	{
		List<Transform> list = new List<Transform>();

		foreach (Transform child in transform)
		{
			list.Add(child);
		}

		children = list.ToArray();

		childrenDefaultAngles = new float[children.Length];
		for (int i = 0; i < childrenDefaultAngles.Length; i++)
		{
			childrenDefaultAngles[i] = children[i].eulerAngles.z;
		}
	}


	public void flip()
	{
		if (!isFlipped)
		{
			transform.localScale = new Vector3(-1, 1, 1);

			for (int i = 0; i < children.Length; i++)
			{
				float a = 180f - childrenDefaultAngles[i];
				children[i].eulerAngles = new Vector3(0f, 0f, a);
			}

			isFlipped = true;
		}
	}


	public void unflip()
	{
		transform.localScale = Vector3.one;

		for (int i = 0; i < children.Length; i++)
		{
			children[i].eulerAngles = new Vector3(0f, 0f, childrenDefaultAngles[i]);
		}

		isFlipped = false;
	}

}
