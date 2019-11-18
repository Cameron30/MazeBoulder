using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookDetector : MonoBehaviour
{

    public bool addFlag;

    private bool fired;

    public GameObject grappleReference;
    public bool otherPlayerHooked;

    Vector3 twoFrames;
    Vector3 oneFrame;

    /*
    private void FixedUpdate()
    {
        if (transform.parent != null)
        {
            if (transform.parent.parent == null)
            {
                RaycastHit hit;
                Vector3 fwd = transform.TransformDirection(Vector3.forward);
                
                if (Physics.Raycast(transform.position, fwd, out hit, 1f))
                {
                    var grapple = grappleReference;

                    var other = hit.collider;
                    //Debug.Log("parent = " + grapple);

                    // reassociate hook back to grappling gun
                    transform.parent = grapple.transform;
                    if (transform.parent != null)
                    {

                        var scriptThing = grapple.GetComponent<GrapplingHook>().fired;
                        fired = scriptThing;
                    }

                    if (fired == true && (other.tag == "Swing" || other.tag == "Climb" || other.tag == "Zipline" || other.tag == "SwingTrap"
                        || other.tag == "Raft"))
                    {

                        //transform.Find("Grappling Gun").GetComponent<GrapplingHook>().hooked = true;
                        //transform.Find("Grappling Gun").GetComponent<GrapplingHook>().hookedObject = other.gameObject;

                        //if valid hookable object, then hook
                        transform.parent.GetComponent<GrapplingHook>().hooked = true;
                        transform.parent.GetComponent<GrapplingHook>().hookedObject = other.gameObject;

                        //apply proper script to new rope
                        if (other.tag == "Swing" && !addFlag)
                        {
                            transform.parent.GetComponent<GrapplingHook>().cylinderRope.AddComponent<Swingable>();
                            addFlag = true;
                            other.tag = "Untagged";
                            //var rope = transform.parent.GetComponent<GrapplingHook>().cylinderRope;
                            //rope.transform.localScale = new Vector3(0.95f, rope.transform.localScale.y, 0.95f);

                        }
                        if (other.tag == "Zipline" && !addFlag)
                        {
                            //transform.parent.parent.GetComponent<GrapplingHook>().cylinderRope.AddComponent<Ziplineable>();
                            transform.parent.GetComponent<GrapplingHook>().cylinderRope.AddComponent<Ziplineable>();
                            addFlag = true;
                            other.tag = "Untagged";

                            // makes zipline stick visible
                            var stick = transform.parent.GetComponent<GrapplingHook>().stick;
                            stick.GetComponent<MeshRenderer>().enabled = true;

                        }
                        if (other.tag == "Climb" && !addFlag)
                        {
                            transform.parent.GetComponent<GrapplingHook>().cylinderRope.AddComponent<Climbable>();
                            addFlag = true;
                            other.tag = "Untagged";
                            /*
                            // spawn in the new grappling hook
                            GameObject newGrapple = (GameObject)Instantiate(Resources.Load("Grappling Gun"));

                            GameObject player = GameObject.Find("Player");

                            GameObject controller = player.transform.Find("OVRCameraRig").transform.Find("TrackingSpace").transform.Find("RightHandAnchor").gameObject;

                            newGrapple.transform.position = controller.transform.position;
                            newGrapple.transform.rotation = controller.transform.rotation;
                            newGrapple.transform.parent = controller.transform;

                            controller.GetComponent<Grabber>().grabbed = true;
                            */ /*
                        }
                        if (other.tag == "RaftObject")
                        {
                            transform.parent.GetComponent<GrapplingHook>().cylinderRope.AddComponent<RaftScript>();
                            addFlag = true;
                            other.tag = "Untagged";

                            // var rope = transform.parent.GetComponent<GrapplingHook>().cylinderRope;
                            //var stick = transform.parent.GetComponent<GrapplingHook>().stick;

                            //rope.GetComponent<MeshRenderer>().enabled = false;
                            //stick.GetComponent<MeshRenderer>().enabled = false;

                            //var gun = transform.Find("gun_rotated").gameObject;
                            //var gun = transform.parent.Find("gun_rotated").gameObject;

                            //gun.GetComponent<MeshRenderer>().enabled = true;

                            /*
                            // spawn in the new grappling hook
                            GameObject newGrapple = (GameObject)Instantiate(Resources.Load("Grappling Gun"));

                            GameObject player = GameObject.Find("Player");

                            GameObject controller = player.transform.Find("OVRCameraRig").transform.Find("TrackingSpace").transform.Find("RightHandAnchor").gameObject;

                            newGrapple.transform.position = controller.transform.position;
                            newGrapple.transform.rotation = controller.transform.rotation;
                            newGrapple.transform.parent = controller.transform;

                            controller.GetComponent<Grabber>().grabbed = true;
                            */ /*
                        }
                        if (other.tag == "SwingTrap")
                        {
                            Destroy(grapple);

                            var stick = other.transform.Find("TrapStick");
                            var rope = other.transform.Find("TrapRope");
                            var cube = other.transform;

                            // makes the stick and rope from the trap visible and enables the colliders
                            stick.GetComponent<MeshRenderer>().enabled = true;
                            stick.GetComponent<CapsuleCollider>().enabled = true;

                            rope.GetComponent<MeshRenderer>().enabled = true;
                            //rope.GetComponent<CapsuleCollider>().enabled = true;
                            rope.GetComponent<BoxCollider>().enabled = true;

                            rope.GetComponent<BoxCollider>().isTrigger = false;

                            // get rid of the trigger
                            cube.GetComponent<MeshRenderer>().enabled = false;
                            cube.GetComponent<BoxCollider>().enabled = false;

                            // spawn in the new grappling hook
                            GameObject newGrapple = (GameObject)Instantiate(Resources.Load("Grappling Gun"));

                            GameObject player = GameObject.Find("Player");

                            GameObject controller = player.transform.Find("OVRCameraRig").transform.Find("TrackingSpace").transform.Find("RightHandAnchor").gameObject;

                            newGrapple.transform.position = controller.transform.position;
                            newGrapple.transform.rotation = controller.transform.rotation;
                            newGrapple.transform.parent = controller.transform;

                            controller.GetComponent<Grabber>().grabbed = true;
                        }


                        // detach grappling gun from player
                        transform.parent.parent = null;
                        //player.transform.GetComponent<Grabber>().grabbed = false;
                        //player.transform.GetComponent<GrapplingHook>().fired = true;

                    }

                    else
                    {
                        transform.parent.GetComponent<GrapplingHook>().ReturnHook();
                        //grapple.GetComponent<GrapplingHook>().ReturnHook();
                    }

                }
            }
            }
        }
    */

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hook Detector");
        //var grapple = transform.parent.gameObject;

        var grapple = grappleReference;
        //Debug.Log("parent = " + grapple);

        // reassociate hook back to grappling gun
        transform.parent = grapple.transform;
        if (transform.parent != null)
        {

            var scriptThing = grapple.GetComponent<GrapplingHook>().fired;
            fired = scriptThing;
        }

        if (fired == true && (other.tag == "Swing" || other.tag == "Climb" || other.tag == "Zipline" || other.tag == "SwingTrap"
            || other.tag == "Raft"))
        {

            //transform.Find("Grappling Gun").GetComponent<GrapplingHook>().hooked = true;
            //transform.Find("Grappling Gun").GetComponent<GrapplingHook>().hookedObject = other.gameObject;

            //if valid hookable object, then hook
            transform.parent.GetComponent<GrapplingHook>().hooked = true;
            transform.parent.GetComponent<GrapplingHook>().hookedObject = other.gameObject;

            //apply proper script to new rope
            if (other.tag == "Swing" && !addFlag)
            {
                transform.parent.GetComponent<GrapplingHook>().cylinderRope.AddComponent<Swingable>();
                addFlag = true;
                other.tag = "Untagged";
                //var rope = transform.parent.GetComponent<GrapplingHook>().cylinderRope;
                //rope.transform.localScale = new Vector3(0.95f, rope.transform.localScale.y, 0.95f);

            }
            if (other.tag == "Zipline" && !addFlag)
            {
                //transform.parent.parent.GetComponent<GrapplingHook>().cylinderRope.AddComponent<Ziplineable>();
                transform.parent.GetComponent<GrapplingHook>().cylinderRope.AddComponent<Ziplineable>();
                addFlag = true;
                other.tag = "Untagged";

                // makes zipline stick visible
                var stick = transform.parent.GetComponent<GrapplingHook>().stick;
                stick.GetComponent<MeshRenderer>().enabled = true;
                //stick.AddComponent<ItemTrigger>();
                stick.GetComponent<BoxCollider>().enabled = true;

                // turn on ItemTrigger collider
                //GameObject.Find("Pit_Post/ItemTrigger").GetComponent<BoxCollider>().enabled = true;

            }
            if (other.tag == "Climb" && !addFlag)
            {
                transform.parent.GetComponent<GrapplingHook>().cylinderRope.AddComponent<Climbable>();
                addFlag = true;
                other.tag = "Untagged";
                /*
                // spawn in the new grappling hook
                GameObject newGrapple = (GameObject)Instantiate(Resources.Load("Grappling Gun"));

                GameObject player = GameObject.Find("Player");

                GameObject controller = player.transform.Find("OVRCameraRig").transform.Find("TrackingSpace").transform.Find("RightHandAnchor").gameObject;

                newGrapple.transform.position = controller.transform.position;
                newGrapple.transform.rotation = controller.transform.rotation;
                newGrapple.transform.parent = controller.transform;

                controller.GetComponent<Grabber>().grabbed = true;
                */
            }
            if (other.tag == "RaftObject")
            {
                transform.parent.GetComponent<GrapplingHook>().cylinderRope.AddComponent<RaftScript>();
                addFlag = true;
                other.tag = "Untagged";

               // var rope = transform.parent.GetComponent<GrapplingHook>().cylinderRope;
                //var stick = transform.parent.GetComponent<GrapplingHook>().stick;

                //rope.GetComponent<MeshRenderer>().enabled = false;
                //stick.GetComponent<MeshRenderer>().enabled = false;

                //var gun = transform.Find("gun_rotated").gameObject;
                //var gun = transform.parent.Find("gun_rotated").gameObject;

                //gun.GetComponent<MeshRenderer>().enabled = true;

                /*
                // spawn in the new grappling hook
                GameObject newGrapple = (GameObject)Instantiate(Resources.Load("Grappling Gun"));

                GameObject player = GameObject.Find("Player");

                GameObject controller = player.transform.Find("OVRCameraRig").transform.Find("TrackingSpace").transform.Find("RightHandAnchor").gameObject;

                newGrapple.transform.position = controller.transform.position;
                newGrapple.transform.rotation = controller.transform.rotation;
                newGrapple.transform.parent = controller.transform;

                controller.GetComponent<Grabber>().grabbed = true;
                */
            }
            if (other.tag == "SwingTrap")
            {
                Destroy(grapple);

                var stick = other.transform.Find("TrapStick");
                var rope = other.transform.Find("TrapRope");
                var cube = other.transform;

                // makes the stick and rope from the trap visible and enables the colliders
                stick.GetComponent<MeshRenderer>().enabled = true;
                stick.GetComponent<CapsuleCollider>().enabled = true;

                rope.GetComponent<MeshRenderer>().enabled = true;
                //rope.GetComponent<CapsuleCollider>().enabled = true;
                rope.GetComponent<BoxCollider>().enabled = true;

                rope.GetComponent<BoxCollider>().isTrigger = false;

                // get rid of the trigger
                cube.GetComponent<MeshRenderer>().enabled = false;
                cube.GetComponent<BoxCollider>().enabled = false;

                // spawn in the new grappling hook
                GameObject newGrapple = (GameObject)Instantiate(Resources.Load("Grappling Gun"));

                GameObject player = GameObject.Find("Player");

                GameObject controller = player.transform.Find("OVRCameraRig").transform.Find("TrackingSpace").transform.Find("RightHandAnchor").gameObject;

                newGrapple.transform.position = controller.transform.position;
                newGrapple.transform.rotation = controller.transform.rotation;
                newGrapple.transform.parent = controller.transform;

                controller.GetComponent<Grabber>().grabbed = true;
            }


            // detach grappling gun from player
            transform.parent.parent = null;
            //player.transform.GetComponent<Grabber>().grabbed = false;
            //player.transform.GetComponent<GrapplingHook>().fired = true;

        }

        else 
        {
            transform.parent.GetComponent<GrapplingHook>().ReturnHook();
            //grapple.GetComponent<GrapplingHook>().ReturnHook();
        }

    }

}
