using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimation : MonoBehaviour {

    private Animator thisAnim;
    private bool grabbed;
    private bool laserPointer;

	// Use this for initialization
	void Start () {
        thisAnim = transform.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        grabbed = transform.parent.parent.GetComponent<Grabber>().grabbed;
        laserPointer = transform.parent.parent.GetComponent<LaserPointer>().active;

        thisAnim.SetBool("Grabbing", grabbed);
        thisAnim.SetBool("LaserPointer", laserPointer);
    }
}
