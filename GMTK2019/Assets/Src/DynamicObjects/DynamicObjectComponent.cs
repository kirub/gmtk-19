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

	void LaunchDynamicObject()
	{
		IsLaunched = true;

		float ExpectedTimeSpeedZero = ShipUnit.Instance.MovingComp.CurrentSpeed / ShipUnit.Instance.MovingComp.DecelerationValue;
		float FinalTime = Mathf.Min(ExpectedTimeSpeedZero, LookAheadTime);

		Vector3 ExpectedPosition = ShipUnit.Instance.MovingComp.transform.position +
			ShipUnit.Instance.MovingComp.transform.forward * ShipUnit.Instance.MovingComp.CurrentSpeed * FinalTime -
			0.5f * ShipUnit.Instance.MovingComp.transform.forward * ShipUnit.Instance.MovingComp.DecelerationValue * FinalTime * FinalTime;
		float Distance = Vector3.Distance(transform.position, ExpectedPosition);

		if (RandomRange > 0f)
		{
			FinalTime += Random.Range(-RandomRange, RandomRange);
			FinalTime = Mathf.Max(MinTime, FinalTime);
		}

		float SpeedNeeded = Distance / FinalTime;
		Vector3 ForwardNeeded = (ExpectedPosition - transform.position).normalized;

		GetComponent<MovingComponent>().CurrentSpeed = SpeedNeeded;
		transform.rotation = Quaternion.LookRotation(ForwardNeeded, Vector3.up);
	}

	private void Start()
	{
		if (!ShipUnit.Instance)
		{
			Debug.LogError("Can't find a ShipUnit");
			Destroy(this);
			return;
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
