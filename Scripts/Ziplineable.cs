using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ziplineable : MonoBehaviour
{

    public bool climbing; // was private
    private bool spawnFlag;
    private GameObject player;
    private GameObject controller;

    public bool posFlag;
    public float totalDistance;
    private bool flag;
    private bool flag2;
    //private bool prox;

    // Use this for initialization
    void Start()
    {
        // acquires the properties of the player for transformations
        player = GameObject.Find("Player");

        // get the input from the left or right controller
        ControllerInput();

        // get rid of the collider box of the hook
        Destroy(transform.parent.GetComponent<Collider>());

        // get rid of the placeable script in the grappling hook
        Destroy(transform.parent.parent.GetComponent<Placeable>());

        // spawn in the grappling hook only once
        if (!spawnFlag)
        {
            SpawnGun();
            spawnFlag = true;
        }
        else if (spawnFlag)
        {
            CancelInvoke("Grappling Gun");
        }

        var grapple = transform.parent.parent;
        /*
        var rope = transform.parent.parent.GetComponent<GrapplingHook>().cylinderRope;
        var distanceToHook = transform.parent.parent.GetComponent<GrapplingHook>().distanceToHook;
        float scaleFactor = distanceToHook * 0.0335f;
        rope.transform.localScale = new Vector3(0.99f, scaleFactor * 9f, 0.9f);
        */
        // Ignore collision between player and rope
        Physics.IgnoreCollision(player.transform.GetComponent<Collider>(), transform.GetComponent<Collider>());
        Physics.IgnoreCollision(player.transform.GetComponent<Collider>(), grapple.transform.GetComponent<Collider>());

        flag = false;
        flag2 = false;
        //prox = false;
        posFlag = false;
    }

    void SpawnGun()
    {
        // spawn in the new grappling hook
        var oldGrapple = transform.parent.parent;

        GameObject newGrapple = (GameObject)Instantiate(Resources.Load("Grappling Gun"));
        //newGrapple.GetComponent<GrapplingHook>().newGun = true;

        // sets the new grappling hook's position, rotation and parent relation
        newGrapple.transform.position = oldGrapple.transform.position;
        newGrapple.transform.rotation = oldGrapple.transform.rotation;

        var held = oldGrapple.GetComponent<GrapplingHook>().held;

        if (held)
        {
            controller = GameObject.Find("Player/OVRCameraRig/TrackingSpace/RightHandAnchor");
            newGrapple.transform.parent = controller.transform;
            controller.GetComponent<Grabber>().grabbed = true;
            controller.GetComponent<Grabber>().grabbedObject = newGrapple;
        }
        //transform.parent.parent = null;
        //Destroy(oldGrapple);
    }

    void ControllerInput()
    {
        // right controller
        if (OVRInput.GetActiveController() == OVRInput.Controller.RTrackedRemote)
        {
            controller = player.transform.Find("OVRCameraRig").transform.Find("TrackingSpace").transform.Find("RightHandAnchor").gameObject;
        }
        // left controller
        else
        {
            controller = player.transform.Find("OVRCameraRig").transform.Find("TrackingSpace").transform.Find("LeftHandAnchor").gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        var rope = transform.parent.parent.GetComponent<GrapplingHook>().cylinderRope;
        rope.transform.position = new Vector3(rope.transform.position.x,
                                              rope.transform.position.y,
                                              rope.transform.position.z);
                                              */
                                              /*
        var rope = transform.parent.parent.GetComponent<GrapplingHook>().cylinderRope;
        var distanceToHook = transform.parent.parent.GetComponent<GrapplingHook>().distanceToHook;
        float scaleFactor = distanceToHook * 0.0335f;
        rope.transform.localScale = new Vector3(0.09f, scaleFactor, 0.09f);
        */
        // turn off player gravity
        //player.transform.GetComponent<Rigidbody>().useGravity = false;

        if (flag && !flag2)
        {
            var grappleHook = transform.parent.parent;
            if (grappleHook != null)
            {
                transform.parent.parent.DetachChildren();
                Destroy(grappleHook.gameObject);
            }
            flag2 = true;
        }
        else
        {
            flag = true;
        }

        // moves player in the opposite direction of controller movement
        if (climbing)
        {

            // variables for Vector3 Lerp function
            float speed = 1.5f;
            float speedValue = speed * 4f * Time.deltaTime;

            Vector3 hookDistance = transform.parent.position;

            float playerToHookDistance = Vector3.Distance(player.transform.position, hookDistance);

            float frac = speedValue / playerToHookDistance;

            var rig = player.transform.Find("OVRCameraRig");
            rig.GetComponent<PlayerMovement>().running = false;
            rig.GetComponent<PlayerMovement>().enabled = false;
            // turn off player gravity
            player.transform.GetComponent<Rigidbody>().useGravity = false;

            //if (!prox)
            //{
            //var moveScript = player.transform.Find("OVRCameraRig");

            //player.transform.position = Vector3.Lerp(player.transform.position, hookDistance, frac);
            if (Vector3.Distance(player.transform.position, transform.parent.position) > 2.2f)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, transform.parent.position, speedValue);
                //Debug.Log("Distance = " + Vector3.Distance(player.transform.position, transform.parent.position));
            }
            //}
            // add the player movement script when the player reaches the end of the rope within a certain threshold
            if (Vector3.Distance(player.transform.position, transform.parent.position) < 2.2f)
            {
                //Debug.Log("LET IT GOOOOOO");
                var moveScript = player.transform.Find("OVRCameraRig");
                moveScript.GetComponent<PlayerMovement>().enabled = true;

                // turn on player gravity
                player.transform.GetComponent<Rigidbody>().useGravity = true;

                //prox = true;
                climbing = false;
            }
        }

        
    }

    void OnTriggerStay(Collider other)
    {
       
        var grabber = other.gameObject.GetComponent<Grabber>();
        
        //var grabbed = grabber.grabbed;
        if (grabber != null)
        {
            var grabbed = grabber.grabbed;
            // Pressing Trigger
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)
                && !grabbed)
            {
                //Debug.Log("You've grabbed the thing");
                //prox = false;
                climbing = true;
            }
        }
    }
}
