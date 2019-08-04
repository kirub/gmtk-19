using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class DestroyOnCollisionComponent : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Planet"))
		{
			Destroy(transform.parent.gameObject);
		}
	}
}
