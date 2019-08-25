using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAggregatedComponent
{
	/*
	 * In MonoBehaviour implementing this interfqce, add this to tick even without a ComponentAggregator
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
