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
	private EShakeType CurrentShakeType = EShakeType.Rotate;
	private Vector3 StartPosition = Vector3.zero;

	private void InternalSetupShake(float Duration, float Amount, EShakeType ShakeType)
	{
		RemainingShakeTime += Duration;
		TotalShakeTime = RemainingShakeTime;

		RemainingShakeAmount += Amount;
		TotalShakeAmount = RemainingShakeAmount;

		CurrentShakeType = ShakeType;
	}

	public void ShakeCamera(float Duration, float Amount, EShakeType ShakeType = EShakeType.Rotate)
	{
		InternalSetupShake(Duration, Amount, ShakeType);

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

	public void ContinuousShakeCamera(float Amount, EShakeType ShakeType = EShakeType.Rotate)
	{
		TotalShakeTime = RemainingShakeTime = -1f;
		TotalShakeAmount = RemainingShakeAmount = Amount;
		CurrentShakeType = ShakeType;

		if (!enabled)
		{
			StartPosition = transform.localPosition;
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
		transform.localPosition = StartPosition;
	}

	void TranslateShake( Vector3 OldPosition, float Amount )
	{
		float XAmount = Random.Range(-1f, 1f) * Amount;
		float YAmount = Random.Range(-1f, 1f) * Amount;
		Vector3 FinalPosition = OldPosition + transform.right * XAmount + transform.up * YAmount;

		if (IsSmoothShake)
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, FinalPosition, Time.unscaledDeltaTime * SmoothAmount);
		}
		else
		{
			transform.localPosition = FinalPosition;
		}
	}

	private IEnumerator DoTranslateShake()
	{
		Vector3 OldPosition = transform.localPosition;
		IsRunning = true;

		while ( RemainingShakeTime >= 0.05f )
		{
			TranslateShake(OldPosition, RemainingShakeAmount);

			ShakePercentage = RemainingShakeTime / TotalShakeTime;

			RemainingShakeAmount = TotalShakeAmount * ShakePercentage;
			RemainingShakeTime = Mathf.Lerp(RemainingShakeTime, 0f, Time.unscaledDeltaTime);

			yield return null;
		}

		transform.localPosition = OldPosition;
		IsRunning = false;
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

	private IEnumerator DoRotateShake()
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
		switch (CurrentShakeType)
		{
			case EShakeType.Rotate:
				TranslateShake(StartPosition, TotalShakeAmount);
				break;
			case EShakeType.Translate:
				RotateShake(TotalShakeAmount);
				break;
		}
	}
}
