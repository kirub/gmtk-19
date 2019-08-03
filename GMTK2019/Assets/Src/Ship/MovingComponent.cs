using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingComponent : MonoBehaviour
{
	[SerializeField] private float MinMovingSpeed = 1f;
	[SerializeField] private float MaxMovingSpeed = 1f;
	[SerializeField] private float Deceleration = 1f;

	public float CurrentSpeed { get; set; } = 0f;

	private void Update()
	{
		CurrentSpeed -= Deceleration * Time.deltaTime;
		CurrentSpeed = Mathf.Clamp( CurrentSpeed, MinMovingSpeed, MaxMovingSpeed );
		transform.Translate(transform.forward * CurrentSpeed * Time.deltaTime);
	}
}
