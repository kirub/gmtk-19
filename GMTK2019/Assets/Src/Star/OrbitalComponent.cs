using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalComponent : MonoBehaviour
{

    public float Mass { get; private set; }
    public float AttractionMass { get; private set; }
    public GameObject _AttractedBy;
    public GameObject AttractedBy {
        get
        {
            return _AttractedBy;
        }
        set
        {
            _AttractedBy = value;
        }
    }
    [SerializeField]
    private bool IsAStaticStar = false;
    public bool IsStatic { get { return IsAStaticStar; } }

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        /*AttractedBy =*/ PlanetManager.Instance.RegisterPlanet(gameObject);
        gameObject.transform.SetParent(AttractedBy.gameObject.transform, true);
        AttractionMass = gameObject.GetComponent<Rigidbody>().mass + AttractedBy.gameObject.GetComponent<Rigidbody>().mass;
        if (!AttractedBy)
        {
            Debug.Log("no Attracted by");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AttractedBy)
        {
            float Dist = Vector3.Distance(AttractedBy.transform.position, gameObject.transform.position);
            /*
            float k = (4 * Mathf.PI * Mathf.PI) / ;
            //float PeriodSq = k * Mathf.Pow(Dist, 3);
            float PeriodSq = k * Dist * Dist;*/
            float Speed = Time.deltaTime * 300 /Dist;// Mathf.Sqrt(PlanetManager.G * AttractionMass / Dist);
            
            gameObject.transform.RotateAround(AttractedBy.transform.position, Vector3.up, Speed);
        }
    }



    public static float ComputeAttraction(OrbitalComponent Lhs, OrbitalComponent Rhs)
    {
        Rigidbody LhsRBody = Lhs.gameObject.GetComponent<Rigidbody>();
        Rigidbody RhsRBody = Rhs.gameObject.GetComponent<Rigidbody>();
        float Distance = Vector3.Distance(Lhs.gameObject.transform.position, Rhs.gameObject.transform.position);
        float m1m2      = LhsRBody.mass * RhsRBody.mass;
        return PlanetManager.G * m1m2 / (Distance * Distance);
    }

    public static bool CompareAttraction( KeyValuePair<OrbitalComponent, OrbitalComponent> Lhs, KeyValuePair<OrbitalComponent, OrbitalComponent> Rhs)
    {
        return ComputeAttraction(Lhs.Key, Lhs.Value) < ComputeAttraction(Rhs.Key, Rhs.Value);
    }
}
