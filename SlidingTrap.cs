using System;
using System.Collections.Generic;
using UnityEngine;

public class SlidingTrap : MonoBehaviour {

    private GameObject slantFloor;                // floor that will rotate (z direction) and go down
    private GameObject rocks;                     // rocks on the slantFloor
    private GameObject player;                    // player within a given prefab
    private GameObject controller;                // controller (hand) of a player
    private GameObject wall;                      // the wall at the end
    private GameObject rope;                      // rope on the wall
    private PlayerMovement pm;                    // player movement
    //private bool runMode;                       // toggle run
    private bool allToArea;                       // boolean that dictates dynamic and static friction on slantFloor
    public bool zRotation;                        // angle of slantFloor
    public bool yDirection;                       // y-axis of slantFloor
    public bool touchRope;
    private Vector3 controllerRotation;
    private bool invoke_once = false;
    private bool trigger_stay = false;


    private int numPlayers;
    private int totPlayers;

    public int playerCount;

    /*public bool makeMove = false;

    void moveDelay()
    {
        makeMove = false; zRotation = false; yDirection = false;
    }*/

    // Use this for initialization
    void Start () {
        
        // initialize variables
        player = GameObject.Find("Player");
        pm = player.transform.Find("OVRCameraRig").GetComponent<PlayerMovement>();
        rocks = transform.parent.Find("Rocks").gameObject;
        //Destroy(rocks);
        //player.GetComponent<PlayerMovement>().running = false;
        playerCount = 0;
        slantFloor = transform.parent.Find("Slant Floor").gameObject;
        //slantFloor.transform.position = Vector3.zero;
        allToArea = false;
        zRotation = false;
        yDirection = false;
        touchRope = false;
        wall = GameObject.Find("/SlidingTrap/Wall");
        rope = GameObject.Find("/SlidingTrap/Climbable Vine/Rope");
        //controller = player.transform.Find("OVRCameraRig").transform.Find("TrackingSpace").transform.Find("RightHandAnchor").gameObject;
        controller = GameObject.Find("/Player/OVRCameraRig/TrackingSpace/RightHandAnchor").gameObject;
        // hard-code dynamic and static friction to be 1 for all 10x10
        foreach (Transform child in slantFloor.transform)
        {
            foreach (Transform floor in child)
            {
                floor.GetComponent<BoxCollider>().sharedMaterial.staticFriction = 1;
                floor.GetComponent<BoxCollider>().sharedMaterial.dynamicFriction = 1;
            }
        }

    }

	// Update is called once per frame
	void Update () {

        //if (makeMove) { zRotation = true; yDirection = true; Invoke("moveDelay", 1.5f); }

        //remote players
        var remotes = GameObject.FindGameObjectsWithTag("Remote");

        totPlayers = remotes.Length;

        numPlayers = 0;
        foreach (GameObject remote in remotes)
        {
            if (Math.Abs(remote.transform.position.x - transform.position.x) < 25.25f && Math.Abs(remote.transform.position.z - transform.position.z) < 5.25f &&
                Math.Abs(remote.transform.position.y - transform.position.y) < 3f)
            {
                numPlayers = numPlayers + 1;
                //rocks = transform.parent.Find("Rocks").gameObject;
                //yDirection = true;
                //zRotation = true;
                //rocks.SetActive(true);
                //rope.GetComponent<MeshRenderer>().enabled = true;
                //flag = true;

            }
        }
        
        
        // change to 4 after whatever
        if (playerCount >= 1)
        {
            Debug.Log("playerCount = " + playerCount);
            // raise boolean flags
            allToArea = true;
            //yDirection = true;
            //zRotation = true;

            // toggle player to run at all times
            //player.GetComponent<PlayerMovement>().running = true;
            var moveScript = player.transform.Find("OVRCameraRig");
            moveScript.GetComponent<PlayerMovement>().running = true;

        }
        else
        {
            //player.GetComponent<Rigidbody>().mass = 1;
            allToArea = false;
        }

        // this branch does nothing for some reason, moved this part to [Invoke Branch]
        if (allToArea)
        {
            Debug.Log("allToArea = " + allToArea);
            foreach (Transform child in slantFloor.transform)
            {
                foreach (Transform floor in child)
                {
                    floor.GetComponent<BoxCollider>().sharedMaterial.staticFriction = 0;
                    floor.GetComponent<BoxCollider>().sharedMaterial.dynamicFriction = 0;
                }
            }

            // keep player running at all times
            //player.GetComponent<PlayerMovement>().running = true;
            pm.running = true;
            //float minRotation = 0f;
            //float maxRotation = 30f;

            /*
            if (!touchRope) {
              if (controller.transform.localEulerAngles.y > 90 &&
                  controller.transform.localEulerAngles.y < 270) {
                    pm.enabled = false;
                    pm.running = false;
                    //player.GetComponent<PlayerMovement>().enabled = false;
                    //player.GetComponent<PlayerMovement>().running = false;
                  }
              else {
                    pm.enabled = true;
                    pm.running = true;
                //player.GetComponent<PlayerMovement>().enabled = true;
                //player.GetComponent<PlayerMovement>().running = true;
              }
            }
            */
        }
        //else
          //  player.GetComponent<PlayerMovement>().running = false;

        if (yDirection)
        {
            //Debug.Log("yDirection = " + yDirection);

           slantFloor.transform.position = new Vector3(slantFloor.transform.position.x,
                                        slantFloor.transform.position.y - 0.01f,
                                        slantFloor.transform.position.z);
            rocks.transform.position = new Vector3(rocks.transform.position.x,
                                                   rocks.transform.position.y - 0.01f,
                                                   rocks.transform.position.z);
                                                   
        }

        if (zRotation)
        {
            //Debug.Log("zRotation = " + zRotation);
            slantFloor.transform.localEulerAngles = new Vector3(slantFloor.transform.localEulerAngles.x,
                                            slantFloor.transform.localEulerAngles.y,
                                            slantFloor.transform.localEulerAngles.z - 0.12f);
            rocks.transform.localEulerAngles = new Vector3(rocks.transform.localEulerAngles.x,
                                               rocks.transform.localEulerAngles.y,
                                               rocks.transform.localEulerAngles.z - 0.125f);

            /*slantFloor.transform.localEulerAngles = new Vector3(slantFloor.transform.localEulerAngles.x,
                                                slantFloor.transform.localEulerAngles.y,
                                                slantFloor.transform.localEulerAngles.z - 0.01f);
            rocks.transform.localEulerAngles = new Vector3(rocks.transform.localEulerAngles.x,
                                                           rocks.transform.localEulerAngles.y,
                                                           rocks.transform.localEulerAngles.z - 0.01f);*/
        }

        /*if (slantFloor.transform.position.y <= -.01f || slantFloor.transform.localEulerAngles.z <= -1)
        {
            yDirection = false;
            zRotation = false;
        }*/

        
        if (yDirection && zRotation && !invoke_once)
        {
            Debug.Log("[Invoke Slant Floor Branch]");
            Invoke("slantFloorDelay", 6.2f); // 6f, 8f
            invoke_once = true;

            Debug.Log("allToArea = " + allToArea);
            foreach (Transform child in slantFloor.transform)
            {
                foreach (Transform floor in child)
                {
                    floor.GetComponent<BoxCollider>().sharedMaterial.staticFriction = 0;
                    floor.GetComponent<BoxCollider>().sharedMaterial.dynamicFriction = 0;
                }
            }
            pm.running = true;
        }

        // stop player running if near Rope
        if (player.transform.position.x == rope.transform.position.x) {
          //player.GetComponent<PlayerMovement>().running = false;
          touchRope = true;
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            if ((numPlayers == totPlayers) && !trigger_stay)
            {
                Debug.Log("OnTriggerStay");
                rocks = transform.parent.Find("Rocks").gameObject;
                yDirection = true;
                zRotation = true;
                //rocks.SetActive(true);
                trigger_stay = true;
                var remotes = GameObject.FindGameObjectsWithTag("Remote");

                numPlayers = 0;

                foreach (GameObject remote in remotes)
                {
                    if (Math.Abs(remote.transform.position.x - transform.position.x) < 25.25f && Math.Abs(remote.transform.position.z - transform.position.z) < 2.75f &&
                        Math.Abs(remote.transform.position.y - transform.position.y) < 3f)
                    {
                        numPlayers = numPlayers + 1;
                    }
                }

                totPlayers = remotes.Length;
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("OnTriggerEnter");
            player = GameObject.Find("Player");
            //Destroy(controller); //<-- this line works
            //controllerRotation = controller.transform.localEulerAngles;
            //controllerRotation = new Vector3(180, 180, 180);
            //controller.transform.localEulerAngles = new Vector3(180, 180, 180);
            //controller.transform.position = new Vector3(20, 20, 20);
            // set rocks active
            

            // set rope SetActive
            //rope = transform.parent.Find("Climbable Vine").gameObject;
            //rope.SetActive(true);
            //rope.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    void slantFloorDelay()
    {
        yDirection = false;
        zRotation = false;
    }
}
