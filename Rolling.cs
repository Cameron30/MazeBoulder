using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rolling : MonoBehaviour {
    
    [SerializeField] float speed = 1.01f;
    [SerializeField] float rotateAboutZ = 20f;

    // Use this for initialization
    void Start () {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
        transform.Rotate(0, 0, rotateAboutZ * Time.deltaTime);

    }
}
