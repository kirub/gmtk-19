using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalComponent : AttractionComponent
{
    List<SpriteRenderer> Sprites = new List<SpriteRenderer>();
    SpriteRenderer UIPlanetHelper = null;
    TextMesh UIPlanetDistanceText = null;

    public bool IsUIActive { get; set; } = false;
    public bool IsUIVisible { get; set; } = false;

    public void Awake()
    {
        Sprites.Clear();
        gameObject.GetComponentsInChildren(true, Sprites);
        UIPlanetHelper = Sprites.Find(x => x.CompareTag("UIPlanetHelper"));
        UIPlanetDistanceText = GetComponentInChildren<TextMesh>();
        UIPlanetDistanceText.transform.SetParent(UIPlanetHelper.gameObject.transform);
    }

    public bool CheckVisibility()
    {
        if (IsUIActive)
        {
            Vector3 ViewportPos = Camera.main.WorldToViewportPoint(gameObject.transform.position);
            IsUIVisible = ShipUnit.Instance &&
                (ViewportPos.x <= 0.0f || ViewportPos.x >= 1.0f) &&
                (ViewportPos.y <= 0.0f || ViewportPos.y >= 1.0f);
        }

        return IsUIVisible;
    }

    public void UpdateUI()
    {
        if (IsUIActive && CheckVisibility())
        {
            float CurrentPlanetDistance = Vector3.Distance(transform.position, ShipUnit.Instance.transform.position);
            UIPlanetDistanceText.text = ((int)CurrentPlanetDistance).ToString();
            Vector3 VecShipToPlanet = gameObject.transform.position - ShipUnit.Instance.transform.position;
            Bounds bounds = CameraExtensions.OrthographicBounds(Camera.main);
            VecShipToPlanet.Normalize();
            UIPlanetHelper.gameObject.transform.position = bounds.center - (new Vector3(-VecShipToPlanet.x * (bounds.size.x / 2), 10.0f, -VecShipToPlanet.z * (bounds.size.y / 2)) * 1.2f);
            UIPlanetHelper.gameObject.transform.rotation = Quaternion.LookRotation(VecShipToPlanet);
        }
    }
}
