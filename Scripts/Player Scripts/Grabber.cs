using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Grabber : MonoBehaviour
{

    [HideInInspector]
    public bool grabbed = false;

    [HideInInspector]
    public GameObject grabbedObject;

    [HideInInspector]
    public bool transition = false;

    private bool flag = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //every frame the position, rotation of grabbed object is set, unless drone is flying
        if (grabbedObject != null)
        {
            var drone = grabbedObject.GetComponent<DroneFly>();

            if (drone == null) // not a drone
            {
                grabbedObject.transform.position = transform.position;
                grabbedObject.transform.rotation = transform.rotation;
            }
            else if (!drone.takenOff)
            {
                grabbedObject.transform.position = transform.position;
                grabbedObject.transform.rotation = transform.rotation;
            }

            if (grabbedObject.transform.parent == null)
            {
                grabbed = false;
                grabbedObject = null;
            }
        } else
        {
            grabbed = false;
        }
    }

    //while colliding with object:
    private void OnTriggerStay(Collider other)
    {
        if (other != null)
        {
            var placeable = other.gameObject.GetComponent<Placeable>();

            if (placeable != null)
            {

                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && !flag)
                {
                    flag = true;
                    if (grabbed && placeable.validSpot)
                    {
                        //letting go of the object if it is a valid spot
                        grabbed = false;

                        //TODO set parent in placeable
                        //other.transform.SetParent(null);

                        //sets transition boolean
                        transition = true;
                        grabbedObject.transform.parent = null;
                        grabbedObject = null;
                    }

                    else if (grabbed && !placeable.validSpot)
                    {
                        //TODO set transition false in placeable
                        //transition = false;
                    }
                    else if (!grabbed && grabbedObject == null)
                    {
                        //Debug.Log("grabbing");
                        //other.isTrigger = true;
                        //other.attachedRigidbody.useGravity = false;
                        grabbed = true;
                        transition = false;
                        placeable.placed = false;

                        other.transform.SetParent(null);
                        other.transform.SetParent(transform);

                        other.transform.position = transform.position;
                        other.transform.rotation = transform.rotation;

                        grabbedObject = other.gameObject;

                        //sets transition boolean
                        //transition = false;
                        //} else if (!grabbed && transform.Find(other.name) != null)
                        //{
                        //    grabbed = true;
                    }
                    
                } else
                {
                    flag = false;
                }
            }
        }
        else
        {
            //grabbed = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {


        }
    }
}
