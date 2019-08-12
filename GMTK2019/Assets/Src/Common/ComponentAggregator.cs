using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class ComponentAggregator : MonoBehaviour
{
    CustomSampler Sampler;
    public string AggregatorName = "ComponentAggregator";
    public HashSet<AggregatedComponent> Components { get; } = new HashSet<AggregatedComponent>();
    
    public void RegisterComponent(AggregatedComponent Component)
    {
        Sampler = CustomSampler.Create(AggregatorName);
        Components.Add(Component);
    }

    public void UnregisterComponent(AggregatedComponent InComponent)
    {
        Components.Remove(InComponent);
    }

    // Update is called once per frame
    private void Update()
    {
        Sampler.Begin();
        foreach (AggregatedComponent CurrentComponent in Components)
        {
            if (CurrentComponent.isActiveAndEnabled)
            {
                CurrentComponent.Tick();
            }
        }
        Sampler.End();
    }
}
