using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorComponent : MonoBehaviour
{
	[SerializeField] private float RotationSpeed = 1f;
	[SerializeField] private Transform MeshContainer = null;

	public Transform MeshRotated { get { return MeshContainer; } }

	private void Awake()
	{
		if (!MeshRotated)
		{
			Debug.LogError("No MeshContainer set on " + this + "RotatorComponent");
		}
	}

	private void Update()
	{
		MeshContainer.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
	}
}
