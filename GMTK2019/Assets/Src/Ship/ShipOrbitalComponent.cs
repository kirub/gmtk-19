using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipOrbitalComponent : MonoBehaviour, IDebugDrawable
{
    public enum EOrbitalState
    {
        None,
        InOuterRadius,
        InInnerRadius,
        InAttractorField,
        WillCrash
    }

    public GameObject       Planet { get; private set; } = null;
    public EOrbitalState    OrbitalState { get; private set; } = EOrbitalState.None;

    private bool            WillBeCounterClockWiseOrbit { get; set; } = false;
    private List<SphereCollider> Colliders = new List<SphereCollider>();
    private Vector3 TangentToReach      = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 DebugPlanetPosition = new Vector3();
    private Vector3 DebugShipPosition = new Vector3();

    public class OnOrbitEvent : UnityEvent { }
    public OnOrbitEvent OnOrbitStartEvent { get; }   = new OnOrbitEvent();
    public OnOrbitEvent OnOrbitEndEvent { get; }     = new OnOrbitEvent();
	
    void Start()
    {
        ShipUnit.Instance.PropulsorComp.OnPropulseEndEvent.AddListener(OnPropulsedEndEvent);
		
		DebugDrawHelper.RegisterDrawable(transform.parent ? transform.parent.gameObject : gameObject, this);
	}

    void OnDestroy()
    {
		DebugDrawHelper.UnregisterDrawable(transform.parent ? transform.parent.gameObject : gameObject, this);

		ShipUnit.Instance.PropulsorComp.OnPropulseEndEvent.RemoveListener(OnPropulsedEndEvent);
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
        //Debug.DrawLine(DebugPlanetPosition, TangentToReach, Color.cyan);
        //Debug.DrawLine(DebugShipPosition, DebugPlanetPosition, Color.red);

        if ( OrbitalState == EOrbitalState.WillCrash || OrbitalState == EOrbitalState.InAttractorField)
        {
            //Debug.Log("OrbitalState: " + OrbitalState.ToString());
            GameObject Ship = gameObject.transform.parent.gameObject;
            Vector3 AttractionDir = Planet.transform.position - Ship.transform.position;
            AttractionDir.Normalize();
            //Ship.transform.position = Ship.transform.position + (AttractionDir * Planet.GetComponent<AttractionComponent>().AttractionForceCoefficient * Time.deltaTime);

            //Debug.DrawLine(Ship.transform.position, Ship.transform.forward * 100, Color.green);
            Ship.transform.forward = Vector3.RotateTowards(Ship.transform.forward, AttractionDir, 0.05f * Planet.GetComponent<AttractionComponent>().AttractionForceCoefficient * Time.deltaTime, 100.0f);

            if (OrbitalState == EOrbitalState.InAttractorField)
            {
                Ship.transform.position = Ship.transform.position + (AttractionDir * Planet.GetComponent<AttractionComponent>().AttractionForceCoefficient * Time.deltaTime);
            }
        }
        else if( OrbitalState == EOrbitalState.InOuterRadius )
        {
            GameObject Ship = gameObject.transform.parent.gameObject;
            Vector3 AttractionDir = TangentToReach - Ship.transform.position;
            AttractionDir.Normalize();
            Ship.transform.forward = Vector3.RotateTowards(Ship.transform.forward, AttractionDir, 0.2f /*Mathf.Lerp(0.2f, 1.0f, ShipUnit.Instance.MovingComp.CurrentSpeed)*/ * Planet.GetComponent<AttractionComponent>().AttractionForceCoefficient * Time.deltaTime, 100.0f);
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
                        if (AttractionComp)
                        {
                            AttractionComp.AttractedBy = other.attachedRigidbody.gameObject;
                            AttractionComp.OrbitCounterClockWise = WillBeCounterClockWiseOrbit;
                        }
                        //Debug.Log("AttachTo: " + Planet.name + " CCW: " + AttractionComp.OrbitCounterClockWise.ToString());//


                        MovingComponent MovingComp = gameObject.GetComponentInParent<MovingComponent>();
                        MovingComp.enabled = false;
                        OnOrbitStartEvent.Invoke();
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
                        DebugPlanetPosition = Planet.transform.position;
                        DebugShipPosition = gameObject.transform.parent.position;
                        Vector3 PerpShipToPlanetVector = ComputePerpShipToPlanetVector(Planet);
                        Planet.GetComponentsInChildren<SphereCollider>(Colliders);
                        SphereCollider InnerCollider = Colliders.Find(x => x.CompareTag("OrbitalInnerRadius"));
                        PerpShipToPlanetVector.Normalize();

                        float ForwardProjectionOnPerp = Vector3.Dot(gameObject.transform.parent.forward, PerpShipToPlanetVector);
                        bool Inv = ForwardProjectionOnPerp < 0.0f;
                        Vector3 ShipPos = gameObject.transform.parent.position;
                        Vector3 PlanetPos = Planet.transform.position;
                        float radius = InnerCollider.radius * Planet.transform.localScale.x;


                        float b = Mathf.Sqrt(Mathf.Pow(gameObject.transform.parent.position.x - Planet.transform.position.x, 2) + Mathf.Pow(gameObject.transform.parent.position.z - Planet.transform.position.z, 2));
                        float th = Mathf.Acos(radius / b);  // angle theta

                        float d = Mathf.Atan2(gameObject.transform.parent.position.z - Planet.transform.position.z, gameObject.transform.parent.position.x - Planet.transform.position.x);  // direction angle of point P from C
                        float d1 = d + th;  // direction angle of point T1 from C
                        float d2 = d - th;  // direction angle of point T2 from C

                        if (Inv)
                        {
                            TangentToReach.x = Planet.transform.position.x + radius * Mathf.Cos(d1);
                            TangentToReach.z = Planet.transform.position.z + radius * Mathf.Sin(d1);
                        }
                        else
                        {
                            TangentToReach.x = Planet.transform.position.x + radius * Mathf.Cos(d2);
                            TangentToReach.z = Planet.transform.position.z + radius * Mathf.Sin(d2);

                        }

                        //TangentToReach = Planet.transform.position + ( PerpShipToPlanetVector * InnerCollider.radius * Planet.transform.localScale.x) * (Inv ? -1 :1 );
                    }
                    MovingComponent MovingComp = gameObject.GetComponentInParent<MovingComponent>();
                    MovingComp.UseDeceleration = false;
                }
            }
        }
        else if (other.gameObject.CompareTag("Attractor"))
        {
            GameObject CollidingPlanet = other.attachedRigidbody.gameObject;
            Planet = CollidingPlanet;
            OrbitalState = EOrbitalState.InAttractorField;
        }
    }




    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.gameObject == Planet)
        {
            SphereCollider SphereCol = other as SphereCollider;
            if (SphereCol)
            {
                if (SphereCol.CompareTag("OrbitalOuterRadius") || SphereCol.CompareTag("Attractor"))
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
        return ComputeTrajectoryDistanceFrom(InPlanet) < PlanetSphere.radius * InPlanet.transform.localScale.x;
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

	public void DebugDraw(ref Rect BasePos, float TextYIncrement, GUIStyle Style)
	{
#if UNITY_EDITOR
		if (Planet)
		{
			GUI.Label(BasePos, "- Planet " + Planet.name, Style);
			BasePos.y += TextYIncrement;
		}
		else
		{
			GUI.Label(BasePos, "- No Planet...", Style);
			BasePos.y += TextYIncrement;
		}
		GUI.Label(BasePos, "- Orbital State " + OrbitalState, Style);
		BasePos.y += TextYIncrement;
#endif
	}
}
