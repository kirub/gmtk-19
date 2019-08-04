using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovingComponent))]
[RequireComponent(typeof(RotatorComponent))]
[RequireComponent(typeof(PropulsorComponent))]
public class ShipUnit : MonoBehaviour
{
	[SerializeField] private bool UseCameraShakeOnExplode = true;
	[SerializeField] private float ExplodeShakeTime = 2f;
	[SerializeField] private float ExplodeShakeAmount = 2f;
	
	[SerializeField] private GameObject ExplosionParticle = null;

	private ShakeComponent CameraShakeComp = null;

	public static ShipUnit Instance { get; private set; } = null;

	public PropulsorComponent PropulsorComp { get; private set; } = null;
	public MovingComponent MovingComp { get; private set; } = null;

	public class OnExplodeEvent : UnityEvent { }
	public OnExplodeEvent OnExplodeShipEvent { get; } = new OnExplodeEvent();

	public void Explode()
	{
		if (UseCameraShakeOnExplode && CameraShakeComp)
		{
			CameraShakeComp.ShakeCamera(ExplodeShakeTime, ExplodeShakeAmount);
		}
		if (ExplosionParticle)
		{
			Instantiate(ExplosionParticle, transform.position, transform.rotation);
		}

		OnExplodeShipEvent.Invoke();
		Destroy(transform.gameObject);
	}

	void Awake()
    {
		if (Instance)
		{
			Debug.LogError("Multiple Instances of ShipUnit");
			Destroy(this);
			return;
		}
		Instance = this;
		PropulsorComp = GetComponent<PropulsorComponent>();
		MovingComp = GetComponent<MovingComponent>();
	}

	private void Start()
	{
		CameraShakeComp = FindObjectOfType<ShakeComponent>();

		if (GameManager.Instance)
		{
			OnExplodeShipEvent.AddListener(GameManager.Instance.GameOver);
		}
	}
}
