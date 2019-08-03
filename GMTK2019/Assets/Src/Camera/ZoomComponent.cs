﻿using System.Collections;
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
	[SerializeField] private float ZoomSpeed = 0f;
	private float ZoomModifier = 0f;

	void OnZoomStart()
	{
		if (!enabled)
		{
			BaseY = transform.position.y;
		}

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
		if (!ShipUnit.Instance)
		{
			Debug.LogWarning("No Ship Instance found, will not be able to use");
			return;
		}

		ShipUnit.Instance.PropulsorComp.OnPropulseStartEvent.AddListener(OnZoomStart);
		ShipUnit.Instance.PropulsorComp.OnPropulseEndEvent.AddListener(OnZoomEnd);

		ZoomSpeed = ZoomBaseSpeed;
		enabled = false;
	}

	private void Update()
	{
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
