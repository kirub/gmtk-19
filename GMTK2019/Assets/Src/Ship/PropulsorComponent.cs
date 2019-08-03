using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovingComponent))]
[RequireComponent(typeof(RotatorComponent))]
public class PropulsorComponent : MonoBehaviour
{
	[SerializeField] private bool CanAlwaysPropulse = false;

	[SerializeField] private float MinPropulsionSpeed = 5f;
	[SerializeField] private float MaxPropulsionSpeed = 20f;
	[SerializeField] private float MaxPressedPropulsionTime = 2f;
	[SerializeField] private float _GoodPropulsionRatio = 0.3f;
	[SerializeField] private float _BadPropulsionRatio = 0.2f;

	[SerializeField] private float SlowTime = 0.5f;
	[SerializeField] private float SlowTimeSpeed = 2f;

	[SerializeField] private bool UseCameraShake = true;
	[SerializeField] private float CameraShakeTime = 2f;
	[SerializeField] private float PropulsingCameraShakeAmount = 2f;
	[SerializeField] private float MinCameraShakeAmount = 2f;
	[SerializeField] private float MaxCameraShakeAmount = 5f;
	
	public float NeutralPropulsionRatio { get { return 1f - _GoodPropulsionRatio - _BadPropulsionRatio; } }
	public float GoodPropulsionRatio { get { return _GoodPropulsionRatio; } }
	public float BadPropulsionRatio { get { return _BadPropulsionRatio; } }

	public class OnCanPropulseEvent : UnityEvent { }
	public OnCanPropulseEvent OnCanPropulseStartEvent { get; } = new OnCanPropulseEvent();
	public OnCanPropulseEvent OnCanPropulseEndEvent { get; } = new OnCanPropulseEvent();

	public class OnPropulseEvent : UnityEvent { }
	public OnPropulseEvent OnPropulseStartEvent { get; } = new OnPropulseEvent();
	public OnPropulseEvent OnPropulseEndEvent { get; } = new OnPropulseEvent();
	public OnPropulseEvent OnPropulseCancelEvent { get; } = new OnPropulseEvent();

	[SerializeField] private List<GameObject> NearComets = new List<GameObject>();

	private float CurrentPressedPropulsionTime = -1f;
	private MovingComponent MovingComp = null;
	private RotatorComponent RotatorComp = null;
	private ShakeComponent CameraShakeComp = null;

	public bool CanPropulse { get { return CanAlwaysPropulse || NearComets.Count > 0; } }
	public bool IsPropulsing { get { return CurrentPressedPropulsionTime >= 0f; } }
	public float CurrentPropulsionRatio { get { return CurrentPressedPropulsionTime / MaxPressedPropulsionTime; } }

	void StopPropulse()
	{
		CameraShakeComp.StopContinuousShakeCamera();
		CurrentPressedPropulsionTime = -1f;
		Time.timeScale = 1f;
	}

	void CancelPropulse()
	{
		StopPropulse();
		OnPropulseCancelEvent.Invoke();
	}

	private void Awake()
	{
		MovingComp = GetComponent<MovingComponent>();
		RotatorComp = GetComponent<RotatorComponent>();
	}

	private void Start()
	{
		CameraShakeComp = FindObjectOfType<ShakeComponent>();
	}

	private void Update()
	{
		if ( IsPropulsing && !CanPropulse )
		{
			CancelPropulse();
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (CanPropulse)
			{
				CurrentPressedPropulsionTime = 0f;
				OnPropulseStartEvent.Invoke();

				if (UseCameraShake && CameraShakeComp)
				{
					CameraShakeComp.ContinuousShakeCamera(PropulsingCameraShakeAmount);
				}
			}
		}

		if ( Input.GetKey( KeyCode.Space ) )
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
			}
		}
		
		if ( Input.GetKeyUp( KeyCode.Space ) )
		{
			if (IsPropulsing)
			{
				OnPropulseEndEvent.Invoke();

				if (NearComets.Count > 0)
				{
					GameObject NearestComet = NearComets[0];
					NearComets.RemoveAt(0);

					Destroy(NearestComet);
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

			StopPropulse();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Comet"))
		{
			NearComets.Add(other.gameObject);
			if (NearComets.Count == 1)
			{
				OnCanPropulseStartEvent.Invoke();
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Comet"))
		{
			NearComets.Remove(other.gameObject);
			if (NearComets.Count == 0)
			{
				OnCanPropulseEndEvent.Invoke();
			}
		}
	}
}
