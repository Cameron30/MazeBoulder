using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour {

    public bool touchingPlayer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.name == "Player")
        {
            touchingPlayer = true;
            transform.parent.Find("BoulderTrigger").gameObject.GetComponent<BoulderTrigger>().playerDied
                = true;
        }
    }
}
