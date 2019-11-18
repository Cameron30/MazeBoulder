using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{

    [HideInInspector]
    public bool placed = false;

    [HideInInspector]
    public bool validSpot = false;

    [HideInInspector]
    public GameObject itemSlots;

    //public bool droppable;

    private bool transition;
    private GameObject grabber;
    private GameObject itemSlot;

    private GrapplingHook grapplegunitem;
    //items
    private GameObject[] items;

    //don't change
    [HideInInspector]
    public bool grabbable = true;

    public void Start()
    {
        //finds item slots without having to assign them
        var player = GameObject.Find("Player");
        var itemSlotsTransform = player.transform.Find("ItemSlots");
        itemSlots = itemSlotsTransform.gameObject;

        //find grabber
        var rig = player.transform.Find("OVRCameraRig");
        var space = rig.transform.Find("TrackingSpace");
        grabber = space.Find("RightHandAnchor").gameObject;
        if (OVRInput.GetActiveController() == OVRInput.Controller.LTrackedRemote)
        {
            grabber = space.Find("LeftHandAnchor").gameObject;
        }

        // init items
        items = new GameObject[3];
        items[0] = null;
        items[1] = null;
        items[2] = null;
    }

    void OnTriggerStay(Collider col)
    {
        try
        {
            if (col != null)
            {
                if (col.transform.parent != null)
                {
                    //if colliding with an empty itemSlot
                    if (col.transform.parent.gameObject == itemSlots && col.transform.childCount == 0)
                    {


                        validSpot = true;

                        transition = grabber.GetComponent<Grabber>().transition;

                        //when the player lets go
                        if (/*transform.parent == null && */transition == true)
                        {
                            //set parent to be the item slot
                            placed = true;
                            transition = false;
                            itemSlot = col.gameObject;
                            transform.SetParent(col.transform);
                            grabber.GetComponent<Grabber>().grabbedObject = null;

                            validSpot = false;

                            //var thisCollider = GetComponent<Collider>();
                            //thisCollider.isTrigger = true;

                            var thisRigidBody = GetComponent<Rigidbody>();
                            //thisRigidBody.useGravity = false;
                            thisRigidBody.angularVelocity = Vector3.zero;
                            thisRigidBody.velocity = Vector3.zero;

                            transform.position = col.transform.position;
                            if (gameObject.GetComponent<GrapplingHook>() != null)
                            {
                                transform.eulerAngles = new Vector3(col.transform.eulerAngles.x + 90, col.transform.eulerAngles.y, col.transform.eulerAngles.z);
                            }
                            else
                            {
                                transform.rotation = col.transform.rotation;
                            }
                        }
                        transform.position = transform.parent.position;
                    }
                }
            }
        }
        catch (Exception)
        {

        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col != null)
        {
            if (col.transform.parent != null)
            {
                if (col.transform.parent.gameObject == itemSlots)
                {
                    //detach item from item slot
                    //col.transform.DetachChildren();
                    //transform.SetParent(grabber.transform);
                    if (col.transform.parent != transform)
                    {
                        placed = false;
                    }


                    validSpot = false;
                    //transition = false;
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (placed)
        {
            try
            {
                grapplegunitem = gameObject.GetComponent<GrapplingHook>();
                if (grapplegunitem != null)
                {
                    try
                    {
                        transform.eulerAngles = new Vector3(itemSlot.transform.eulerAngles.x + 90, itemSlot.transform.eulerAngles.y, itemSlot.transform.eulerAngles.z);
                    } catch
                    {
                        itemSlot = transform.parent.gameObject;
                        transform.eulerAngles = new Vector3(itemSlot.transform.eulerAngles.x + 90, itemSlot.transform.eulerAngles.y, itemSlot.transform.eulerAngles.z);
                    }
                }
            }
            catch (Exception)
            {
                transform.rotation = itemSlot.transform.rotation;
            }

            /*if (gameObject.GetComponent<GrapplingHook>() != null)
            {
                transform.eulerAngles = new Vector3(itemSlot.transform.eulerAngles.x + 90, itemSlot.transform.eulerAngles.y, itemSlot.transform.eulerAngles.z);
            }
            else
            {
                transform.rotation = itemSlot.transform.rotation;
            }*/
        }

        if (transform.parent != null)
        {
            transform.position = transform.parent.position;
        }


    }
}
