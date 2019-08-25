using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAggregatedComponent
{
	/*
	 * In MonoBehaviour implementing this interface, add the following (T is the class of your ComponentAggregator)
	void Start()
	{
		T Aggregator = FindObjectOfType<T>();
		if (Aggregator)
		{
			Aggregator.RegisterComponent(this);
		}
	}
	void OnDestroy()
	{
		T Aggregator = FindObjectOfType<T>();
		if (Aggregator)
		{
			Aggregator.UnregisterComponent(this);
		}
	}

	void Update()
	{
		if ( IsTickable() )
		{
			Tick();
		}
	}
	 */

	bool IsTickable();
	void Tick();
}
