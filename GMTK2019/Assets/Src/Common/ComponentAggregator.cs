using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class ComponentAggregator<T> : MonoBehaviour where T : MonoBehaviour, IAggregatedComponent
{
	/*
	 * Your inherited class should have this
	protected void Awake()
	{
		AggregatorName = "YourName";
	}
	 */

	protected string AggregatorName = "ComponentAggregator";

    private CustomSampler Sampler;
	public HashSet<T> Components { get; private set; } = new HashSet<T>();

	private void Start()
	{
		T[] AggregatedObjects = FindObjectsOfType<T>();
		Components = new HashSet<T>(AggregatedObjects);

		foreach (T CurrentComponent in Components)
		{
			CurrentComponent.enabled = false;
		}

		Sampler = CustomSampler.Create(AggregatorName);
	}

	private void Update()
    {
        Sampler.Begin();
        foreach (T CurrentComponent in Components)
        {
            if (CurrentComponent.IsTickable())
            {
                CurrentComponent.Tick();
            }
        }
        Sampler.End();
    }
}
