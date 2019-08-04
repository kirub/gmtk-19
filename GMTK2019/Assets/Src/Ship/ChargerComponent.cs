using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerComponent : MonoBehaviour
{
	[SerializeField] private bool CanAddRechargeWithR = false;

	[SerializeField] private int MaxRecharge = 3;
	[SerializeField] private int BaseRecharge = 3;

	[SerializeField] private float RechargeTime = 1f;

	[SerializeField] private AudioSource RechargeCompleteSound = null;

	public int CurrentNumRecharge { get; private set; } = 0;

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

	private void Update()
	{
		if (CanAddRechargeWithR && Input.GetKeyUp(KeyCode.R))
		{
			AddCharge();
		}

		if (IsRecharging && MaxRechargeAvailable > 0)
		{
			CurrentRechargeTime += Time.unscaledDeltaTime;
			if (CurrentRechargeTime > RechargeTime)
			{
				AddCharge();
				--MaxRechargeAvailable;
			}
		}
	}
}
