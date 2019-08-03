using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayerMove : MonoBehaviour
{
    public float Initspeed = 2;
    public float PushConstant = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(Initspeed, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(PushConstant, 0, 0);
        //this.gameObject.transform.Translate(speed*Time.fixedDeltaTime, 0, 0);
    }
}
