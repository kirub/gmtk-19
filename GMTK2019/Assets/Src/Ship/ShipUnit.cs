using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovingComponent))]
[RequireComponent(typeof(RotatorComponent))]
[RequireComponent(typeof(PropulsorComponent))]
public class ShipUnit : MonoBehaviour
{
	[SerializeField] private bool UseCameraShakeOnExplode = true;
	[SerializeField] private float ExplodeShakeTime = 2f;
	[SerializeField] private float ExplodeShakeAmount = 2f;

	private ShakeComponent CameraShakeComp = null;

	public static ShipUnit Instance { get; private set; } = null;

	public PropulsorComponent PropulsorComp { get; private set; } = null;

	public void Explode()
	{
		if (UseCameraShakeOnExplode && CameraShakeComp)
		{
			CameraShakeComp.ShakeCamera(ExplodeShakeTime, ExplodeShakeAmount);
		}
		Destroy(transform.gameObject);
	}

	void Awake()
    {
		if (Instance && Instance != this)
		{
			Debug.LogError("Multiple Instances of ShipUnit");
			Destroy(this);
			return;
		}
		Instance = this;
		PropulsorComp = GetComponent<PropulsorComponent>();
	}

	private void Start()
	{
		CameraShakeComp = FindObjectOfType<ShakeComponent>();
	}
}
