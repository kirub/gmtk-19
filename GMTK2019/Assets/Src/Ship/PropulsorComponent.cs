using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovingComponent))]
public class PropulsorComponent : MonoBehaviour
{
	[SerializeField] private float MinPropulsionSpeed = 5f;
	[SerializeField] private float MaxPropulsionSpeed = 20f;
	[SerializeField] private float MaxPressedPropulsionTime = 2f;
	
	[SerializeField] private float SlowTime = 0.5f;
	[SerializeField] private float SlowTimeSpeed = 2f;

	[SerializeField] private bool UseCameraShake = true;
	[SerializeField] private float CameraShakeTime = 2f;
	[SerializeField] private float MinCameraShakeAmount = 2f;
	[SerializeField] private float MaxCameraShakeAmount = 5f;

	private float CurrentPressedPropulsionTime = 0f;
	private MovingComponent MovingComp = null;
	private ShakeComponent CameraShakeComp = null;

	private void Awake()
	{
		MovingComp = GetComponent<MovingComponent>();
	}

	private void Start()
	{
		CameraShakeComp = FindObjectOfType<ShakeComponent>();
	}

	private void Update()
	{
		if ( Input.GetKey( KeyCode.Space ) )
		{
			CurrentPressedPropulsionTime += Time.deltaTime;
			float newTimeScale = Time.timeScale - SlowTimeSpeed * Time.deltaTime;
			if (newTimeScale > SlowTime)
			{
				Time.timeScale = newTimeScale;
			}
			else
			{
				Time.timeScale = SlowTime;
			}
		}
		
		if ( Input.GetKeyUp( KeyCode.Space ) )
		{
			float CameraShakeAmount = MaxCameraShakeAmount;
			MovingComp.CurrentSpeed = MaxPropulsionSpeed;

			if (CurrentPressedPropulsionTime < MaxPressedPropulsionTime)
			{
				float PressedRatio = CurrentPressedPropulsionTime / MaxPressedPropulsionTime;
				CameraShakeAmount = MinCameraShakeAmount + (MaxCameraShakeAmount - MinCameraShakeAmount) * PressedRatio;
				MovingComp.CurrentSpeed = MinPropulsionSpeed + (MaxPropulsionSpeed - MinPropulsionSpeed) * PressedRatio;
			}
			CurrentPressedPropulsionTime = 0f;
			Time.timeScale = 1f;

			if (UseCameraShake && CameraShakeComp)
			{
				CameraShakeComp.ShakeCamera(CameraShakeTime, CameraShakeAmount);
			}
		}
	}
}
