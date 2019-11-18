using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_trap : MonoBehaviour {
    [SerializeField] float movingSpeed = 0.1f;

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position.Set(transform.position.x, transform.position.y + movingSpeed, transform.position.z);

    }
}
