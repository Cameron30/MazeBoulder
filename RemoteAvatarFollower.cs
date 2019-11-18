using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteAvatarFollower : MonoBehaviour {

    private GameObject col;
	// Use this for initialization
	void Start () {

        GameObject col = (GameObject)Instantiate(Resources.Load("RemoteCollider"));
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        col.transform.position = transform.position;
	}
}
