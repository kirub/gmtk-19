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
	
	[SerializeField] private GameObject P1Particle = null;
	[SerializeField] private float P1DisplayedTime = 2f;

	[SerializeField] private GameObject ExplosionParticle = null;

	private ShakeComponent CameraShakeComp = null;
	private float CurrentP1DisplayedTime = 0f;

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

		if (P1Particle)
		{
			StartCoroutine(UpdateP1Visibility());
		}
	}

	private IEnumerator UpdateP1Visibility()
	{
		while (CurrentP1DisplayedTime < P1DisplayedTime)
		{
			CurrentP1DisplayedTime += Time.unscaledDeltaTime;
			yield return null;
		}

		P1Particle.SetActive(false);
	}
}
