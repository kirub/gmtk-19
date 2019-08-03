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

	private void InternalSetupShake(float Duration, float Amount)
	{
		RemainingShakeTime += Duration;
		TotalShakeTime = RemainingShakeTime;

		RemainingShakeAmount += Amount;
		TotalShakeAmount = RemainingShakeAmount;
	}

	public void ShakeCamera(float Duration, float Amount)
	{
		InternalSetupShake(Duration, Amount);

		if (!IsRunning)
		{
			StartCoroutine(DoShake());
		}
	}

	public void ContinuousShakeCamera(float Amount)
	{
		TotalShakeTime = RemainingShakeTime = -1f;
		TotalShakeAmount = RemainingShakeAmount = Amount;

		if (!enabled)
		{
			enabled = true;
		}
	}

	public void StopContinuousShakeCamera()
	{
		if (!enabled)
		{
			return;
		}

		enabled = false;
	}

	void RotateShake(float Amount)
	{
		Vector3 RotationAmount = Random.insideUnitSphere * Amount;
		RotationAmount.z = 0f; // looks weird

		if (IsSmoothShake)
		{
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(RotationAmount), Time.unscaledDeltaTime * SmoothAmount);
		}
		else
		{
			transform.localRotation = Quaternion.Euler(RotationAmount);
		}
	}

	private IEnumerator DoShake()
	{
		IsRunning = true;

		while (RemainingShakeTime >= 0.05f)
		{
			RotateShake(RemainingShakeAmount);

			ShakePercentage = RemainingShakeTime / TotalShakeTime;

			RemainingShakeAmount = TotalShakeAmount * ShakePercentage;
			RemainingShakeTime = Mathf.Lerp(RemainingShakeTime, 0f, Time.unscaledDeltaTime);

			yield return null;
		}

		transform.localRotation = Quaternion.identity;
		IsRunning = false;
	}

	private void Start()
	{
		enabled = false;
	}

	private void Update()
	{
		RotateShake(TotalShakeAmount);
	}
}
