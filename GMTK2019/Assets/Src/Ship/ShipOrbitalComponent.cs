using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipOrbitalComponent : MonoBehaviour
{
    public enum EOrbitalState
    {
        None,
        InOuterRadius,
        InInnerRadius,
        WillCrash
    }

    public GameObject       Planet { get; private set; } = null;
    public EOrbitalState    OrbitalState { get; private set; } = EOrbitalState.None;

    private bool            WillBeCounterClockWiseOrbit { get; set; } = false;
    private List<SphereCollider> Colliders = new List<SphereCollider>();
    private Vector3 TangentToReach = new Vector3();

    public class OnOrbitEvent : UnityEvent { }
    public OnOrbitEvent OnOrbitStartEvent { get; }   = new OnOrbitEvent();
    public OnOrbitEvent OnOrbitEndEvent { get; }     = new OnOrbitEvent();

    void OnDestroy()
    {
        ShipUnit.Instance.PropulsorComp.OnPropulseEndEvent.RemoveListener(OnPropulsedEndEvent);
    }

    // Start is called before the first frame update
    void Start()
    {
        ShipUnit.Instance.PropulsorComp.OnPropulseEndEvent.AddListener(OnPropulsedEndEvent);
    }

    private void OnPropulsedEndEvent()
    {
        if(OrbitalState == EOrbitalState.InInnerRadius)
        { 
            //OrbitalState = EOrbitalState.InOuterRadius;
            AttractionComponent AttractionComp = gameObject.GetComponentInParent<AttractionComponent>();
            AttractionComp.AttractedBy = null;
            MovingComponent MovingComp = gameObject.GetComponentInParent<MovingComponent>();
            MovingComp.enabled = true;

            OnOrbitEndEvent.Invoke();
            //Vector3 PlanetToShipVector = gameObject.transform.parent.position - Planet.transform.position;
            //gameObject.transform.parent.forward = Vector3.Cross(PlanetToShipVector, Vector3.up);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Planet)
        Debug.DrawLine(Planet.transform.position, TangentToReach, Color.cyan);
        if( OrbitalState == EOrbitalState.WillCrash )
        {
            GameObject Ship = gameObject.transform.parent.gameObject;
            Vector3 AttractionDir = Planet.transform.position - Ship.transform.position;
            AttractionDir.Normalize();
            //Ship.transform.position = Ship.transform.position + (AttractionDir * Planet.GetComponent<AttractionComponent>().AttractionForceCoefficient * Time.deltaTime);

            Debug.DrawLine(Ship.transform.position, Ship.transform.forward * 100, Color.green);
            Ship.transform.forward = Vector3.RotateTowards(Ship.transform.forward, AttractionDir, 0.2f * Planet.GetComponent<AttractionComponent>().AttractionForceCoefficient * Time.deltaTime, 100.0f);
            Debug.Log("OrbitalState: " + OrbitalState.ToString());
        }
        else if( OrbitalState == EOrbitalState.InOuterRadius )
        {
            GameObject Ship = gameObject.transform.parent.gameObject;
            Vector3 AttractionDir = TangentToReach - Ship.transform.position;
            AttractionDir.Normalize();
            Ship.transform.forward = Vector3.RotateTowards(Ship.transform.forward, AttractionDir, 0.2f * Planet.GetComponent<AttractionComponent>().AttractionForceCoefficient * Time.deltaTime, 100.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.attachedRigidbody && other.attachedRigidbody.gameObject.CompareTag("Planet") )
        {
            GameObject CollidingPlanet = other.attachedRigidbody.gameObject;
            SphereCollider SphereCol = other as SphereCollider;
            if(SphereCol)
            {
                if (SphereCol.CompareTag("OrbitalInnerRadius") && CollidingPlanet == Planet)
                {
                    if (ComputeWillCrash(Planet))
                    {
                        OrbitalState = EOrbitalState.WillCrash;
                    }
                    else
                    {
                        OrbitalState = EOrbitalState.InInnerRadius;

                        ComputeTrajectoryDistanceFrom(Planet);
                        AttractionComponent AttractionComp = gameObject.GetComponentInParent<AttractionComponent>();
                        AttractionComp.AttractedBy = other.attachedRigidbody.gameObject;
                        AttractionComp.OrbitCounterClockWise = WillBeCounterClockWiseOrbit;
                        Debug.Log("AttachTo: " + Planet.name + " CCW: " + AttractionComp.OrbitCounterClockWise.ToString());


                        MovingComponent MovingComp = gameObject.GetComponentInParent<MovingComponent>();
                        MovingComp.enabled = false;
                    }
                }
                else if ( SphereCol.CompareTag("OrbitalOuterRadius") && !Planet)
                {
                    Planet = CollidingPlanet;
                    if (ComputeWillCrash(Planet))
                    {
                        OrbitalState = EOrbitalState.WillCrash;
                    }
                    else
                    {
                        OrbitalState = EOrbitalState.InOuterRadius;

                        GameObject Ship = gameObject.transform.parent.gameObject;
                        Vector3 PerpShipToPlanetVector = ComputePerpShipToPlanetVector(Planet);
                        Planet.GetComponentsInChildren<SphereCollider>(Colliders);
                        SphereCollider InnerCollider = Colliders.Find(x => x.CompareTag("OrbitalInnerRadius"));
                        PerpShipToPlanetVector.Normalize();

                        float ForwardProjectionOnPerp = Vector3.Dot(gameObject.transform.parent.forward, PerpShipToPlanetVector);
                        bool Inv = ForwardProjectionOnPerp < 0.0f;

                        TangentToReach = Planet.transform.position + ( PerpShipToPlanetVector * InnerCollider.radius * Planet.transform.localScale.x) * (Inv ? -1 :1 );
                    }
                    MovingComponent MovingComp = gameObject.GetComponentInParent<MovingComponent>();
                    MovingComp.UseDeceleration = false;
                    OnOrbitStartEvent.Invoke();
                }
            }
        }
    }




    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.gameObject == Planet)
        {
            SphereCollider SphereCol = other as SphereCollider;
            if (SphereCol)
            {
                if (SphereCol.CompareTag("OrbitalOuterRadius"))
                {
                    WillBeCounterClockWiseOrbit = false;
                    OrbitalState = EOrbitalState.None;
                    Planet = null;
                    MovingComponent MovingComp = gameObject.GetComponentInParent<MovingComponent>();
                    MovingComp.UseDeceleration = true;
                }
            }
        }
    }

    Vector3 ComputePerpShipToPlanetVector(GameObject InPlanet)
    {
        return Vector3.Cross(InPlanet.transform.position - gameObject.transform.parent.position, Vector3.up);

    }

    float ComputeTrajectoryDistanceFrom(GameObject InPlanet)
    {
        Vector3 PerpShipToPlanetVector = ComputePerpShipToPlanetVector(InPlanet);
        float ForwardProjectionOnPerp = Vector3.Dot(gameObject.transform.parent.forward, PerpShipToPlanetVector);
        WillBeCounterClockWiseOrbit = ForwardProjectionOnPerp < 0.0f;
        float TrajDistanceFromPlanet = Mathf.Abs(ForwardProjectionOnPerp);
        return TrajDistanceFromPlanet;
    }

    bool ComputeWillCrash(GameObject InPlanet)
    {
        SphereCollider PlanetSphere = InPlanet.GetComponent<SphereCollider>();
        return ComputeTrajectoryDistanceFrom(InPlanet) < PlanetSphere.radius;
    }

    /*
    bool IsInOrbitRange(out GameObject OrbitalGameObj)
    {
        Vector3 Direction = gameObject.transform.forward;
        RaycastHit HitInfo;
        if( Physics.Raycast(transform.position, Direction, out HitInfo) )
        {
            GameObject Obj = HitInfo.collider.gameObject;
            OrbitalComponent OrbitComp = Obj.GetComponent<OrbitalComponent>();
            if(OrbitComp)
            {
                if( OrbitComp.GetOuterRadius() > HitInfo.distance )
                {

                }
            }
        }
    }*/
}
