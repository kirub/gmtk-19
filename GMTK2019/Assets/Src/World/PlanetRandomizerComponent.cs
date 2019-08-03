using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRandomizerComponent : MonoBehaviour
{
    public List<GameObject>    PlanetPrefabs = new List<GameObject>();

    private void Awake()
    {
        int Index = Random.Range(0, PlanetPrefabs.Count);
        Instantiate(PlanetPrefabs[Index], transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
