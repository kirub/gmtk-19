using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Supernova : MonoBehaviour
{
    public const float DefaultExpantionSpeed = 5f;

    [SerializeField]
    List<float> TimersOffset    = new List<float>();
    [SerializeField]
    List<float> ExpansionSpeed  = new List<float>();

    private List<float> TriggersTimersOffset { get { return TimersOffset; } }
    private List<float> TriggersExpansionSpeed { get { return ExpansionSpeed; } }

    public static Supernova Instance { get; private set; } = null;

    public GameObject FXStartExplosion = null;

    
    public Collider NovaCollider;

	public float TimerOffsetBeforeStartForFX = 1f;
	public float TimerBeforeStart = 3f;

	public AudioSource ExplosionSound = null;

    public UnityEvent OnEnterOuterSupernova;
    public UnityEvent OnExitOuterSupernova;
	
    private bool ExpantionIsOn = false;
    private Vector3 VScale;

	private void Awake()
	{
        if (Instance)
        {
            Debug.LogError("Multiple Instances of Supernova");
            Destroy(this);
            return;
        }
        Instance = this;
        NovaCollider.enabled = false;
        FXStartExplosion.SetActive(false);
    }

	void Start()
    {
		VScale.Set(DefaultExpantionSpeed, 0, DefaultExpantionSpeed);
        StartCoroutine(TimerExpantionStart());
        if (!ShipUnit.Instance)
		{
			Debug.LogError("No ShipUnit found !");
		}
    }

    private IEnumerator TimerExpantionStart()
    {
        yield return new WaitForSeconds(TimerBeforeStart - TimerOffsetBeforeStartForFX);
        FXStartExplosion.SetActive(true);
        yield return new WaitForSeconds(TimerOffsetBeforeStartForFX/5.0f);
        if (ExplosionSound)
        {
            ExplosionSound.Play();
        }
        ExpantionIsOn = true;
        yield return new WaitForSeconds(4*TimerOffsetBeforeStartForFX / 5.0f);
		NovaCollider.enabled = true;
        StartCoroutine(LaunchTriggers());
    }

    public float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, ShipUnit.Instance.transform.position);
    }

    public IEnumerator LaunchTriggers()
    {
        while(TriggersTimersOffset.Count > 0)
        {
            yield return new WaitForSeconds(TriggersTimersOffset[0]);
            VScale.x = VScale.z = TriggersExpansionSpeed[0];
            
            TriggersTimersOffset.RemoveAt(0);
            TriggersExpansionSpeed.RemoveAt(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(ExpantionIsOn)
        {
            
            transform.localScale = transform.localScale + (VScale* Time.deltaTime);
		}

		if (!ShipUnit.Instance)
		{
			return;
		}
    }

    
}
