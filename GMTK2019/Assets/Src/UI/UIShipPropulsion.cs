using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Renderer))]
public class UIShipPropulsion : MonoBehaviour
{
	private Renderer PropulsionObj = null;

	void OnPropulsionStart()
	{
		enabled = true;
		UpdatePropulsionValue();
	}

	void OnPropulsionEnd()
	{
		enabled = false;
		UpdatePropulsionValue();
	}

	private void Awake()
	{
		PropulsionObj = GetComponent<Renderer>();
	}

	void Start()
    {
		if (!ShipUnit.Instance)
		{
			Debug.LogError("No Ship Instance found in " + this + "UIShipPropulsion");
			Destroy(this);
			return;
		}
		
		ShipUnit.Instance.PropulsorComp.OnPropulseStartEvent.AddListener(OnPropulsionStart);
		ShipUnit.Instance.PropulsorComp.OnPropulseEndEvent.AddListener(OnPropulsionEnd);
		ShipUnit.Instance.PropulsorComp.OnPropulseCancelEvent.AddListener(OnPropulsionEnd);

		UpdatePropulsionValue();
	}

	private void OnDestroy()
	{
		if (!ShipUnit.Instance)
		{
			return;
		}

		ShipUnit.Instance.PropulsorComp.OnPropulseStartEvent.RemoveListener(OnPropulsionStart);
		ShipUnit.Instance.PropulsorComp.OnPropulseEndEvent.RemoveListener(OnPropulsionEnd);
		ShipUnit.Instance.PropulsorComp.OnPropulseCancelEvent.RemoveListener(OnPropulsionEnd);
	}

	private void Update()
	{
		if (!ShipUnit.Instance)
		{
			enabled = false;
			return;
		}

		UpdatePropulsionValue();
	}

	void UpdatePropulsionValue()
	{
		PropulsionObj.material.SetFloat("_Percent", ShipUnit.Instance.PropulsorComp.CurrentPropulsionRatio);
	}
}
