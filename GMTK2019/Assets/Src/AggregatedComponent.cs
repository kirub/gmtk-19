using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggregatedComponent : MonoBehaviour
{
    ComponentAggregator Aggregator = null;

    virtual public void Awake()
    {
        Aggregator = GetComponentInParent<ComponentAggregator>();

    }
    // Start is called before the first frame update
    virtual public void Start()
    {
        if(Aggregator)
        {
            Aggregator.RegisterComponent(this);
        }
    }

    virtual public void OnDestroy()
    {
        if (Aggregator)
        {
            Aggregator.UnregisterComponent(this);
        }
    }

    virtual public void Tick()
    {
    }
}
