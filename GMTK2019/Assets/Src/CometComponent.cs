using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometComponent : MonoBehaviour
{
	[SerializeField] private ParticleSystem RangeParticle = null;

	private Color CurrentSpriteColor = new Color();
	private float CurrentSpriteTransparency = 0f;
	private float CurrentDisplaySpriteTime = 0f;

	private float StartSpriteTransparency = 0f;
	private float FinalSpriteTransparency = 0f;

	void OnCanPropulseStart()
	{
		RangeParticle.gameObject.SetActive(true);
	}

	void OnCanPropulseEnd()
	{
		RangeParticle.gameObject.SetActive(false);
	}

	private void Awake()
	{
		if (!RangeParticle)
		{
			Debug.LogError("No RangeParticle on " + this + " CometComponent");
			Destroy(this);
			return;
		}

		RangeParticle.gameObject.SetActive(false);
	}

	private void Start()
	{
		if (ShipUnit.Instance)
		{
			ShipUnit.Instance.PropulsorComp.OnCanPropulseStartEvent.AddListener(OnCanPropulseStart);
			ShipUnit.Instance.PropulsorComp.OnCanPropulseEndEvent.AddListener(OnCanPropulseEnd);
		}
	}

	private void OnDestroy()
	{
		if (ShipUnit.Instance)
		{
			ShipUnit.Instance.PropulsorComp.OnCanPropulseStartEvent.RemoveListener(OnCanPropulseStart);
			ShipUnit.Instance.PropulsorComp.OnCanPropulseEndEvent.RemoveListener(OnCanPropulseEnd);
		}
	}
}
