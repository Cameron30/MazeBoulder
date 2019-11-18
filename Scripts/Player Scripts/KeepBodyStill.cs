using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepBodyStill : MonoBehaviour {

    public GameObject body;
    public GameObject camera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (camera.transform.eulerAngles.y < body.transform.eulerAngles.y - 90 || camera.transform.eulerAngles.y > body.transform.eulerAngles.y + 90)
        {
            //TODO set a boolean to start turning then turn all  the way

            body.transform.eulerAngles = new Vector3(0, (System.Math.Sign(camera.transform.eulerAngles.y - body.transform.eulerAngles.y)) * Time.deltaTime, 0);
        }
        
	}
}
