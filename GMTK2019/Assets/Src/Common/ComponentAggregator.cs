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
		Sampler = CustomSampler.Create(AggregatorName);
	}

	private void Update()
    {
        Sampler.Begin();
        foreach (T CurrentComponent in Components)
        {
            if (CurrentComponent && CurrentComponent.IsTickable())
            {
                CurrentComponent.Tick();
            }
        }
        Sampler.End();
    }

	public void RegisterComponent(T Component)
	{
		Component.enabled = false;
		Components.Add(Component);
	}

	public void UnregisterComponent(T Component)
	{
		Components.Remove(Component);
		Component.enabled = true;
	}
}
