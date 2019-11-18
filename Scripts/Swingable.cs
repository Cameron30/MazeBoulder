using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swingable : MonoBehaviour
{

    private bool spawnFlag;

    private GameObject player;
    private GameObject controller;
    private GameObject hookThing;

    private Vector3 hookPosition;

    private HingeJoint hookHinge;
    private JointMotor hingeMotor;
    

    // Use this for initialization
    void Start()
    {
        // line renderer width = 0.05
        // add spring joint to hook and change settings to match rope behavior


        
        // THIS IS WHERE THE SPRINGJOINT APPEARS called springHook, way down
        //Destroy(gameObject);
        var hook = transform.parent.gameObject;
        hook.AddComponent<SpringJoint>();
        var line = hook.GetComponent<LineRenderer>();
        line.useWorldSpace = true;

        /*
        Destroy(hook.GetComponent<LineRenderer>());
        hook.AddComponent<LineRenderer>();
        var line = hook.GetComponent<LineRenderer>();

        line.useWorldSpace = true;
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;
        line.positionCount = 0;
        */
        //hook.GetComponent<LineRenderer>().isVisible;
        // I guess this means the hook's boxcollider is now affected by collision.
        hook.GetComponent<BoxCollider>().isTrigger = false;

        // for whatever reason the springjoint is created some ten lines up
        
        var springHook = hook.GetComponent<SpringJoint>();
        var grapple = transform.parent.parent;
        var weight = grapple.Find("Swing Weight");

        // Create Box Collider for Swing Weight for player movement error
        // 0w0 what's this?

        var weightCollider = weight.GetComponent<BoxCollider>();

        weight.GetComponent<Rigidbody>().isKinematic = false;
        weight.GetComponent<Rigidbody>().useGravity = true;

        var weightThing = weight.gameObject;

        weightThing.AddComponent<SwingHinge>();

        weightCollider.size = new Vector3(20, 40, 40); // what a big collider
        weightCollider.isTrigger = true; // it got triggered by its size

        springHook.connectedBody = weight.GetComponent<Rigidbody>();
        springHook.anchor = Vector3.zero;
        springHook.autoConfigureConnectedAnchor = false;
        springHook.connectedAnchor = Vector3.zero;

        // add rope controller to hook
        hook.AddComponent<RopeControllerSimple>();
        //hook.AddComponent<RopeControllerRealistic>();
        var ropeController = hook.GetComponent<RopeControllerSimple>();
        //var realRope = hook.GetComponent<RopeControllerRealistic>();

        ropeController.whatTheRopeIsConnectedTo = hook.transform;
        ropeController.whatIsHangingFromTheRope = weight.transform;
        ropeController.loadMass = 100f;

        //realRope.whatIsHangingFromTheRope = hook.transform;
        //realRope.whatTheRopeIsConnectedTo = weight.transform;

        hookThing = transform.parent.gameObject;
        hookPosition = hookThing.transform.position;

        // rope distance
        //ropeDistance = gameObject.GetComponent<MeshRenderer>().bounds.size;
        //ropeDistance = gameObject.GetComponent<MeshRenderer>().bounds.size.y;


        // get rid of the placeable script in the grappling hook
        Destroy(transform.parent.parent.GetComponent<Placeable>());

        // get rid of the collider box on the hook
        Destroy(transform.parent.GetComponent<Collider>());

        // acquires the properties of the player for transformations
        player = GameObject.Find("Player");

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

        // Create a hinge joint for the rope to swing on
        CreateHinges();

        // increase collider radius of rope
        var ropeCollider = gameObject.transform.GetComponent<CapsuleCollider>();
        ropeCollider.radius = ropeCollider.radius * 2f;


    }

    void CreateHinges()
    {

        // anchor = 0 1 0
        // axis = 1 0 0
        // enable preprocessing = true
        // default solver iterations = 6
        // default solver velocity = 1

        // y rotation = 90
        // axis = 0 0 1

        //var rope = gameObject;
        //var rope = transform.parent.parent.GetComponent<GrapplingHook>().cylinderRope;
        var hook = transform.parent.gameObject;
        var grapple = transform.parent.parent;
        //var hookObject = GameObject.Find("SwingCube");
        /*
        var rope = transform.parent.parent.GetComponent<GrapplingHook>().cylinderRope;
        var distanceToHook = transform.parent.parent.GetComponent<GrapplingHook>().distanceToHook;
        float scaleFactor = distanceToHook * 0.0335f;
        rope.transform.localScale = new Vector3(0.09f, scaleFactor, 0.09f);
        */
        /*
        rope.transform.position = new Vector3(rope.transform.position.x,
                                              rope.transform.position.y,
                                              rope.transform.position.z);
                                              */
        /*------------------------------------------------------------------------
        // Main Hinge and Main Motor
        hookHinge = hook.AddComponent<HingeJoint>();
        hookHinge.connectedBody = grapple.GetComponent<Rigidbody>();
        hookHinge.anchor = new Vector3(0, 0, 0);
        hookHinge.axis = new Vector3(0, 0, 0); // y axis
        // hinge motor
        hingeMotor = new JointMotor();
        hingeMotor.force = 100; // 10000
        hingeMotor.targetVelocity = positiveVelocity;
        hingeMotor.freeSpin = false;
        hookHinge.motor = hingeMotor;
        hookHinge.useMotor = true;

        isPositive = true;
        isNegative = false;
        firstFlag = true;
        */
        // spawn in cylinders and change sizes before connecting to hinge joints
        /*
        GameObject ropeB = (GameObject)Instantiate(Resources.Load("SwingRope"));
        ropeB.transform.position = new Vector3(rope.transform.position.x,
                                               hook.transform.position.y,
                                               rope.transform.position.z);
        ropeB.transform.rotation = rope.transform.rotation;

        var topHinge = ropeB.transform.Find("Top Chain");
        topHinge.GetComponent<HingeJoint>().connectedBody = hook.GetComponent<Rigidbody>();
        */
        /*----------------------------------------------------------------------------------------
        GameObject conRope = (GameObject)Instantiate(Resources.Load("ConfigRope"));
        conRope.transform.position = new Vector3(rope.transform.position.x,
                                                 hook.transform.position.y,
                                                 rope.transform.position.z);
        conRope.transform.rotation = rope.transform.rotation;
        var topHinge = conRope.transform.Find("Top Chain");
        topHinge.GetComponent<ConfigurableJoint>().connectedBody = hook.GetComponent<Rigidbody>();
        */
        //ropeB.transform.parent = grapple;
        //ropeB.transform.rotation = Quaternion.Euler(0, 0, 45);
        //ropeB.transform.position = hook.transform.position;

        // Ignore collision between hinges and maze walls
        /*
        var chainA = ropeB.transform.Find("Chain 1");
        var chainB = ropeB.transform.Find("Chain 2");
        var chainC = ropeB.transform.Find("Chain 3");
        var chainD = ropeB.transform.Find("Chain 4");
        var chainE = ropeB.transform.Find("Chain 5");
        var chainF = ropeB.transform.Find("Chain 6");
        var chainG = ropeB.transform.Find("Chain 7");
        var chainH = ropeB.transform.Find("Chain 8");
        var chainI = ropeB.transform.Find("Chain 9");
        var chainJ = ropeB.transform.Find("Chain 10");
        var bottomHinge = ropeB.transform.Find("Bottom Chain");
        */
        //var mazeWall = GameObject.Find();

        //Physics.IgnoreCollision(topHinge.GetComponent<CapsuleCollider>(), );
        var controller = player.transform.Find("OVRCameraRig").transform.Find("TrackingSpace").transform.Find("RightHandAnchor").gameObject;
        //ropeB.transform.rotation = controller.transform.rotation;
        /*
        ropeB.transform.position = new Vector3(hook.transform.position.x,
                                               rope.transform.position.y + 99f,
                                               hook.transform.position.z);
                                               */
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
        transform.parent.parent = null;
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

        // update hook's position
        var hook = transform.parent.gameObject;

        hook.transform.position = hookPosition;
    }
}


