using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(MovingComponent))]
public class DynamicObjectComponent : MonoBehaviour
{
	[SerializeField] private float LookAheadTime = 3f;

	private bool IsLaunched = false;

	void LaunchDynamicObject()
	{
		float ExpectedTimeSpeedZero = ShipUnit.Instance.MovingComp.CurrentSpeed / ShipUnit.Instance.MovingComp.DecelerationValue;
		float FinalTime = Mathf.Min( ExpectedTimeSpeedZero, LookAheadTime );

		Vector3 ExpectedPosition = ShipUnit.Instance.MovingComp.transform.position + 
			ShipUnit.Instance.MovingComp.transform.forward * ShipUnit.Instance.MovingComp.CurrentSpeed * FinalTime -
			0.5f * ShipUnit.Instance.MovingComp.transform.forward * ShipUnit.Instance.MovingComp.DecelerationValue * FinalTime * FinalTime;
		float Distance = Vector3.Distance(transform.position, ExpectedPosition);

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
			LaunchDynamicObject();
			GetComponent<SphereCollider>().enabled = false;
		}
		else if (other.CompareTag("Planet"))
		{
			Destroy(gameObject);
		}
	}
}
