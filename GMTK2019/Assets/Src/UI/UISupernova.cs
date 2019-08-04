using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        SupernovaComp = gameObject.transform.parent.GetComponentInChildren<Supernova>();

        Vector3 VecShipToSupernova = SupernovaComp.gameObject.transform.position - ShipUnit.Instance.transform.position;
        Bounds bounds = CameraExtensions.OrthographicBounds(Camera.main);
        gameObject.transform.Rotate(Vector3.up, Vector3.Angle(VecShipToSupernova, Vector3.back));
    }

    // Update is called once per frame
    void Update()
    {
        if (ShipUnit.Instance)
        {
            Vector3 VecShipToSupernova = SupernovaComp.gameObject.transform.position - ShipUnit.Instance.transform.position;
            Bounds bounds = CameraExtensions.OrthographicBounds(Camera.main);

            VecShipToSupernova.Normalize();
            gameObject.transform.position = bounds.center - new Vector3(-VecShipToSupernova.x * (bounds.size.x / 2), 10.0f, -VecShipToSupernova.z * (bounds.size.y / 2));
            transform.rotation = Quaternion.LookRotation(VecShipToSupernova);
        }
    }
}
