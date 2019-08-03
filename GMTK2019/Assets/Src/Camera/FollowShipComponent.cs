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
			Debug.LogError("No Ship Instance found in " + this + "FollowShipComponent");
			Destroy(this);
		}
	}

	private void Update()
	{
		if (ShipUnit.Instance.transform)
		{
			transform.position = Vector3.Lerp( transform.position, ShipUnit.Instance.transform.position + Vector3.up * DistanceToShip, Time.deltaTime * SmoothAmount );
		}
		else
		{
			Destroy(this);
		}
	}
}
