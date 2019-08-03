using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheckComponent : MonoBehaviour
{
	[SerializeField] private bool UseCameraShake = true;
	[SerializeField] private float CameraShakeTime = 2f;
	[SerializeField] private float CameraShakeAmount = 2f;

	private ShakeComponent CameraShakeComp = null;

	private void Start()
	{
		CameraShakeComp = FindObjectOfType<ShakeComponent>();
	}
	private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("OrbitalInnerRadius") || other.CompareTag("OrbitalOuterRadius"))
            return;

		if (UseCameraShake && CameraShakeComp)
		{
			CameraShakeComp.ShakeCamera(CameraShakeTime, CameraShakeAmount);
		}
		Destroy(transform.parent.gameObject);
	}
}
