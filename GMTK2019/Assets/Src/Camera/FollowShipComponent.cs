using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShipComponent : MonoBehaviour
{
	[SerializeField] public Transform ShipTransform = null;
	[SerializeField] public float DistanceToShip = 100f;

	[SerializeField] public bool SmoothFollow = true;
	[SerializeField] public float SmoothAmount = 1f;

	private void Awake()
	{
		if (ShipTransform)
		{
			transform.position = ShipTransform.position + Vector3.up * DistanceToShip;
		}
		else
		{
			Debug.LogError("No Ship set on " + this + "FollowShipComponent");
			Destroy(this);
		}
	}

	private void Update()
	{
		transform.position = Vector3.Lerp( transform.position, ShipTransform.position + Vector3.up * DistanceToShip, Time.deltaTime * SmoothAmount );
	}
}
