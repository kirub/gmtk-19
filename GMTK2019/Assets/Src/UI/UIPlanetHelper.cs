using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlanetHelper : MonoBehaviour
{
    public const int HintedPlanetCount = 5;
    List<OrbitalComponent> Planets = new List<OrbitalComponent>(HintedPlanetCount);
    private float CurrentMaxDistance = -1.0f;

    public void UpdateFor(OrbitalComponent Planet)
    {
        if (ShipUnit.Instance == null)
        {
            Planet.IsUIActive = false;
            return;
        }

        float CurrentPlanetDistance = Vector3.Distance(Planet.transform.position, ShipUnit.Instance.transform.position);

        if (Planet.IsUIActive)
            return;

        if (Planets.Count < HintedPlanetCount)
        {
            Planets.Add(Planet);
            Planet.IsUIActive = true;
            if (CurrentPlanetDistance > CurrentMaxDistance)
            {
                CurrentMaxDistance = CurrentPlanetDistance;
            }
        }
        else
        {
            if (CurrentMaxDistance != -1 && CurrentMaxDistance < CurrentPlanetDistance)
            {
                Planet.IsUIActive = false;
                return;
            }

            OrbitalComponent AlreadyInPlanet = null;
            for (int Idx = Planets.Count - 1; Idx >= 0; Idx--)
            {
                AlreadyInPlanet = Planets[Idx];
                if (AlreadyInPlanet)
                {
                    float AlreadyInDistance = Vector3.Distance(AlreadyInPlanet.transform.position, ShipUnit.Instance.transform.position);
                    if (CurrentPlanetDistance <= AlreadyInDistance)
                    {
                        Planets[Idx].IsUIActive = false;
                        Planets.RemoveAt(Idx);
                        Planets.Add(Planet);
                        Planet.IsUIActive       = true;
                        if (CurrentPlanetDistance > CurrentMaxDistance)
                        {
                            CurrentMaxDistance = CurrentPlanetDistance;
                        }
                        break;
                    }
                }
            }
        }
    }

    private void Update()
    {
        foreach( OrbitalComponent Planet in Planets)
        {
            Planet.UpdateUI();
        }
    }
}
