using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheckComponent : MonoBehaviour
{
    [SerializeField]
    List<string> CollisionsExceptions = new List<string>();

    private List<string> CollisionTagExceptionsList { get { return CollisionsExceptions; } }

	private void OnTriggerEnter(Collider other)
    {
        if (CollisionTagExceptionsList.Contains(other.tag))
            return;

        if (!ShipUnit.Instance)
		{
			Debug.LogWarning("No Ship Instance found, will not be able to die");
			return;
		}

		ShipUnit.Instance.Explode();
	}
}
