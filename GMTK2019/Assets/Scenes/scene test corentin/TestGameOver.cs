using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimerStart());
    }
    private IEnumerator TimerStart()
    {
        yield return new WaitForSeconds(5);
        GameObject.FindObjectOfType<Manager>().GameOver();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
