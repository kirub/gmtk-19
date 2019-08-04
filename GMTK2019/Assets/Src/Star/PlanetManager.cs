using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public static float G = 9.81f;

    public static PlanetManager Instance { get; private set; } = null;

    public ArrayList Planets { get; } = new ArrayList();

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject FindAttractedPlanet(GameObject Planet )
    {
        OrbitalComponent AttractedByComponent = null;
        OrbitalComponent LastComponent = null;
        OrbitalComponent AddedOrbitalComp = Planet.GetComponent<OrbitalComponent>();
        foreach (GameObject PlanetObj in Planets)
        {
            OrbitalComponent OrbitalComp = PlanetObj.GetComponent<OrbitalComponent>();
            if (OrbitalComp)
            {
                bool IsAttracted = false;
                KeyValuePair<OrbitalComponent, OrbitalComponent> Pair1;
                KeyValuePair<OrbitalComponent, OrbitalComponent> Pair2;
                if (!AddedOrbitalComp.IsStatic && LastComponent)
                {
                    Pair1 = new KeyValuePair<OrbitalComponent, OrbitalComponent>(AddedOrbitalComp, LastComponent);
                    Pair2 = new KeyValuePair<OrbitalComponent, OrbitalComponent>(AddedOrbitalComp, OrbitalComp);

                    if (OrbitalComponent.CompareAttraction(Pair1, Pair2))
                    {
                        IsAttracted = true;
                        AttractedByComponent = OrbitalComp;
                        LastComponent = AttractedByComponent;
                    }
                    else if (!AttractedByComponent)
                    {
                        AttractedByComponent = LastComponent;
                    }
                }

                if (!IsAttracted && !OrbitalComp.IsStatic)
                {
                    if (OrbitalComp.AttractedBy)
                    {
                        Pair1 = new KeyValuePair<OrbitalComponent, OrbitalComponent>(OrbitalComp, OrbitalComp.AttractedBy.GetComponent<OrbitalComponent>());
                        Pair2 = new KeyValuePair<OrbitalComponent, OrbitalComponent>(OrbitalComp, AddedOrbitalComp);

                        if (OrbitalComponent.CompareAttraction(Pair1, Pair2))
                        {
                            OrbitalComp.AttractedBy = AddedOrbitalComp.gameObject;
                        }
                    }
                    else
                    {
                        OrbitalComp.AttractedBy = AddedOrbitalComp.gameObject;
                    }
                }

                if (!LastComponent)
                {
                    LastComponent = OrbitalComp;
                }
            }
        }

        return AttractedByComponent.gameObject;
    }

    public GameObject RegisterPlanet( GameObject Planet )
    {
        GameObject AttractedByObj = null;
        //AttractedByObj = FindAttractedPlanet(Planet);
        Planets.Add(Planet);
        return AttractedByObj;
    }
}
