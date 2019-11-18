using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crumblingwall : MonoBehaviour {

    private bool activateTrapCard;
    private PlayerMovement moveScript;
	// Use this for initialization
	void Start () {
        moveScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        activateTrapCard = false;
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Transform child in transform)
        {
            if (child.position.y < -10)
            {
                Destroy(child.gameObject);
            }
        }
        if (transform.childCount  <= 3)
        {
            moveScript.enabled = true;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            // disable player movement when stepping in the designated position.
            //var moveScript = other.gameObject.transform.Find("OVRCameraRig");
            other.GetComponent<PlayerMovement>().running = false;
            other.GetComponent<PlayerMovement>().enabled = false;
            // NOT SO FAST! REVERSE CARD OPEN
            activateTrapCard = true;
            if (activateTrapCard) // when the player steps in the trigger
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                }
                foreach (Transform child in transform)
                {
                    child.gameObject.GetComponent<BoxCollider>().isTrigger = true;
                    child.gameObject.GetComponent<Rigidbody>().AddForce(0, 0, 1000);
                }
                foreach (Transform child in transform)
                {
                    child.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                }

            }
        }
    }
}
