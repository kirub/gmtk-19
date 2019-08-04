using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIShipPropulsion : MonoBehaviour
{
	[SerializeField] private GameObject WarningObjects = null;
	private Image PropulsionObj = null;

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
		PropulsionObj = GetComponent<Image>();
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
		PropulsionObj.material.SetFloat("Percent", ShipUnit.Instance.PropulsorComp.CurrentPropulsionRatio);

		if (WarningObjects)
		{
			WarningObjects.SetActive(ShipUnit.Instance.PropulsorComp.CurrentPropulsionRatio > ShipUnit.Instance.PropulsorComp.GoodPropulsionThreshold);
		}
	}
}
