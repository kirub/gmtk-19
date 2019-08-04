using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InnerSupernova : MonoBehaviour
{
	public Collider NovaCollider;

	public float TimerBeforeStart = 2f;
    public float ExpantionSpeed = 5f;

    public UnityEvent OnEnterInnerSupernova;

    private bool ExpantionIsOn = false;
    private Vector3 VScale;

	private void Awake()
	{
		NovaCollider.enabled = false;
	}

	void Start()
    {
        VScale.Set(ExpantionSpeed, 0, ExpantionSpeed);
        StartCoroutine(TimerExpantionStart());

		if (GameManager.Instance)
		{
			OnEnterInnerSupernova.AddListener(GameManager.Instance.GameOver);
		}
    }
    private IEnumerator TimerExpantionStart()
    {
        yield return new WaitForSeconds(TimerBeforeStart);
        ExpantionIsOn = true;
		NovaCollider.enabled = true;
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
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
            transform.localScale = transform.localScale + (VScale * Time.deltaTime);
    }
}
