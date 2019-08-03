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

	public enum EShakeType
	{
		Rotate,
		Translate
	}

	private void InternalSetupShake(float Duration, float Amount)
	{
		RemainingShakeTime += Duration;
		TotalShakeTime = RemainingShakeTime;

		RemainingShakeAmount += Amount;
		TotalShakeAmount = RemainingShakeAmount;
	}

	public void ShakeCamera(float Duration, float Amount, EShakeType ShakeType = EShakeType.Rotate)
	{
		InternalSetupShake(Duration, Amount);

		if (!IsRunning)
		{
			switch (ShakeType)
			{
				case EShakeType.Rotate:
					StartCoroutine(DoRotateShake());
					break;
				case EShakeType.Translate:
					StartCoroutine(DoTranslateShake());
					break;
			}
		}
	}

	private IEnumerator DoTranslateShake()
	{
		Vector3 oldPosition = transform.localPosition;
		IsRunning = true;

		while ( RemainingShakeTime >= 0.05f )
		{
			float XAmount = Random.Range(-1f, 1f) * RemainingShakeAmount;
			float YAmount = Random.Range(-1f, 1f) * RemainingShakeAmount;
			Vector3 FinalPosition = oldPosition + transform.right * XAmount + transform.up * YAmount;

			ShakePercentage = RemainingShakeTime / TotalShakeTime;

			RemainingShakeAmount = TotalShakeAmount * ShakePercentage;
			RemainingShakeTime = Mathf.Lerp(RemainingShakeTime, 0f, Time.deltaTime);

			if (IsSmoothShake)
			{
				transform.localPosition = Vector3.Lerp(transform.localPosition, FinalPosition, Time.deltaTime * SmoothAmount);
			}
			else
			{
				transform.localPosition = FinalPosition; 
			}

			yield return null;
		}

		transform.localPosition = oldPosition;
		IsRunning = false;
	}

	private IEnumerator DoRotateShake()
	{
		IsRunning = true;

		while (RemainingShakeTime >= 0.05f)
		{
			Vector3 RotationAmount = Random.insideUnitSphere * RemainingShakeAmount;
			RotationAmount.z = 0f; // looks weird

			ShakePercentage = RemainingShakeTime / TotalShakeTime;

			RemainingShakeAmount = TotalShakeAmount * ShakePercentage;
			RemainingShakeTime = Mathf.Lerp(RemainingShakeTime, 0f, Time.deltaTime);

			if (IsSmoothShake)
			{
				transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(RotationAmount), Time.deltaTime * SmoothAmount);
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
