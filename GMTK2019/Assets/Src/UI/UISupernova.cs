using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class CameraExtensions
{
    public static Bounds OrthographicBounds(this Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }
}

public class UISupernova : MonoBehaviour
{
    Vector3         SupernovaArrowPos   = new Vector3(0.0f, 0.0f, 0.0f);
    Supernova       SupernovaComp       = null;
    TextMesh        UIInGameScore       = null;
    private bool    IsVisible           = false;

    // Start is called before the first frame update
    void Start()
    {
        UIInGameScore = GetComponentInChildren<TextMesh>();
        UIInGameScore.transform.SetParent(this.transform);
        SupernovaComp = gameObject.transform.parent.GetComponentInChildren<Supernova>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ShipUnit.Instance)
        {
            Vector3 ViewportPos = Camera.main.WorldToViewportPoint(SupernovaComp.gameObject.transform.position);
            IsVisible =
                (ViewportPos.x > -0.1f && ViewportPos.x < 1.1f) &&
                (ViewportPos.y > -0.1f && ViewportPos.y < 1.1f);
            if (!IsVisible)
            {
                Vector3 VecShipToSupernova = SupernovaComp.gameObject.transform.position - ShipUnit.Instance.transform.position;
                Bounds bounds = CameraExtensions.OrthographicBounds(Camera.main);

                VecShipToSupernova.Normalize();
                transform.position = bounds.center - (new Vector3(-VecShipToSupernova.x * (bounds.size.x / 2), 10.0f, -VecShipToSupernova.z * (bounds.size.y / 2)) * 1.2f);
                transform.rotation = Quaternion.LookRotation(VecShipToSupernova);

                UIInGameScore.text = ((int)Supernova.Instance.GetPlayerDistanceFromCenter()).ToString();
            }
        }
    }
}
