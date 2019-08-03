using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShipRecharge : MonoBehaviour
{
	[SerializeField] private float YOffset = 20f;
	[SerializeField] private GameObject RechargesContainerObj = null;
	[SerializeField] private List<GameObject> FullRechargesObj = new List<GameObject>();

	private RectTransform RectTrans = null;
	private int CurrentNumRecharges = 0;

	void Start()
    {
		if (!ShipUnit.Instance)
		{
			Debug.LogError("No Ship Instance found in " + this + "UIShipPropulsion");
			Destroy(this);
			return;
		}

		RectTrans = GetComponent<RectTransform>();
		UpdatePosition();

		CurrentNumRecharges = ShipUnit.Instance.PropulsorComp.CurrentNumRecharge;
		for ( int r = 0; r < FullRechargesObj.Count; ++r )
		{
			FullRechargesObj[r].SetActive(r < CurrentNumRecharges);
		}
		RechargesContainerObj.SetActive(CurrentNumRecharges < FullRechargesObj.Count);
	}

	private void Update()
	{
		if (!ShipUnit.Instance)
		{
			Destroy(gameObject);
			return;
		}

		if (CurrentNumRecharges != ShipUnit.Instance.PropulsorComp.CurrentNumRecharge)
		{
			UpdateRechargesVisibility();
		}

		UpdatePosition();
	}

	void UpdateRechargesVisibility()
	{
		CurrentNumRecharges = ShipUnit.Instance.PropulsorComp.CurrentNumRecharge;
		for (int r = 0; r < FullRechargesObj.Count; ++r)
		{
			FullRechargesObj[r].SetActive(r < CurrentNumRecharges);
		}
		RechargesContainerObj.SetActive(CurrentNumRecharges < FullRechargesObj.Count);
	}

	void UpdatePosition()
	{
		Vector3 ScreenPos = Camera.main.WorldToScreenPoint(ShipUnit.Instance.transform.position);
		ScreenPos.y += YOffset;
		RectTrans.position = ScreenPos;
	}
}
