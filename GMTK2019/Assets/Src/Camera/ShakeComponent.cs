using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeComponent : MonoBehaviour
{
	[SerializeField] private bool IsSmoothShake = true;
	[SerializeField] private float SmoothAmount = 5f;

	private float RemainingShakeTime = 0f;
	private float RemainingShakeAmount = 0f;
	private float ShakePercentage = 0f;

	private float TotalShakeTime = 0f;
	private float TotalShakeAmount = 0f;

	private bool IsRunning = false;

	public void ShakeCamera( float Duration, float Amount )
	{
		RemainingShakeTime += Duration;
		TotalShakeTime = RemainingShakeTime;

		RemainingShakeAmount += Amount;
		TotalShakeAmount = RemainingShakeAmount;

		if (!IsRunning)
		{
			StartCoroutine(DoShake());
		}
	}

	private IEnumerator DoShake()
	{
		IsRunning = true;

		while ( RemainingShakeTime >= 0.05f )
		{
			Vector3 RotationAmount = Random.insideUnitSphere * RemainingShakeAmount;
			RotationAmount.z = 0f; // looks weird

			ShakePercentage = RemainingShakeTime / TotalShakeTime;

			RemainingShakeAmount = TotalShakeAmount * ShakePercentage;
			RemainingShakeTime = Mathf.Lerp(RemainingShakeTime, 0f, Time.deltaTime);

			if (IsSmoothShake)
			{
				transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(RotationAmount), Time.deltaTime * SmoothAmount);
			}
			else
			{
				transform.localRotation = Quaternion.Euler(RotationAmount); 
			}

			yield return null;
		}

		transform.localRotation = Quaternion.identity;
		IsRunning = false;
	}
}
