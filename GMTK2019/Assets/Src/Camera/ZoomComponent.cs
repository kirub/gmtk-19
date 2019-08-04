using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZoomComponent : MonoBehaviour
{
	[SerializeField] private float MaxZoom = 100f;
	[SerializeField] private float ZoomBaseSpeed = 1f;
	[SerializeField] private float ZoomMaxSpeed = 1f;
	[SerializeField] private float ZoomAcceleration = 1f;

	private float BaseY = 0f;
	private float ZoomSpeed = 0f;
	private float ZoomModifier = 0f;

	private bool IsBaseYSetup = false;

	void OnZoomStart()
	{
		ZoomModifier = -1f;
		enabled = true;
	}

	void OnZoomEnd()
	{
		ZoomModifier = 1f;
		enabled = true;
	}

	private void Start()
	{
		if (ShipUnit.Instance)
		{
			ShipUnit.Instance.PropulsorComp.OnPropulseStartEvent.AddListener(OnZoomStart);
			ShipUnit.Instance.PropulsorComp.OnPropulseEndEvent.AddListener(OnZoomEnd);
			ShipUnit.Instance.PropulsorComp.OnPropulseCancelEvent.AddListener(OnZoomEnd);

			ZoomSpeed = ZoomBaseSpeed;
			enabled = false;
		}
		else
		{
			Debug.LogWarning("No Ship Instance found in " + this + "ZoomComponent");
			Destroy(this);
		}
	}

	private void OnDestroy()
	{
		if (!ShipUnit.Instance)
		{
			return;
		}

		ShipUnit.Instance.PropulsorComp.OnPropulseStartEvent.RemoveListener(OnZoomStart);
		ShipUnit.Instance.PropulsorComp.OnPropulseEndEvent.RemoveListener(OnZoomEnd);
		ShipUnit.Instance.PropulsorComp.OnPropulseCancelEvent.RemoveListener(OnZoomEnd);
	}

	private void Update()
	{
		if (!IsBaseYSetup)
		{
			BaseY = transform.position.y;
		}

		ZoomSpeed = Mathf.Min(ZoomMaxSpeed, ZoomSpeed + ZoomAcceleration * Time.unscaledDeltaTime);
		transform.position = transform.position - Vector3.up * ZoomModifier * Time.unscaledDeltaTime * ZoomSpeed;
		if (transform.position.y >= MaxZoom)
		{
			ZoomSpeed = ZoomBaseSpeed;
			transform.position = new Vector3(transform.position.x, MaxZoom, transform.position.z);
			enabled = false;
		}
		else if (transform.position.y <= BaseY)
		{
			ZoomSpeed = ZoomBaseSpeed;
			transform.position = new Vector3(transform.position.x, BaseY, transform.position.z);
			enabled = false;
		}
	}
}
