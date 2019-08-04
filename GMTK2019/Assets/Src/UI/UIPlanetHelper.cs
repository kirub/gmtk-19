using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlanetHelper : MonoBehaviour
{
    public const int HintedPlanetCount = 5;
    List<GameObject> Planets = new List<GameObject>(HintedPlanetCount); 
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (ShipUnit.Instance)
        {
            List<GameObject> PlanetsInCell = PlanetManager.Instance.GetPlanetsInGridFromPosition(ShipUnit.Instance.transform.position);
            if (PlanetsInCell != null)
            {
                foreach (GameObject CurrentPlanet in PlanetsInCell)
                {
                    if (Planets.Contains(CurrentPlanet))
                        continue;

                    float CurrentPlanetDistance = Vector3.Distance(CurrentPlanet.transform.position, ShipUnit.Instance.transform.position);
                    if(Planets.Count < HintedPlanetCount)
                    {
                        Planets.Add(CurrentPlanet);
                    }
                    else
                    {
                        for (int Idx = Planets.Count - 1; Idx >= 0; Idx--)
                        {
                            GameObject AlreadyInPlanet = Planets[Idx];
                            if (AlreadyInPlanet)
                            {
                                float AlreadyInDistance = Vector3.Distance(AlreadyInPlanet.transform.position, ShipUnit.Instance.transform.position);
                                if (CurrentPlanetDistance <= AlreadyInDistance)
                                {
                                    Planets.RemoveAt(Idx);
                                    Planets.Add(CurrentPlanet);
                                }
                            }
                        }
                    }
                }
            }

            foreach (GameObject Planet in Planets)
            {
                Vector3 ViewportPos = Camera.main.WorldToViewportPoint(Planet.transform.position);
                bool IsVisible =
                    (ViewportPos.x > 0.2f && ViewportPos.x < 0.8f) &&
                    (ViewportPos.y > 0.2f && ViewportPos.y < 0.8f);

                List<SpriteRenderer> Sprites = new List<SpriteRenderer>();
                Planet.GetComponentsInChildren<SpriteRenderer>(true, Sprites);
                SpriteRenderer UIPlanetHelper = Sprites.Find(x => x.CompareTag("UIPlanetHelper"));
                UIPlanetHelper.gameObject.SetActive(!IsVisible);

                if (!IsVisible)
                {
                    float CurrentPlanetDistance = Vector3.Distance(Planet.transform.position, ShipUnit.Instance.transform.position);
                    TextMesh UIPlanetDistanceText = UIPlanetHelper.gameObject.GetComponentInChildren<TextMesh>();
                    UIPlanetDistanceText.transform.SetParent(UIPlanetHelper.gameObject.transform);
                    UIPlanetDistanceText.text = ((int)CurrentPlanetDistance).ToString();

                    Vector3 VecShipToPlanet = Planet.gameObject.transform.position - ShipUnit.Instance.transform.position;
                    Bounds bounds = CameraExtensions.OrthographicBounds(Camera.main);

                    VecShipToPlanet.Normalize();
                    UIPlanetHelper.gameObject.transform.position = bounds.center - (new Vector3(-VecShipToPlanet.x * (bounds.size.x / 2), 10.0f, -VecShipToPlanet.z * (bounds.size.y / 2)) * 1.2f);
                    UIPlanetHelper.gameObject.transform.rotation = Quaternion.LookRotation(VecShipToPlanet);
                }
            }
        }
    }
}
