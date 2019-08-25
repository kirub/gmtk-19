using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionComponentAggregator : ComponentAggregator<AttractionComponent>
{
	protected void Awake()
	{
		AggregatorName = "AttractionComponentAggregator";
	}
}
