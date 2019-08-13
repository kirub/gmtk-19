using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovingComponent))]
[RequireComponent(typeof(RotatorComponent))]
[RequireComponent(typeof(ChargerComponent))]
public class PropulsorComponent : MonoBehaviour, IDebugDrawable
{
	[SerializeField] private bool CanAlwaysPropulse = false;

	[SerializeField] private float MinPropulsionSpeed = 5f;
	[SerializeField] private float MaxPropulsionSpeed = 20f;
	[SerializeField] private float MaxPressedPropulsionTime = 2f;
	[SerializeField] private float _NeutralPropulsionRatio = 0.5f;
	[SerializeField] private float _GoodPropulsionRatio = 0.3f;

	[SerializeField] private float SlowTime = 0.5f;
	[SerializeField] private float SlowTimeSpeed = 2f;

	[SerializeField] private bool UseCameraShake = true;
	[SerializeField] private float CameraShakeTime = 2f;
	[SerializeField] private float PropulsingCameraShakeAmount = 2f;
	[SerializeField] private float MinCameraShakeAmount = 2f;
	[SerializeField] private float MaxCameraShakeAmount = 5f;

	[SerializeField] private GameObject ReactorParticle = null;
	[SerializeField] private GameObject ChargeParticle = null;
	[SerializeField] private ParticleSystem PropulsionParticle = null;
	[SerializeField] private float WaitTimeToReactivateRotator = 1f;

	[SerializeField] private AudioSource PropulsionChargeSound = null;
	[SerializeField] private AudioSource PropulsionImpulseSound = null;
	[SerializeField] private AudioSource PropulsionCancelSound = null;

	public float NeutralPropulsionThreshold { get { return _NeutralPropulsionRatio; } }
	public float GoodPropulsionThreshold { get { return NeutralPropulsionThreshold + _GoodPropulsionRatio; } }
	public float BadPropulsionThreshold { get { return 1f; } }

	public class OnCanPropulseEvent : UnityEvent { }
	public OnCanPropulseEvent OnCanPropulseStartEvent { get; } = new OnCanPropulseEvent();
	public OnCanPropulseEvent OnCanPropulseEndEvent { get; } = new OnCanPropulseEvent();

	public class OnPropulseEvent : UnityEvent { }
	public OnPropulseEvent OnPropulseStartEvent { get; } = new OnPropulseEvent();
	public OnPropulseEvent OnPropulseEndEvent { get; } = new OnPropulseEvent();
	public OnPropulseEvent OnPropulseCancelEvent { get; } = new OnPropulseEvent();
	
	private List<GameObject> NearComets = new List<GameObject>();
	private EnergySupplierComponent CurrentEnergySupplier = null;

	private float CurrentPressedPropulsionTime = -1f;
	private MovingComponent MovingComp = null;
	private RotatorComponent RotatorComp = null;
	private ChargerComponent ChargerComp = null;
	private ShipOrbitalComponent ShipOrbitalComp = null;
	private ShakeComponent CameraShakeComp = null;

	private float CurrentWaitTimeBeforeReactivatingRotator = -1f;

    public bool CanPropulse { get { return CanAlwaysPropulse || ChargerComp.CurrentNumRecharge > 0 || NearComets.Count > 0 || (ShipOrbitalComp && ShipOrbitalComp.OrbitalState == ShipOrbitalComponent.EOrbitalState.InInnerRadius); } }
    public bool IsPropulsing { get { return CurrentPressedPropulsionTime >= 0f; } }
	public bool IsValidPropulsion { get { return IsPropulsing && CurrentPropulsionRatio > NeutralPropulsionThreshold; } }
	public float CurrentPropulsionRatio { get { return IsPropulsing ? CurrentPressedPropulsionTime / MaxPressedPropulsionTime : 0f; } }

	EnergySupplierComponent GetLinkedEnergySupplier()
	{
		NearComets.RemoveAll(x => x == null);
		if (ShipOrbitalComp && ShipOrbitalComp.Planet && ShipOrbitalComp.OrbitalState == ShipOrbitalComponent.EOrbitalState.InInnerRadius)
		{
			return ShipOrbitalComp.Planet.GetComponent<EnergySupplierComponent>();
		}
		else if ( NearComets.Count > 0 )
		{
			return NearComets[0].GetComponent<EnergySupplierComponent>();
		}

		return null;
	}

	void UpdateEnergySupplier()
	{
		EnergySupplierComponent NewSupplier = GetLinkedEnergySupplier();

		if (CurrentEnergySupplier != NewSupplier)
		{
			CurrentEnergySupplier = NewSupplier;
			if (ChargerComp.IsRecharging)
			{
				ChargerComp.UpdateAvailability(CurrentEnergySupplier ? CurrentEnergySupplier.AvailableEnergy : 0);
			}
			else if (CurrentEnergySupplier)
			{
				ChargerComp.StartRecharge(CurrentEnergySupplier.AvailableEnergy);
			}
		}
	}

	void ResetPropulse()
	{
		if (CameraShakeComp)
		{
			CameraShakeComp.StopContinuousShakeCamera();
		}
		CurrentPressedPropulsionTime = -1f;
		Time.timeScale = 1f;

		// Reset slow-mo on soundtrack
		UpdateAudioAccordingToTimeScale(Time.timeScale);

		if (ChargeParticle)
		{
			ChargeParticle.SetActive(false);
		}
		if (PropulsionChargeSound)
		{
			PropulsionChargeSound.Stop();
		}

		ChargerComp.StopRecharge();
	}

	void CancelPropulse( bool PlaySound = true )
	{
		ResetPropulse();
		OnPropulseCancelEvent.Invoke();

		if (PlaySound && PropulsionCancelSound)
		{
			PropulsionCancelSound.Play();
		}
	}

	private void UpdateAudioAccordingToTimeScale(float timeScale) {
		if (!GameManager.Instance) return;

		GameManager.Instance.UpdateAudioAccordingToTimeScale(timeScale);
	}

	void StartPropulse()
	{
		if (CanPropulse)
		{
			CurrentPressedPropulsionTime = 0f;
			OnPropulseStartEvent.Invoke();

			if (UseCameraShake && CameraShakeComp)
			{
				CameraShakeComp.ContinuousShakeCamera(PropulsingCameraShakeAmount);
			}

			if (ChargeParticle)
			{
				ChargeParticle.SetActive(true);
			}
			if (ReactorParticle)
			{
				ReactorParticle.SetActive(true);
			}
			if (PropulsionChargeSound)
			{
				PropulsionChargeSound.Play();
			}

			UpdateEnergySupplier();
		}
	}
	
	void UpdatePropulse()
	{
		if (IsPropulsing)
		{
			CurrentPressedPropulsionTime += Time.unscaledDeltaTime;
			float newTimeScale = Time.timeScale - SlowTimeSpeed * Time.unscaledDeltaTime;
			if (newTimeScale > SlowTime)
			{
				Time.timeScale = newTimeScale;
			}
			else
			{
				Time.timeScale = SlowTime;
			}

			// Add a slow-mo effect on in-game audio
			UpdateAudioAccordingToTimeScale(Time.timeScale);

			if (CurrentPressedPropulsionTime > MaxPressedPropulsionTime)
			{
				OnCanPropulseEndEvent.Invoke();
				CancelPropulse();
				if (ShipUnit.Instance)
				{
					ShipUnit.Instance.Explode();
				}
				else
				{
					Debug.LogWarning("No Ship Instance found, will not be able to die");
				}
			}
			
			UpdateEnergySupplier();
		}
	}

	void EndPropulse()
	{
		if (IsValidPropulsion)
		{
			OnPropulseEndEvent.Invoke();

			if (PropulsionParticle)
			{
				PropulsionParticle.Play();
				if (WaitTimeToReactivateRotator > 0)
				{
					CurrentWaitTimeBeforeReactivatingRotator = WaitTimeToReactivateRotator;
					RotatorComp.enabled = false;
				}
			}

			if (PropulsionImpulseSound)
			{
				PropulsionImpulseSound.Play();
			}

			NearComets.RemoveAll(x => x == null);
			if (NearComets.Count > 0)
			{
				GameObject NearestComet = NearComets[0];
				NearComets.RemoveAt(0);

				Destroy(NearestComet);
			}
			else
			{
				ChargerComp.UseCharge();
			}

			float CameraShakeAmount = MinCameraShakeAmount + (MaxCameraShakeAmount - MinCameraShakeAmount) * CurrentPropulsionRatio;
			MovingComp.CurrentSpeed = MinPropulsionSpeed + (MaxPropulsionSpeed - MinPropulsionSpeed) * CurrentPropulsionRatio;

			if (RotatorComp.MeshRotated)
			{
				transform.rotation = RotatorComp.MeshRotated.rotation;
				RotatorComp.MeshRotated.localRotation = Quaternion.identity;
			}

			if (UseCameraShake && CameraShakeComp)
			{
				CameraShakeComp.ShakeCamera(CameraShakeTime, CameraShakeAmount);
			}
		}
		else if (IsPropulsing)
		{
			CancelPropulse();
		}

		ResetPropulse();
	}

	private void OnPause(bool IsPaused)
	{
		if (IsPropulsing)
		{
			CancelPropulse();
		}
		else
		{
			ResetPropulse();
		}
	}

	private void Awake()
	{
		MovingComp = GetComponent<MovingComponent>();
		RotatorComp = GetComponent<RotatorComponent>();
		ChargerComp = GetComponent<ChargerComponent>();
		ShipOrbitalComp = GetComponentInChildren<ShipOrbitalComponent>();

		if (ChargeParticle)
		{
			ChargeParticle.SetActive(false);
		}
		if (ReactorParticle)
		{
			ReactorParticle.SetActive(false);
		}
	}

	private void Start()
	{
		CameraShakeComp = FindObjectOfType<ShakeComponent>();

		if (GameManager.Instance)
		{
			GameManager.Instance.OnPauseUnpauseEvent.AddListener(OnPause);
		}
	}

	private void OnDestroy()
	{
		if (GameManager.Instance)
		{
			GameManager.Instance.OnPauseUnpauseEvent.RemoveListener(OnPause);
		}

		OnCanPropulseEndEvent.Invoke();
		CancelPropulse(false);
	}

	private void Update()
	{
		if (GameManager.Instance && GameManager.Instance.IsInPause)
		{
			return;
		}

		if (CurrentWaitTimeBeforeReactivatingRotator > 0f)
		{
			CurrentWaitTimeBeforeReactivatingRotator -= Time.deltaTime;
			if (CurrentWaitTimeBeforeReactivatingRotator < 0f)
			{
				RotatorComp.enabled = true;
				if (ReactorParticle)
				{
					ReactorParticle.SetActive(false);
				}
			}
		}

		if ( IsPropulsing && !CanPropulse )
		{
			CancelPropulse();
		}

		if ( Input.GetKeyDown( KeyCode.Space ) )
		{
			StartPropulse();
		}

		if ( Input.GetKey( KeyCode.Space ) )
		{
			UpdatePropulse();
		}
		
		if ( Input.GetKeyUp( KeyCode.Space ) )
		{
			EndPropulse();
		}
	}

	GameObject GetCometFromCollider(Collider other)
	{
		if (other.CompareTag("Comet"))
		{
			return other.gameObject;
		}
		else if (other.CompareTag("DynamicObjectTrigger"))
		{
			CometComponent ChildComp = other.gameObject.GetComponentInChildren<CometComponent>();
			if (ChildComp)
			{
				return ChildComp.gameObject;
			}
		}

		return null;
	}

	private void OnTriggerEnter(Collider other)
	{
		GameObject Comet = GetCometFromCollider(other);

		if (Comet)
		{
			NearComets.Add(Comet);
			if (NearComets.Count == 1)
			{
				OnCanPropulseStartEvent.Invoke();
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		GameObject Comet = GetCometFromCollider(other);

		if (Comet)
		{
			NearComets.Remove(Comet);
			if (NearComets.Count == 0)
			{
				OnCanPropulseEndEvent.Invoke();
			}
		}
	}

	public void DebugDraw(ref Rect BasePos, float TextYIncrement, GUIStyle Style)
	{
#if UNITY_EDITOR
		GUI.Label(BasePos, "- " + (IsPropulsing ? "Propulsing " + (100f * CurrentPropulsionRatio).ToString("F2") : "Not propulsing..."), Style);
		BasePos.y += TextYIncrement;
		GUI.Label(BasePos, "- Time scale " + Time.timeScale, Style);
		BasePos.y += TextYIncrement;
		GUI.Label(BasePos, "- " + NearComets.Count + " comets around", Style);
		BasePos.y += TextYIncrement;
		
		if (CurrentEnergySupplier)
		{
			GUI.Label(BasePos, "- Energy supplier " + CurrentEnergySupplier.name, Style);
		}
		else
		{
			GUI.Label(BasePos, "- No energy supplier around", Style);
		}
		
		if (WaitTimeToReactivateRotator > 0f)
		{			
			GUI.Label(BasePos, "- Wait time before rotator " + WaitTimeToReactivateRotator.ToString("F2") + "s", Style);
			BasePos.y += TextYIncrement;
		}
#endif
	}
}
