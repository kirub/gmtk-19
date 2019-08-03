using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Supernova : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform PlayerTransform;
    public Rigidbody PlayerBody;
    
    public float TimerBeforeStart = 5f;
    public float ExpantionSpeed = 0.5f;
    public float NovaPullBaseStrengh = 0.5f;
    public float NovaPullStrenghIncreaseOverTime = 0.001f;

    public UnityEvent OnEnterOuterSupernova;
    public UnityEvent OnExitOuterSupernova;

    private Transform Novacore;
    private bool ExpantionIsOn = false;
    private bool NovaPullIsOn = false;
    private Vector3 VScale;
    private Manager M;
    private float DistancePlayerNovacore = 0;
    private float NovaPullActualStrengh = 0;

    void Start()
    {
        Novacore = this.transform;
        M = GameObject.FindObjectOfType<Manager>();
        VScale.Set(ExpantionSpeed, 0, ExpantionSpeed);
        StartCoroutine(TimerExpantionStart());
    }
    private IEnumerator TimerExpantionStart()
    {
        yield return new WaitForSeconds(TimerBeforeStart);
        ExpantionIsOn = true;
    }

    //ajouter condition ou layer pour ne capter que le ship
    public void OnTriggerEnter(Collider other)
    {
        OnEnterOuterSupernova.Invoke();
        NovaPullIsOn = true;
        NovaPullActualStrengh = NovaPullBaseStrengh;
        //Debug.Log("Enter outer");
    }
    public void OnTriggerExit(Collider other)
    {
        OnExitOuterSupernova.Invoke();
        NovaPullIsOn = false;
        NovaPullActualStrengh = 0;
        //Debug.Log("Exit outer");
    }
    // Update is called once per frame
    void Update()
    {
        if(ExpantionIsOn)
        {
            Novacore.localScale = Novacore.localScale + (VScale* Time.deltaTime);
            
        }
        if (NovaPullIsOn)
        {
            NovaPullActualStrengh += NovaPullStrenghIncreaseOverTime*Time.deltaTime;
            PlayerBody.AddForce((Novacore.position - PlayerTransform.position)* NovaPullActualStrengh);
           // Debug.Log(NovaPullActualStrengh);
        }
        DistancePlayerNovacore = Vector3.Distance(Novacore.position, PlayerTransform.position);
        M.LatestScore = DistancePlayerNovacore*100;
    }

    
}
