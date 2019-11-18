using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideRope : MonoBehaviour {

	private GameObject player;
	private GameObject controller;
    private GameObject topRope;
    private GameObject rope;

    public bool climbingUp;
	//private bool climbingDown;
    private bool ropeTouch;

	public float moveCount;

	// Use this for initialization
	void Start () {

		// initialize variables
		player = GameObject.Find("Player");
		controller = GameObject.Find("/Player/OVRCameraRig/TrackingSpace/RightHandAnchor").gameObject;
        topRope = transform.parent.Find("TopRopeTrigger").gameObject;
        rope = transform.parent.Find("Rope").gameObject;

		//climbingDown = false;
		climbingUp = false;
        ropeTouch = false;
		moveCount = 0f;

	}

	// Update is called once per frame
	void FixedUpdate () {

		//var topRope = GameObject.Find("/SlidingTrap/Climbable Wall/TopRopeTrigger");
		//var topRope = transform.parent.Find("TopRopeTrigger");

		// climb up with controller up rotation
		if (climbingUp) {
			player.transform.GetComponent<Rigidbody>().useGravity = false;

            var rb = player.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // move player up in the y direction at a slow rate
            player.transform.position = new Vector3(player.transform.position.x,
													player.transform.position.y + 0.1f,
													player.transform.position.z);
			++moveCount;

			// stop movement once moveCount is incremented 10 times
			// total movement per climb = 1.0f
			if (moveCount == 10000f || player.transform.position.y > 4.2f) {
				climbingUp = false;
				moveCount = 0f;

                // reset values to eliminate player position errors
                var prb = player.GetComponent<Rigidbody>();
                prb.velocity = Vector3.zero;
                prb.angularVelocity = Vector3.zero;
            }
		}      
	}

    void OnTriggerEnter(Collider other)
    {

        // check for topOfRope collider
        /*if (other.name == "TopRopeTrigger")
        {
            //Debug.Log("player has reached top of rope, turning on gravity");
            //player.transform.GetComponent<Rigidbody>().useGravity = true;
            Debug.Log("TopRopeTrigger");
            climbingUp = false;
            ropeTouch = false;
            moveCount = 0f;
            player.transform.GetComponent<Rigidbody>().useGravity = true;
        }

        if ((other.name == "Player")
            && (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)))
        {
            var moveScript = player.transform.Find("OVRCameraRig");
            moveScript.GetComponent<PlayerMovement>().running = false;

            player.transform.GetComponent<Rigidbody>().useGravity = false;
            var rb = player.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Debug.Log("player gravity = " + player.transform.GetComponent<Rigidbody>().useGravity);
        }*/
    }


    
    
    void OnTriggerStay(Collider other) {

		var grabber = other.gameObject.GetComponent<Grabber>();

		if (grabber != null) {

			var grabbed = grabber.grabbed;

			// Pressing trigger (upward)
			if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)
					//&& (controller.transform.localEulerAngles.x <= 360 && controller.transform.localEulerAngles.x >= 181)
					&& !grabbed) {

						climbingUp = true;
                        ropeTouch = true;
						moveCount = 0f;

                // turn off running
                //player.transform.GetComponent<PlayerMovement>().running = false;
                var moveScript = player.transform.Find("OVRCameraRig");
                moveScript.GetComponent<PlayerMovement>().running = false;
                player.transform.GetComponent<Rigidbody>().useGravity = false;

                //Debug.Log("[Pressing trigger branch]");
                //Debug.Log("running = " + moveScript.GetComponent<PlayerMovement>().running);
                //Debug.Log("gravity = " + player.transform.GetComponent<Rigidbody>().useGravity);
            }
		}
	}
}
