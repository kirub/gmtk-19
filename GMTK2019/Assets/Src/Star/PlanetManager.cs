using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public static float G   = 9.81f;
    const int MapHalfSize   = 1000;
    const int GridSize      = 10;
    const int CellSize      = (MapHalfSize * 2) / GridSize;
    Vector3 MapSizeOffset   = new Vector3();

    List<GameObject>[,] ObjectGrid = new List<GameObject>[GridSize, GridSize];

    public static PlanetManager Instance { get; private set; } = null;

    public ArrayList    Planets { get; }    = new ArrayList();
    private Vector2Int  PositionInGrid      = new Vector2Int();

    private class PlanetGridInfos
    {
        List<GameObject> Objects    = new List<GameObject>();
        Vector2Int CurrentPos       = new Vector2Int();

        public bool Check(Vector2Int InPos, out List<GameObject> InObjects)
        {
            InObjects = Objects;
            return CurrentPos == InPos;
        }

        public void UpdatePos(Vector2Int InPos)
        {
            CurrentPos = InPos;
        }
    }

    private PlanetGridInfos PlanetCache = new PlanetGridInfos();

    private void Awake()
    {
        for(int X = 0; X < GridSize; ++X)
        {
            for (int Y = 0; Y < GridSize; ++Y)
            {
                ObjectGrid[X, Y] = new List<GameObject>();
            }
        }

        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        MapSizeOffset.x = MapHalfSize;
        MapSizeOffset.z = MapHalfSize;

        List<OrbitalComponent> OrbitalComps = new List<OrbitalComponent>();
        GetComponentsInChildren<OrbitalComponent>(OrbitalComps);
        List<OrbitalComponent> PlanetsOrbitalComps = OrbitalComps.FindAll(x => x.CompareTag("Planet"));
        PlanetsOrbitalComps.ForEach(x => RegisterPlanet(x.gameObject));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> GetPlanetsInGridFromPosition( Vector3 Position, int Extent = 1, bool Cache = false)
    {
        List<GameObject> OutPlanets = null;
        GetPositionInGrid(Position, ref PositionInGrid);
        if (!PlanetCache.Check(PositionInGrid, out OutPlanets))
        {
            List<GameObject> CopyOldPlanetCache = new List<GameObject>(OutPlanets);
            if (!Cache)
                OutPlanets = new List<GameObject>();
            else
                OutPlanets.Clear();
            for (int X = PositionInGrid.x - Extent; X <= PositionInGrid.x + Extent; ++X)
            {
                for (int Y = PositionInGrid.y - Extent; Y <= PositionInGrid.y + Extent; ++Y)
                {
                    ObjectGrid[X, Y].ForEach(Planet =>
                    {
                        OutPlanets.Add(Planet);
                        CopyOldPlanetCache.Remove(Planet);
                        Planet.SetActive(true);
                    });
                }
            }

            PlanetCache.UpdatePos(PositionInGrid);
            CopyOldPlanetCache.ForEach(x => x.gameObject.SetActive(false));
        }

        return OutPlanets;
    }

    void GetPositionInGrid(Vector3 Position, ref Vector2Int PositionInGrid)
    {
        Vector3 OffsetPos = Position + MapSizeOffset;
        Debug.Assert(OffsetPos.x >= 0.0f && OffsetPos.z >= 0.0f);

        PositionInGrid.x = (int)OffsetPos.x / CellSize;
        PositionInGrid.y = (int)OffsetPos.z / CellSize;
    }

    void AddPlanetInGrid(GameObject Planet)
    {
        GetPositionInGrid(Planet.transform.position, ref PositionInGrid);
        ObjectGrid[PositionInGrid.x, PositionInGrid.y].Add(Planet);
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
        AddPlanetInGrid(Planet);
        Planet.SetActive(false);
        return AttractedByObj;
    }
}
