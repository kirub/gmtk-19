using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Renderer))]
public class UIShipPropulsion : MonoBehaviour
{
	[SerializeField] private GameObject WarningObjects = null;
	private Renderer PropulsionObj = null;

	void OnPropulsionStart()
	{
		enabled = true;
		UpdatePropulsionValue(0f);
	}

	void OnPropulsionEnd()
	{
		enabled = false;
		UpdatePropulsionValue(0f);
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

		UpdatePropulsionValue(0f);
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

		UpdatePropulsionValue(ShipUnit.Instance.PropulsorComp.CurrentPropulsionRatio);
	}

	void UpdatePropulsionValue(float NewValue)
	{
		PropulsionObj.material.SetFloat("_Percent", NewValue);

		if (WarningObjects)
		{
			WarningObjects.SetActive(NewValue > ShipUnit.Instance.PropulsorComp.GoodPropulsionThreshold);
		}
	}
}
