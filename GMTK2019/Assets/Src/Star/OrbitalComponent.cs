using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalComponent : AttractionComponent
{
    public void Awake()
    {
    }

    // Start is called before the first frame update
    new void Start()
    {
        PlanetManager.Instance.RegisterPlanet(gameObject);
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

    }
}
