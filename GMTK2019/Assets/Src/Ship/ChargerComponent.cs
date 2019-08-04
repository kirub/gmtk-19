using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerComponent : MonoBehaviour
{
	[SerializeField] private bool CanAddRechargeWithR = false;

	[SerializeField] private int MaxRecharge = 3;
	[SerializeField] private int BaseRecharge = 3;

	public int CurrentNumRecharge { get; private set; } = 0;

	public void UseCharge()
	{
		CurrentNumRecharge = Mathf.Max(0, CurrentNumRecharge - 1);
		Debug.Log("Remaining recharge " + CurrentNumRecharge);
	}

	private void Awake()
	{
		CurrentNumRecharge = BaseRecharge;
		Debug.Log("Starting recharge " + CurrentNumRecharge);
	}

	private void Update()
	{
		if (CanAddRechargeWithR && Input.GetKeyUp(KeyCode.R))
		{
			CurrentNumRecharge = Mathf.Min(MaxRecharge, CurrentNumRecharge + 1);
			Debug.Log("New recharge " + CurrentNumRecharge);
		}
	}
}
