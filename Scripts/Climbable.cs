using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour
{

    private bool climbingUp;
    private bool climbingDown;
    private bool spawnFlag;
    private GameObject player;
    private GameObject controller;

    public bool posFlag;
    public bool flag;
    public bool flag2;
    //public bool insidePit;
    public float totalDistance;
    public float moveCount;

    // Use this for initialization
    void Start()
    {


        // get rid of the placeable script in the grappling hook
        Destroy(transform.parent.parent.GetComponent<Placeable>());

        // get rid of the collider box of the hook
        Destroy(transform.parent.GetComponent<Collider>());

        // acquires the properties of the player for transformations
        player = GameObject.Find("Player");

        // turn off player gravity
        //player.transform.GetComponent<Rigidbody>().useGravity = false;

        // get the input from the left or right controller
        ControllerInput();
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



        climbingDown = false;
        climbingUp = false;

    }

    void SpawnGun()
    {
        // spawn in the new grappling hook
        GameObject oldGrapple = GameObject.Find("Grappling Gun");

        if (oldGrapple == null)
        {
            oldGrapple = GameObject.Find("Grappling Gun(Clone)");
        }
        GameObject newGrapple = (GameObject)Instantiate(Resources.Load("Grappling Gun"));

        // sets the new grappling hook's position, rotation and parent relation
        newGrapple.transform.position = oldGrapple.transform.position;
        newGrapple.transform.rotation = oldGrapple.transform.rotation;

        if (oldGrapple.transform.parent != null)
        {
            var controller = oldGrapple.transform.parent;
            newGrapple.transform.parent = controller.transform;
            controller.GetComponent<Grabber>().grabbed = true;
            controller.GetComponent<Grabber>().grabbedObject = newGrapple;
        }
        transform.parent = null;
        Destroy(oldGrapple);
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


        // fix the rope's angle to always be straight up
        var rope = transform.parent.parent.GetComponent<GrapplingHook>().cylinderRope;

        // re-position the rope based on hook position
        //var distance = transform.parent.parent.GetComponent<GrapplingHook>().distanceToHook;
        //float scaleFactor = distance * 5f;

        /*
         *  | <- extra rope space
         *  |
         *  |\
         *  | \
         * a|  \
         *  |    \ c
         *  |      \
         *  |________\ X   a^2 + b^2 = c^2
                b    */

        /*
        rope.transform.localScale = new Vector3(rope.transform.localScale.x,
                                                1.5f,
                                                rope.transform.localScale.z);
        */
        /*
        rope.transform.position = new Vector3(transform.parent.position.x,
                                      rope.transform.position.y, // <- a //rope.transform.position.y - 5f
                                      transform.parent.position.z);
        */
        //rope.transform.rotation = Quaternion.Euler(90, 0, 0);

        rope.transform.localScale = new Vector3(rope.transform.localScale.x,
                                               0.3f,
                                                rope.transform.localScale.z);

        var hook = transform.parent.parent.GetComponent<GrapplingHook>().hook;
        var grapple = transform.parent.parent;
        float yPosition = (grapple.transform.position.y + hook.transform.position.y) / 2f;

        if (player.transform.position.y < -2)
        {
            rope.transform.position = new Vector3(transform.parent.position.x,
                                        rope.transform.position.y,
                                        transform.parent.position.z);
        }
        else
        {
            rope.transform.position = new Vector3(transform.parent.position.x,
                                        yPosition - 3.1f,
                                        transform.parent.position.z);
        }


        transform.parent.rotation = Quaternion.Euler(90, 0, 0);

        var stick = transform.parent.parent.GetComponent<GrapplingHook>().stick;

        stick.transform.position = new Vector3(stick.transform.position.x,
                                       stick.transform.position.y - 1f,
                                       stick.transform.position.z);


        // climb up with controller up rotation
        if (climbingUp)
        {

            // turn off player gravity
            player.transform.GetComponent<Rigidbody>().useGravity = false;

            // move player up in the y direction at a slow rate
            player.transform.position = new Vector3(player.transform.position.x,
                                                    player.transform.position.y + 0.1f,
                                                    player.transform.position.z);

            //float speed = 1.5f;
            //float speedValue = speed * 4f * Time.deltaTime;

            Vector3 hookDistance = transform.parent.position;
            //float playerToHookDistance = Vector3.Distance(player.transform.position, hookDistance);

            //float frac = speedValue / playerToHookDistance;

            //++moveCount;
            // stop movement once moveCount is incremented 10 times
            // total movement per climb = 1.0f
            // if (moveCount == 10.0f)
            // {
            // climbingUp = false;
            //  moveCount = 0f;
            // }

        }

        if (climbingDown)
        {

            // turn off player gravity
            player.transform.GetComponent<Rigidbody>().useGravity = false;

            //float speed = 1.5f;
            //float speedValue = speed * 4f * Time.deltaTime;

            //Vector3 hookDistance = transform.parent.position;

            //float playerToHookDistance = Vector3.Distance(player.transform.position, hookDistance);

            //float frac = speedValue / playerToHookDistance;

            player.transform.position = new Vector3(player.transform.position.x,
                player.transform.position.y - 0.1f,
                player.transform.position.z);

            ++moveCount;
            // stope movement once moveCount is incremented 10 times
            // total movement per climb = 1.0f
            if (moveCount == 10.0f)
            {
                climbingDown = false;
                moveCount = 0f;
            }

        }

        //var hook = transform.parent.parent.GetComponent<GrapplingHook>().hook;
        //stick.GetComponent<MeshRenderer>().enabled = true;

        //hook.transform.position = controller.transform.position;

        // add the player movement script when the player reaches the end of the rope within a certain threshold
        //if ((player.transform.Find("OVRCameraRig").GetComponent<PlayerMovement>() == null)
        //  && ((player.transform.position.y > hook.transform.position.y - 0.25f)        // climbing up the rope
        // || (player.transform.position.y < stick.transform.position.y + 0.25f)))          // climbing down the rope
        if ((player.transform.position.y >= hook.transform.position.y - 1.3f)  // climb up threshold - 0.15f
        //|| (player.transform.position.y <= stick.transform.position.y + 0.01f) // climb down threshold + 0.15f
          || ((player.transform.position.x > hook.transform.position.x + 1f) && (player.transform.position.x < hook.transform.position.x + 2f))    // 1 meter radius threshold
          || ((player.transform.position.x < hook.transform.position.x - 1f) && (player.transform.position.x > hook.transform.position.x - 2f))    // ----------------------
          || ((player.transform.position.z > hook.transform.position.z + 1f) && (player.transform.position.z < hook.transform.position.z + 2f))    // ----------------------
          || ((player.transform.position.z < hook.transform.position.z - 1f) && (player.transform.position.z > hook.transform.position.z - 2f)))   // 1 meter radius threshold
        {
            // turn on player gravity
            player.transform.GetComponent<Rigidbody>().useGravity = true;
            climbingUp = false;
            climbingDown = false;
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
                && (controller.transform.localEulerAngles.x <= 360 && controller.transform.localEulerAngles.x >= 181)
                && !grabbed)
            {
                climbingUp = true;
                moveCount = 0f;
            }
            // Pressing Touchpad
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)
                && (controller.transform.localEulerAngles.x <= 180 && controller.transform.localEulerAngles.x >= 0)
                && !grabbed)
            {
                climbingDown = true;
                moveCount = 0f;
            }
        }
    }
}
