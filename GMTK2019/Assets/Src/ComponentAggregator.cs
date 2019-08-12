using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class ComponentAggregator : MonoBehaviour
{
    CustomSampler Sampler;
    public string AggregatorName = "ComponentAggregator";
    public HashSet<AggregatedComponent> Components { get; } = new HashSet<AggregatedComponent>();

    public void Start()
    {
        Sampler = CustomSampler.Create(AggregatorName);
    }
    public void RegisterComponent(AggregatedComponent Component)
    {
        Sampler.Begin();
        Components.Add(Component);

        Sampler.End();
    }

    public void UnregisterComponent(AggregatedComponent Component)
    {
        Sampler.Begin();
        Components.Remove(Component);
        Sampler.End();
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
