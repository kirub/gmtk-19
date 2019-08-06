using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingComponent : MonoBehaviour
{
	[SerializeField] private float BaseMovingSpeed = 1f;
	[SerializeField] private float MinMovingSpeed = 1f;
	[SerializeField] private float MaxMovingSpeed = 1f;
	[SerializeField] private float Deceleration = 1f;

    public float BoostSpeed { get; set; } = 1f;
    public float CurrentSpeed { get; set; } = 0f;
    public float MinMovingSpeedValue { get { return MinMovingSpeed; } }
    public float MaxMovingSpeed { get { return MaxMovingSpeed; } }
    public float DecelerationValue { get { return Deceleration; } }
	public bool UseDeceleration { get; set; } = true;

	private void Awake()
	{
		CurrentSpeed = BaseMovingSpeed;
		if (MaxMovingSpeed <= 0f)
		{
			MaxMovingSpeed = float.MaxValue;
		}
	}

	private void Update()
	{
		if (UseDeceleration)
		{
			CurrentSpeed -= Deceleration * Time.deltaTime;
		}
		CurrentSpeed = Mathf.Clamp( CurrentSpeed, MinMovingSpeed, MaxMovingSpeed ) * BoostSpeed;
		transform.position = transform.position + transform.forward * CurrentSpeed * Time.deltaTime;

        BoostSpeed = 1.0f;
    }
}
