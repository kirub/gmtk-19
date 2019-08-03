using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InnerSupernova : MonoBehaviour
{
    public float TimerBeforeStart = 2f;
    public float ExpantionSpeed = 5f;

    public UnityEvent OnEnterInnerSupernova;

    private bool ExpantionIsOn = false;
    private Transform Novacore;
    private Vector3 VScale;
    private Manager M;

    void Start()
    {
        M = GameObject.FindObjectOfType<Manager>();
        Novacore = this.transform;
        VScale.Set(ExpantionSpeed, 0, ExpantionSpeed);
        StartCoroutine(TimerExpantionStart());
        OnEnterInnerSupernova.AddListener(FindObjectOfType<Manager>().GameOver);
    }
    private IEnumerator TimerExpantionStart()
    {
        yield return new WaitForSeconds(TimerBeforeStart);
        ExpantionIsOn = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        //je peux pas faire si other.tag=="player" ça passe pas ;'(
        if ((other.tag != "Comet" )&&(other.tag != "OrbitalOuterRadius") && (other.tag != "OrbitalInnerRadius") && (other.tag != "Untagged") && (other.tag != "Planet"))
        {
            Debug.Log("Enter Inner = gameover " + other.tag);
            OnEnterInnerSupernova.Invoke();
        }
        //Debug.Log("Enter Inner = gameover");
    }

    // Update is called once per frame
    void Update()
    {
        if (ExpantionIsOn)
            Novacore.localScale = Novacore.localScale + (VScale * Time.deltaTime);
    }
}
