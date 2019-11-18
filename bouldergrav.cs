using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This code was made for the purpose of altering gravity of the boulder
// without depending on BoulderTrigger
// while not referencing the player.
public class bouldergrav : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Boulder")
            other.GetComponent<Rigidbody>().useGravity = !other.GetComponent<Rigidbody>().useGravity;

    }
}
