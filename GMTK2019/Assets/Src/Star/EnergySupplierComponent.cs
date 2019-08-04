using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySupplierComponent : MonoBehaviour
{
	[SerializeField] private int Energy = 1;
	public int AvailableEnergy { get { return Energy; } }
}
