using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingComponent : MonoBehaviour
{
	[SerializeField] private float BaseMovingSpeed = 1f;
	[SerializeField] private float MinMovingSpeed = 1f;
	[SerializeField] private float MaxMovingSpeed = 1f;
	[SerializeField] private float Deceleration = 1f;

    private float BoostSpeed { get; set; } = 1f;
    public float CurrentSpeed { get; set; } = 0f;
	public bool UseDeceleration { get; set; } = true;

	private void Awake()
	{
		CurrentSpeed = BaseMovingSpeed;
	}

    void OnDestroy()
    {
        ShipUnit.Instance.OrbitalComp.OnOrbitEndEvent.RemoveListener(BoostOnOrbitLeft);
    }

    // Start is called before the first frame update
    void Start()
    {
        ShipUnit.Instance.OrbitalComp.OnOrbitEndEvent.AddListener(BoostOnOrbitLeft);
    }

    public void BoostOnOrbitLeft()
    {
        BoostSpeed = 1.2f;
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
