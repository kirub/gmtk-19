using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometComponent : MonoBehaviour
{
	[SerializeField] private SpriteRenderer DistanceSprite = null;
	[SerializeField] private float DisplaySpriteTime = 1f;

	private Color CurrentSpriteColor = new Color();
	private float CurrentSpriteTransparency = 0f;
	private float CurrentDisplaySpriteTime = 0f;

	private float StartSpriteTransparency = 0f;
	private float FinalSpriteTransparency = 0f;

	void OnCanPropulseStart()
	{
		StartSpriteTransparency = 0f;
		FinalSpriteTransparency = 1f;
		enabled = true;
	}

	void OnCanPropulseEnd()
	{
		StartSpriteTransparency = 1f;
		FinalSpriteTransparency = 0f;
		enabled = true;
	}

	void UpdateSpriteColor( float NewTransparency )
	{
		CurrentSpriteColor.a = NewTransparency;
		DistanceSprite.color = CurrentSpriteColor;
	}

	private void Awake()
	{
		if (!DistanceSprite)
		{
			Debug.LogError("No DistanceSprite on " + this + " CometComponent");
			Destroy(this);
		}

		CurrentDisplaySpriteTime = 0f;
		FinalSpriteTransparency = CurrentSpriteTransparency = 0f;
		CurrentSpriteColor = DistanceSprite.color;
		UpdateSpriteColor(FinalSpriteTransparency);
	}

	private void Start()
	{
		if (ShipUnit.Instance)
		{
			ShipUnit.Instance.PropulsorComp.OnCanPropulseStartEvent.AddListener(OnCanPropulseStart);
			ShipUnit.Instance.PropulsorComp.OnCanPropulseEndEvent.AddListener(OnCanPropulseEnd);
		}
		enabled = false;
	}

	private void OnDestroy()
	{
		if (ShipUnit.Instance)
		{
			ShipUnit.Instance.PropulsorComp.OnCanPropulseStartEvent.RemoveListener(OnCanPropulseStart);
			ShipUnit.Instance.PropulsorComp.OnCanPropulseEndEvent.RemoveListener(OnCanPropulseEnd);
		}
	}

	private void Update()
	{
		CurrentDisplaySpriteTime += Time.deltaTime;
		if (CurrentDisplaySpriteTime >= DisplaySpriteTime)
		{
			CurrentDisplaySpriteTime = 0f;
			UpdateSpriteColor(FinalSpriteTransparency);
			enabled = false;
		}
		else
		{
			CurrentSpriteTransparency = Mathf.Lerp(StartSpriteTransparency, FinalSpriteTransparency, CurrentDisplaySpriteTime / DisplaySpriteTime);
			UpdateSpriteColor(CurrentSpriteTransparency);
		}
	}
}
