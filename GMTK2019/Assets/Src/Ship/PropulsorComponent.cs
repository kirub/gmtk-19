﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovingComponent))]
[RequireComponent(typeof(RotatorComponent))]
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
	
	private List<GameObject> NearComets = new List<GameObject>();

	private float CurrentPressedPropulsionTime = 0f;
	private MovingComponent MovingComp = null;
	private RotatorComponent RotatorComp = null;
	private ShakeComponent CameraShakeComp = null;

	public bool CanPropulse { get { return NearComets.Count > 0; } }

	private void Awake()
	{
		MovingComp = GetComponent<MovingComponent>();
		RotatorComp = GetComponent<RotatorComponent>();
	}

	private void Start()
	{
		CameraShakeComp = FindObjectOfType<ShakeComponent>();
	}

	private void Update()
	{
		if ( Input.GetKey( KeyCode.Space ) )
		{
			if (CanPropulse)
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
		}
		
		if ( Input.GetKeyUp( KeyCode.Space ) )
		{
			if (CanPropulse)
			{
				float CameraShakeAmount = MaxCameraShakeAmount;
				MovingComp.CurrentSpeed = MaxPropulsionSpeed;

				if (CurrentPressedPropulsionTime < MaxPressedPropulsionTime)
				{
					float PressedRatio = CurrentPressedPropulsionTime / MaxPressedPropulsionTime;
					CameraShakeAmount = MinCameraShakeAmount + (MaxCameraShakeAmount - MinCameraShakeAmount) * PressedRatio;
					MovingComp.CurrentSpeed = MinPropulsionSpeed + (MaxPropulsionSpeed - MinPropulsionSpeed) * PressedRatio;
				}

				if (RotatorComp.MeshRotated)
				{
					transform.rotation = RotatorComp.MeshRotated.rotation;
					RotatorComp.MeshRotated.localRotation = Quaternion.identity;
				}

				if (UseCameraShake && CameraShakeComp)
				{
					CameraShakeComp.ShakeCamera(CameraShakeTime, CameraShakeAmount);
				}
				
				GameObject NearestComet = NearComets[0];
				NearComets.RemoveAt(0);

				Destroy(NearestComet);
			}

			CurrentPressedPropulsionTime = 0f;
			Time.timeScale = 1f;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Comet"))
		{
			NearComets.Add(other.gameObject);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Comet"))
		{
			NearComets.Remove(other.gameObject);
		}
	}
}
