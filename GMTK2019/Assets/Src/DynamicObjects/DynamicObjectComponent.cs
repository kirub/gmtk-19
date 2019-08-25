using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(MovingComponent))]
public class DynamicObjectComponent : MonoBehaviour
{
	public enum ETargetType
	{
		HomingRandom,
		HomingFixed,
		FollowingForward
	}

	[SerializeField] private ETargetType TargetType = ETargetType.HomingRandom;
	[SerializeField] private float HomingValue = 2f;
	[SerializeField] private float HomingLookAheadTime = 3f;
	[SerializeField] private float HomingLookAheadMinTime = 0.5f;


	private bool IsLaunched = false;
	private GameObject MeshContainer = null;
	private MovingComponent MovingComp = null;

	void LaunchDynamicObject()
	{
		IsLaunched = true;

		if (MeshContainer)
		{
			MeshContainer.SetActive(true);
		}

		if (TargetType == ETargetType.FollowingForward)
		{
			MovingComp.CurrentSpeed = MovingComp.MaxMovingSpeedValue;
		}
		else
		{
			float ExpectedTimeSpeedZero = ShipUnit.Instance.MovingComp.CurrentSpeed / ShipUnit.Instance.MovingComp.DecelerationValue;
			float ExpectedTime = Mathf.Min(ExpectedTimeSpeedZero, HomingLookAheadTime);

			Vector3 ExpectedPosition = ShipUnit.Instance.MovingComp.transform.position +
				ShipUnit.Instance.MovingComp.transform.forward * ShipUnit.Instance.MovingComp.CurrentSpeed * ExpectedTime -
				0.5f * ShipUnit.Instance.MovingComp.transform.forward * ShipUnit.Instance.MovingComp.DecelerationValue * ExpectedTime * ExpectedTime;
			float Distance = Vector3.Distance(transform.position, ExpectedPosition);

			float FinalTime = ExpectedTime;
			if (HomingValue > 0f)
			{
				if (TargetType == ETargetType.HomingRandom)
				{
					FinalTime += Random.Range(-HomingValue, HomingValue);
				}
				else
				{
					FinalTime += HomingValue;
				}
				FinalTime = Mathf.Max(HomingLookAheadMinTime, FinalTime);
			}

			float ExpectedSpeed = Distance / ExpectedTime;
			float FinalSpeed = Distance / FinalTime;
			Vector3 ExpectedForward = (ExpectedPosition - transform.position).normalized;

			if (MovingComp.MaxMovingSpeedValue > 0f && ExpectedSpeed > MovingComp.MaxMovingSpeedValue)
			{
				ExpectedSpeed = MovingComp.MaxMovingSpeedValue;
				transform.position = ExpectedPosition - ExpectedForward * ExpectedSpeed * FinalTime;
			}

			GetComponent<MovingComponent>().CurrentSpeed = FinalSpeed;
			transform.rotation = Quaternion.LookRotation(ExpectedForward, Vector3.up);
		}
	}

	private void Awake()
	{
		MovingComp = GetComponent<MovingComponent>();
	}

	private void Start()
	{
		if (!ShipUnit.Instance)
		{
			Debug.LogError("Can't find a ShipUnit");
			Destroy(this);
			return;
		}

		MeshContainer = null;
		int i = 0;
		while (i < transform.childCount && MeshContainer == null)
		{
			Transform CurrentChild = transform.GetChild(i);
			if (CurrentChild.GetComponentInChildren<MeshRenderer>())
			{
				MeshContainer = CurrentChild.gameObject;
				MeshContainer.SetActive(false);
			}
			++i;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (IsLaunched)
		{
			return;
		}

		if (other.CompareTag("Player"))
		{
			GetComponent<SphereCollider>().enabled = false;
			LaunchDynamicObject();
		}
	}
}
