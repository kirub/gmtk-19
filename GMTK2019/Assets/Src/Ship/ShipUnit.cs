using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovingComponent))]
[RequireComponent(typeof(RotatorComponent))]
[RequireComponent(typeof(PropulsorComponent))]
[RequireComponent(typeof(ChargerComponent))]
public class ShipUnit : MonoBehaviour, IDebugDrawable
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
	public ChargerComponent ChargerComp { get; private set; } = null;
	public ShipOrbitalComponent OrbitalComp { get; private set; } = null;

    public class OnExplodeEvent : UnityEvent { }
	public OnExplodeEvent OnExplodeShipEvent { get; } = new OnExplodeEvent();

	public void BoostOnOrbitLeft()
	{
		MovingComp.BoostSpeed = 1.2f;
	}

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
		ChargerComp = GetComponent<ChargerComponent>();
		OrbitalComp = GetComponentInChildren<ShipOrbitalComponent>();

		OrbitalComp.OnOrbitEndEvent.RemoveListener(BoostOnOrbitLeft);
	}

	void OnDestroy()
	{
		OrbitalComp.OnOrbitEndEvent.RemoveListener(BoostOnOrbitLeft);
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

	public void DebugDraw(ref Rect BasePos, float TextYIncrement, GUIStyle Style)
	{
#if UNITY_EDITOR
		if (CurrentP1DisplayedTime < P1DisplayedTime)
		{
			GUI.Label(BasePos, "- P1 Time Left : " + (P1DisplayedTime - CurrentP1DisplayedTime).ToString("F2") + "s", Style);
			BasePos.y += TextYIncrement;
		}
#endif
	}
}
