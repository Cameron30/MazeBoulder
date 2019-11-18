using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{

    public GameObject hook;
    public GameObject cylinderRope;
    public GameObject stick;

    public float hookTravelSpeed;
    public float maxDistance;

    public bool held;

    [HideInInspector]
    public bool fired;
    [HideInInspector]
    public bool hooked;
    [HideInInspector]
    public GameObject hookedObject;

    [HideInInspector]
    public float currentDistance;
    [HideInInspector]
    public float distanceToHook;

    public bool remote;

    private bool fireable;
    private int counter;
    private bool counterBool;
    private bool first = true;
    private float gunAngleX;
    private float gunAngleZ;
    private Transform hookRef;

    public bool newFlag;
    private bool oneFrameFlag;

    public bool firstFired;
    public bool wasFired;

    private void Start()
    {
        newFlag = true;
    }

    private void LateUpdate()
    {
        
        if (oneFrameFlag == true)
            newFlag = false;
        if (newFlag == true)
            oneFrameFlag = true;
    }

    void Update()
    {
        if (fired && !wasFired)
        {
            Debug.Log("firstFired");
            firstFired = true;
        } else if (fired)
        {
            wasFired = true;
            firstFired = false;
        } else
        {
            wasFired = false;
            firstFired = false;
        }

        LineRenderer rope = hook.GetComponent<LineRenderer>();
        AudioSource grappleFire = GetComponent<AudioSource>();

        //if grabbed, fireable is true
        if (transform.parent != null && transform.parent.gameObject.GetComponent<Grabber>() != null)
        {
            var grabber = transform.parent.gameObject.GetComponent<Grabber>();
            if (grabber.grabbed == true)
            {
                fireable = true;
                held = true;
            }

        }
        else
        {
            fireable = false;
            held = false;
        }

        //get own placeable script
        var placeable = GetComponent<Placeable>();

        // firing the hook
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && fired == false && fireable == true && !placeable.validSpot)
        {
            grappleFire.Play();
            fired = true;

            // save gun angle for hook
            //gunAngle = transform.rotation;
            //gunAngleX = transform.rotation.x;
            //gunAngleZ = transform.rotation.z;
        }
        

        if (Input.GetKeyDown("space"))
        {
            fired = true;
        }

       // if (remote)
       // {
       //     fired = true;
       // }

        if (fired)
        {
            rope.useWorldSpace = true;
            rope.positionCount = 2;
            rope.SetPosition(0, transform.position);            // index 0
            rope.SetPosition(1, hook.transform.position);       // index 1
        }
        else
        {
            // reassign hook's parent
            hook.transform.parent = transform;
            //hookRef.transform.parent = transform;
            ReturnHook();
        }

        //updated position and rotation every frame (ADD THIS IN GRABBER/PLACEABLE)
        /*
        if (fired == false)
        {
            transform.position = transform.parent.position;
            transform.rotation = transform.parent.rotation;
        }
        */

        if (fired == true && hooked == false)
        {
            hook.transform.Translate((-Vector3.forward) * Time.deltaTime * hookTravelSpeed);
            // fix the hook's angle the moment it is fired
            //hook.transform.rotation = gunAngle;
            //hook.transform.rotation = Quaternion.Euler(gunAngleX, hook.transform.rotation.y, gunAngleZ);
            hook.transform.parent = null;
            //hookRef = transform.Find("Hook Reference");
            //hook.transform.rotation = hookRef.transform.rotation;
            //hookRef.transform.parent = null;


            //hook.transform.rotation = Quaternion.Euler(hook.transform.rotation.eulerAngles.x - transform.rotation.eulerAngles.x, 
             //   hook.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y, 
             //   hook.transform.rotation.eulerAngles.z - transform.rotation.eulerAngles.z);

            currentDistance = Vector3.Distance(transform.position, hook.transform.position);
        }

        if (currentDistance >= maxDistance)
        {
            // reassign hook's parent
            hook.transform.parent = transform;
            //hookRef.transform.parent = transform;
            ReturnHook();
        }

        if (fired == true && hooked == true)
        {

            // reassign parent to hook
            hook.transform.parent = transform;
            //hookRef.transform.parent = transform;

            distanceToHook = Vector3.Distance(transform.position, hook.transform.position);

            float scaleFactor = distanceToHook * 0.0335f;
            // distanceToHook * 0.001

            // scale of the cylinder rope
            cylinderRope.transform.localScale = new Vector3(0.09f, scaleFactor, 0.09f);
            // 0.09f, scaleFactor, 0.09f
            // .5f, scaleFactor, .5f
            // .7f, scaleFactor, 0.15f

            // gets the midpoint between the grappling hook and the hook in x,y,z axis
            float xPosition = (transform.position.x + hook.transform.position.x) / 2f;
            float yPosition = (transform.position.y + hook.transform.position.y) / 2f;
            float zPosition = (transform.position.z + hook.transform.position.z) / 2f;

            // position of the cylinder rope in respect to the hook holder 
            // z axis operation
            cylinderRope.transform.position = new Vector3(xPosition, yPosition, zPosition);

            cylinderRope.GetComponent<MeshRenderer>().enabled = true;

            var swing = cylinderRope.GetComponent<Swingable>();

            if (swing != null)
            {
                // make rope wider and scale down rope for other Hinge Joints
                cylinderRope.transform.localScale = new Vector3(0.25f, scaleFactor, 0.25f);

                //var ropeCollider = cylinderRope.transform.GetComponent<CapsuleCollider>().height;
                //ropeCollider *= 5f;
                rope.useWorldSpace = true;
                cylinderRope.GetComponent<MeshRenderer>().enabled = false;

                //yPosition = (transform.position.y - hook.transform.position.y) / 2.0f;

                //cylinderRope.transform.position = new Vector3(xPosition, hook.transform.position.y - 0.5f, zPosition + 1f);

                //cylinderRope.GetComponent<MeshRenderer>().enabled = false;

                /*
                float ropeSize = cylinderRope.GetComponent<MeshRenderer>().bounds.size.y;

                // gets new midpoint between the grappling hook and the hook in x,y,z axis
                xPosition = (transform.position.x + hook.transform.position.x) / 2.0f;
                // yPosition = (transform.position.y + hook.transform.position.y) / 2.0f;
                yPosition = hook.transform.position.y + (transform.parent.position.y * ropeSize);
                zPosition = (transform.position.z + hook.transform.position.z) / 2.0f;

                cylinderRope.transform.position = new Vector3(xPosition, yPosition, zPosition);
                */
            }



            // fix the rotation and position of zipline stick
            stick.transform.position = new Vector3(transform.position.x,
                                                   transform.position.y - 5.6f,
                                                   transform.position.z);
            stick.transform.rotation = Quaternion.Euler(0,0,0);
            stick.transform.localScale = new Vector3(stick.transform.localScale.x,
                                                     2f,
                                                     stick.transform.localScale.z);

            rope.positionCount = 0;

            //rope.useWorldSpace = false;

            // delete grappling gun
            //Destroy(transform.Find("gun_rotated").gameObject);
            var gun = transform.Find("gun_rotated").gameObject;
            gun.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void ReturnHook()
    {
        hook.transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x + 180, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z * -1);
        hook.transform.position = transform.position + (transform.forward * .65f) + (transform.right * .001f) + (transform.up * .05f);
        fired = false;
        hooked = false;

        LineRenderer rope = hook.GetComponent<LineRenderer>();
        rope.useWorldSpace = false;
        rope.positionCount = 0;
        counter = 0;
    }
}
