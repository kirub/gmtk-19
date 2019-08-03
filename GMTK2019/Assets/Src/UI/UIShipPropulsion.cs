using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShipPropulsion : MonoBehaviour
{
	[SerializeField] private float YOffset = 20f;
	[SerializeField] private RectTransform NeutralTransform = null;
	[SerializeField] private RectTransform GoodTransform = null;
	[SerializeField] private RectTransform BadTransform = null;
	[SerializeField] private RectTransform Slider = null;

	private RectTransform RectTrans = null;
	private float TotalWidth = 0f;

	void OnPropulsionStart()
	{
		gameObject.SetActive(true);
	}

	void OnPropulsionEnd()
	{
		gameObject.SetActive(false);
	}

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

		TotalWidth = RectTrans.rect.width;
		NeutralTransform.anchoredPosition = new Vector2(0f, 0f);
		NeutralTransform.sizeDelta = new Vector2(-TotalWidth * (1f - ShipUnit.Instance.PropulsorComp.NeutralPropulsionRatio), 0f);

		float GoodPos = ShipUnit.Instance.PropulsorComp.NeutralPropulsionRatio;
		GoodTransform.anchoredPosition = new Vector2(TotalWidth * GoodPos, 0f);
		GoodTransform.sizeDelta = new Vector2(-TotalWidth * (1f - ShipUnit.Instance.PropulsorComp.GoodPropulsionRatio), 0f);

		float BadPos = ShipUnit.Instance.PropulsorComp.NeutralPropulsionRatio + ShipUnit.Instance.PropulsorComp.GoodPropulsionRatio;
		BadTransform.anchoredPosition = new Vector2(TotalWidth * BadPos, 0f);
		BadTransform.sizeDelta = new Vector2(-TotalWidth * (1f - ShipUnit.Instance.PropulsorComp.BadPropulsionRatio), 0f);

		ShipUnit.Instance.PropulsorComp.OnPropulseStartEvent.AddListener(OnPropulsionStart);
		ShipUnit.Instance.PropulsorComp.OnPropulseEndEvent.AddListener(OnPropulsionEnd);
		ShipUnit.Instance.PropulsorComp.OnPropulseCancelEvent.AddListener(OnPropulsionEnd);

		gameObject.SetActive(false);
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
			Destroy(this);
			return;
		}

		Slider.anchoredPosition = new Vector2(TotalWidth * ShipUnit.Instance.PropulsorComp.CurrentPropulsionRatio, Slider.anchoredPosition.y);
		UpdatePosition();
	}

	void UpdatePosition()
	{
		Vector3 ScreenPos = Camera.main.WorldToScreenPoint(ShipUnit.Instance.transform.position);
		ScreenPos.y += YOffset;
		RectTrans.position = ScreenPos;
	}
}
