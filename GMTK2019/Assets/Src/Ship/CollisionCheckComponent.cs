using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheckComponent : MonoBehaviour
{	
	private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OrbitalInnerRadius") || other.CompareTag("OrbitalOuterRadius"))
            return;

        if (!ShipUnit.Instance)
		{
			Debug.LogWarning("No Ship Instance found, will not be able to die");
			return;
		}

		ShipUnit.Instance.Explode();
	}
}
