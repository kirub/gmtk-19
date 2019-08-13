using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerComponent : MonoBehaviour, IDebugDrawable
{
	[SerializeField] private bool CanAddRechargeWithR = false;

	[SerializeField] private int MaxRecharge = 3;
	[SerializeField] private int BaseRecharge = 3;

	[SerializeField] private float RechargeTime = 1f;

	[SerializeField] private AudioSource RechargeCompleteSound = null;

	public int CurrentNumRecharge { get; private set; } = 0;
	private bool CanRechargeMore { get { return IsRecharging && MaxRechargeAvailable > 0 && CurrentNumRecharge < MaxRecharge; } }

	public bool IsRecharging { get; private set; } = false;
	private int MaxRechargeAvailable = 0;
	private float CurrentRechargeTime = 0f;

	public void UseCharge()
	{
		CurrentNumRecharge = Mathf.Max(0, CurrentNumRecharge - 1);
		Debug.Log("Remaining recharge " + CurrentNumRecharge);
	}

	public void StartRecharge( int AvailableRecharge )
	{
		CurrentRechargeTime = 0f;
		IsRecharging = true;
		MaxRechargeAvailable = AvailableRecharge;
	}

	public void UpdateAvailability(int AvailableRecharge)
	{
		MaxRechargeAvailable = AvailableRecharge;
	}

	public void StopRecharge()
	{
		CurrentRechargeTime = 0f;
		IsRecharging = false;
	}

	void AddCharge()
	{
		CurrentNumRecharge = Mathf.Min(MaxRecharge, CurrentNumRecharge + 1);
		Debug.Log("New recharge " + CurrentNumRecharge);

		if (RechargeCompleteSound)
		{
			RechargeCompleteSound.Play();
		}
	}

	private void Awake()
	{
		CurrentNumRecharge = BaseRecharge;
		Debug.Log("Base recharge " + CurrentNumRecharge);
	}

	private void Start()
	{
		DebugDrawHelper.RegisterDrawable(gameObject, this);
	}

	private void OnDestroy()
	{
		DebugDrawHelper.UnregisterDrawable(gameObject, this);
	}

	private void Update()
	{
		if (CanAddRechargeWithR && Input.GetKeyUp(KeyCode.R))
		{
			AddCharge();
		}

		if (CanRechargeMore)
		{
			CurrentRechargeTime += Time.unscaledDeltaTime;
			if (CurrentRechargeTime > RechargeTime)
			{
				AddCharge();
				CurrentRechargeTime = 0f;
				--MaxRechargeAvailable;
				Debug.Log("Recharge available " + MaxRechargeAvailable);
			}
		}
	}

	public void DebugDraw(ref Rect BasePos, float TextYIncrement, GUIStyle Style)
	{
#if UNITY_EDITOR
		GUI.Label(BasePos, "- Recharges " + CurrentNumRecharge + "/" + MaxRecharge, Style);
		BasePos.y += TextYIncrement;

		if (IsRecharging)
		{
			if (CanRechargeMore)
			{
				float Percent = 100f * CurrentRechargeTime / RechargeTime;
				GUI.Label(BasePos, "- Charging " + Percent + "% [" + MaxRechargeAvailable + "]", Style);
				BasePos.y += TextYIncrement;
			}
			else
			{
				GUI.Label(BasePos, "- Charging MAX", Style);
				BasePos.y += TextYIncrement;
			}
		}
		else
		{
			GUI.Label(BasePos, "- Not charging...", Style);
			BasePos.y += TextYIncrement;
		}
#endif
	}
}
