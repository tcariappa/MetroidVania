using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricLine : MonoBehaviour
{
    public static event System.Action<float> OnElectric;
    [SerializeField]
    private float damage = 300.0f;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == Alias.LAYER_PC_TRIGGER)
        {
            if (OnElectric != null)
                OnElectric(damage);
        }
        else if(coll.gameObject.layer != (Alias.LAYER_PC_TRIGGER | Alias.LAYER_PC_SOLID))
            Destroy(coll.gameObject);
    }
}
	
	
