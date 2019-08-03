using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShipComponent : MonoBehaviour
{
	[SerializeField] public float DistanceToShip = 100f;

	[SerializeField] public bool SmoothFollow = true;
	[SerializeField] public float SmoothAmount = 1f;

	private void Start()
	{
		if (ShipUnit.Instance)
		{
			transform.position = ShipUnit.Instance.transform.position + Vector3.up * DistanceToShip;
		}
		else
		{
			Debug.LogWarning("No Ship Instance found in " + this + "FollowShipComponent");
			Destroy(this);
		}
	}

	private void Update()
	{
		if (ShipUnit.Instance)
		{
			Vector3 FinalPosition = new Vector3(ShipUnit.Instance.transform.position.x, transform.position.y, ShipUnit.Instance.transform.position.z);
			transform.position = Vector3.Lerp( transform.position, FinalPosition, Time.deltaTime * SmoothAmount );
		}
		else
		{
			Destroy(this);
		}
	}
}
