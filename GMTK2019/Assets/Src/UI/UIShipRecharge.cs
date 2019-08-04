using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShipRecharge : MonoBehaviour
{
	[SerializeField] private float TransparencyWhenUsed = 0.2f;
	[SerializeField] private List<Image> FullRechargesObj = new List<Image>();

	private int CurrentNumRecharges = 0;
	private Color DefaultColor = new Color();
	private Color UsedColor = new Color();

	void Start()
    {
		if (!ShipUnit.Instance)
		{
			Debug.LogError("No Ship Instance found in " + this + "UIShipPropulsion");
			Destroy(this);
			return;
		}

		if (FullRechargesObj.Count > 1)
		{
			DefaultColor = FullRechargesObj[0].color;
		}
		UsedColor = DefaultColor;
		UsedColor.a = TransparencyWhenUsed;

		UpdateRechargesVisibility();
	}

	private void Update()
	{
		if (!ShipUnit.Instance)
		{
			enabled = false;
			return;
		}

		if (CurrentNumRecharges != ShipUnit.Instance.ChargerComp.CurrentNumRecharge)
		{
			UpdateRechargesVisibility();
		}
	}

	void UpdateRechargesVisibility()
	{
		CurrentNumRecharges = ShipUnit.Instance.ChargerComp.CurrentNumRecharge;
		for (int r = 0; r < FullRechargesObj.Count; ++r)
		{
			FullRechargesObj[r].color = (r < CurrentNumRecharges) ? DefaultColor : UsedColor;
		}
	}
}
