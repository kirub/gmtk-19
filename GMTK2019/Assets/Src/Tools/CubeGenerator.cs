using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
	[SerializeField] private int NumCubesToGenerate = 100;
	[SerializeField] private float MinCubeSize = 1f;
	[SerializeField] private float MaxCubeSize = 1f;
	
	[SerializeField] private float MinX = -500f;
	[SerializeField] private float MaxX = 500f;
	[SerializeField] private float MinZ = -500f;
	[SerializeField] private float MaxZ = 500f;
	[SerializeField] private float MinY = -10f;
	[SerializeField] private float MaxY = -10f;

	[SerializeField] private bool GenerateOnStart = true;

	public void Generate()
	{
		if (!ShipUnit.Instance)
		{
			Debug.LogWarning("Can't generate cubes without ship");
			return;
		}

		for ( int c = 0; c < NumCubesToGenerate; ++c )
		{
			float x = Random.Range(MinX, MaxX);
			float y = Random.Range(MinY, MaxY);
			float z = Random.Range(MinZ, MaxZ);
			float scale = Random.Range(MinCubeSize, MaxCubeSize);

			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.position = ShipUnit.Instance.transform.position + new Vector3(x, y, z);
			cube.transform.localScale = new Vector3(scale, scale, scale);
			cube.transform.SetParent(transform);
		}
	}

	private void Start()
	{
		if (GenerateOnStart)
		{
			Generate();
		}
	}
}
