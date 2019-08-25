using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorComponent : MonoBehaviour, IDebugDrawable
{
	[SerializeField] private float RotationAcceleration = 1f;
	[SerializeField] private float RotationSpeed = 1f;
	[SerializeField] private Transform MeshContainer = null;

	public Transform MeshRotated { get { return MeshContainer; } }

	private float CurrentRotationSpeed = 0f;

	private void Awake()
	{
		if (!MeshRotated)
		{
			Debug.LogError("No MeshContainer set on " + this + "RotatorComponent");
		}
	}

	private void Start()
	{
		DebugDrawHelper.RegisterDrawable(gameObject, this);
	}

	private void OnDestroy()
	{
		DebugDrawHelper.UnregisterDrawable(gameObject, this);
	}

	private void Update()
	{
		CurrentRotationSpeed = Mathf.Min(RotationSpeed, CurrentRotationSpeed + RotationAcceleration * Time.deltaTime);
		MeshContainer.Rotate(Vector3.up, CurrentRotationSpeed * Time.deltaTime);
	}

	private void OnEnable()
	{
		CurrentRotationSpeed = 0f;
	}

	private void OnDisable()
	{
		CurrentRotationSpeed = 0f;
	}

	public void DebugDraw(ref Rect BasePos, float TextYIncrement, GUIStyle Style)
	{
#if UNITY_EDITOR
		GUI.Label(BasePos, "- Rotation Speed " + CurrentRotationSpeed + "/" + RotationSpeed, Style);
		BasePos.y += TextYIncrement;
#endif
	}
}
