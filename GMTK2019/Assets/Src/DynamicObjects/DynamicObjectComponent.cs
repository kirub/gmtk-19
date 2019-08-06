using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(MovingComponent))]
public class DynamicObjectComponent : MonoBehaviour
{
	[SerializeField] private float LookAheadTime = 3f;
	[SerializeField] private float RandomRange = 2f;
	[SerializeField] private float MinTime = 0.5f;

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

		float ExpectedTimeSpeedZero = ShipUnit.Instance.MovingComp.CurrentSpeed / ShipUnit.Instance.MovingComp.DecelerationValue;
		float ExpectedTime = Mathf.Min(ExpectedTimeSpeedZero, LookAheadTime);

		Vector3 ExpectedPosition = ShipUnit.Instance.MovingComp.transform.position +
			ShipUnit.Instance.MovingComp.transform.forward * ShipUnit.Instance.MovingComp.CurrentSpeed * ExpectedTime -
			0.5f * ShipUnit.Instance.MovingComp.transform.forward * ShipUnit.Instance.MovingComp.DecelerationValue * ExpectedTime * ExpectedTime;
		float Distance = Vector3.Distance(transform.position, ExpectedPosition);

		float FinalTime = ExpectedTime;
		if (RandomRange > 0f)
		{
			FinalTime += Random.Range(-RandomRange, RandomRange);
			FinalTime = Mathf.Max(MinTime, FinalTime);
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
