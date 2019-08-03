using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovingComponent))]
[RequireComponent(typeof(RotatorComponent))]
[RequireComponent(typeof(PropulsorComponent))]
public class ShipUnit : MonoBehaviour
{
	public static ShipUnit Instance { get; private set; } = null;

	public PropulsorComponent PropulsorComp { get; private set; } = null;

	void Awake()
    {
		if (Instance && Instance != this)
		{
			Debug.LogError("Multiple Instances of ShipUnit");
			Destroy(this);
			return;
		}
		Instance = this;
		PropulsorComp = GetComponent<PropulsorComponent>();
	}
}
