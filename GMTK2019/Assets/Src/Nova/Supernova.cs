using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Supernova : MonoBehaviour
{
    // Start is called before the first frame update

    public static Supernova Instance { get; private set; } = null;

    public Collider NovaCollider;

	public float TimerBeforeStart = 2f;
    public float ExpantionSpeed = 5f;

	public AudioSource ExplosionSound = null;

    public UnityEvent OnEnterOuterSupernova;
    public UnityEvent OnExitOuterSupernova;
	
    private bool ExpantionIsOn = false;
    private Vector3 VScale;
    private float DistancePlayerNovacore = 0;

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
	}

	void Start()
    {
		VScale.Set(ExpantionSpeed, 0, ExpantionSpeed);
        StartCoroutine(TimerExpantionStart());

		if (!ShipUnit.Instance)
		{
			Debug.LogError("No ShipUnit found !");
		}
    }

    private IEnumerator TimerExpantionStart()
    {
        yield return new WaitForSeconds(TimerBeforeStart);
        ExpantionIsOn = true;
		NovaCollider.enabled = true;

		if (ExplosionSound)
		{
			ExplosionSound.Play();
		}
	}

    public float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, ShipUnit.Instance.transform.position);
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
