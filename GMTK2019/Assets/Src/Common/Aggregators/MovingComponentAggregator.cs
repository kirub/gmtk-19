using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingComponentAggregator : ComponentAggregator<MovingComponent>
{
	protected void Awake()
	{
		AggregatorName = "MovingComponentAggregator";
	}
}
