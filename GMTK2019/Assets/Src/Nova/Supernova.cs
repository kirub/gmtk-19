using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Supernova : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float TimerBeforeStart = 2f;
    public float ExpantionSpeed = 5f;
    public float NovaPullBaseStrengh = 0.5f;
    public float NovaPullStrenghIncreaseOverTime = 0.001f;

    public UnityEvent OnEnterOuterSupernova;
    public UnityEvent OnExitOuterSupernova;
	
    private bool ExpantionIsOn = false;
    private bool NovaPullIsOn = false;
    private Vector3 VScale;
    private float DistancePlayerNovacore = 0;
    private float NovaPullActualStrengh = 0;
	
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
	}

    //ajouter condition ou layer pour ne capter que le ship
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            OnEnterOuterSupernova.Invoke();
            NovaPullIsOn = true;
            NovaPullActualStrengh = NovaPullBaseStrengh;
            //Debug.Log("Enter outer");
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            OnExitOuterSupernova.Invoke();
            NovaPullIsOn = false;
            NovaPullActualStrengh = 0;
            //Debug.Log("Exit outer");
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

		if (NovaPullIsOn)
        {
            NovaPullActualStrengh += NovaPullStrenghIncreaseOverTime*Time.deltaTime;
            ShipUnit.Instance.MovingComp.CurrentSpeed = Mathf.Max(ShipUnit.Instance.MovingComp.MinSpeed, ShipUnit.Instance.MovingComp.CurrentSpeed - NovaPullActualStrengh * Time.deltaTime);
           // Debug.Log(NovaPullActualStrengh);
        }
        DistancePlayerNovacore = Vector3.Distance(transform.position, ShipUnit.Instance.transform.position);
        if (GameManager.Instance)
			GameManager.Instance.LatestScore = DistancePlayerNovacore*100;
    }

    
}
