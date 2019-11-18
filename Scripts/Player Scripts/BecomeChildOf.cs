using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BecomeChildOf : MonoBehaviour {

    public GameObject parent;
	// Use this for initialization
	void Start () {
        transform.parent = parent.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
