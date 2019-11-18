using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerMovement : MonoBehaviour {

    //running boolean (toggle when swipe up)
    public bool running = false;

    public GameObject headset;
    public GameObject body;
    float walkSpeed = 2f;
    float runSpeed = 4f;

    float startRunY;
    float endRunY;
    //float runTime = 0f;

    Vector3 walkDir;
    Vector3 forward;
    Vector3 right;

    private bool itemFlag;

    private GameObject head;
    private GameObject itemSlots;
    private GameObject controller;

    //private CharacterController cc;
    //private int frameCount;
    private bool flag;
    private bool turnFlag;
    //private bool controllerOff;
    //private Transform space;
    // Use this for initialization
     void Start()
    {
        OVRManager.tiledMultiResLevel = OVRManager.TiledMultiResLevel.LMSHigh;
        OVRManager.display.displayFrequency = 72.0f;

        //space = transform.Find("TrackingSpace");
        controller = GameObject.Find("/Player/OVRCameraRig/TrackingSpace/RightHandAnchor").gameObject;

        body = GameObject.Find("Player");

        headset = GameObject.Find("/Player/OVRCameraRig/TrackingSpace/CenterEyeAnchor").gameObject;

        itemSlots = GameObject.Find("/Player/ItemSlots").gameObject;

        //frameCount = 0;

        //cc = GetComponent<CharacterController>();
        //controllerOff = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //head always follows the camera
        //TODO: DO THIS WHEN HEAD ROTATION IS AROUND CENTRAL POINT, NOT NECK AREA
        //head.transform.eulerAngles = headset.transform.eulerAngles;

        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) && flag == false)
        {
            var coords = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            flag = true;
            //controls for turning and running using d-pad
            if (coords.y > 0)
            {
                if (coords.x <= 0 && -coords.x >= coords.y)
                {
                    body.transform.eulerAngles = new Vector3(0, body.transform.eulerAngles.y - 30, 0);
                    turnFlag = true;
                }
                else if (coords.x > 0 && coords.x > coords.y)
                {
                    body.transform.eulerAngles = new Vector3(0, body.transform.eulerAngles.y + 30, 0);
                    turnFlag = true;
                }
                else
                {
                    if (running)
                    {
                        running = false;
                        StartCoroutine(pauseController());
                    } else if (!running)
                    {
                        running = true;
                    }
                    //running = true;
                    turnFlag = false;
                }
            }
            else if (coords.y <= 0)
            {
                if (coords.x <= 0 && coords.x <= coords.y)
                {
                    body.transform.eulerAngles = new Vector3(0, body.transform.eulerAngles.y - 30, 0);
                    turnFlag = true;
                }
                else if (coords.x > 0 && -coords.x < coords.y)
                {
                    body.transform.eulerAngles = new Vector3(0, body.transform.eulerAngles.y + 30, 0);
                    turnFlag = true;
                }
                else
                {
                    //running = false;
                }
            }
            //itemSlots.transform.eulerAngles = new Vector3(0, headset.transform.eulerAngles.y, 0);
        } else if (flag == true)
        {
            flag = false;
        }

        //turning flag reset when they let go of the dpad
        if (OVRInput.GetUp(OVRInput.Touch.One))
        {
            turnFlag = false;
        }

        //running movement in direction of head
        if (running)
        {
            //move body while running
            var pointing = controller.transform.forward;
            pointing.y = 0f;
            pointing.Normalize();
            body.transform.position = body.transform.position + pointing * runSpeed * Time.deltaTime;
        }
        else if (!turnFlag)
        {

            if (Mathf.Abs(headset.transform.eulerAngles.y - itemSlots.transform.eulerAngles.y) > 30)
            {
                if ((headset.transform.eulerAngles.y < itemSlots.transform.eulerAngles.y && Math.Abs(headset.transform.eulerAngles.y - itemSlots.transform.eulerAngles.y) < 180) || headset.transform.eulerAngles.y - 180f > itemSlots.transform.eulerAngles.y)
                {
                    itemSlots.transform.eulerAngles = new Vector3(itemSlots.transform.eulerAngles.x, itemSlots.transform.eulerAngles.y - 1, itemSlots.transform.eulerAngles.z);
                } else
                {
                    itemSlots.transform.eulerAngles = new Vector3(itemSlots.transform.eulerAngles.x, itemSlots.transform.eulerAngles.y + 1, itemSlots.transform.eulerAngles.z);
                }
            }

            //if not running, walk if touching touchpad
            if (OVRInput.GetDown(OVRInput.Touch.One))
            {
                //camera forward and right vectors:
                forward = headset.transform.forward;
                right = headset.transform.right;
                
            }

            /*
            if (OVRInput.GetUp(OVRInput.Touch.One))
            {
                forward = new Vector3(0, 0, 0);
                right = new Vector3(0, 0, 0);
            }
            */

            //get poition on touchpad
            Vector2 thumbPos = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

                //project forward and right vectors on the horizontal plane (y = 0)
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();

                //this is the direction in the world space we want to move:
                var desiredMoveDirection = forward * thumbPos.y + right * thumbPos.x;

                //apply the movement
                body.transform.Translate(desiredMoveDirection * walkSpeed * Time.deltaTime, Space.World);
            
        }

        //check for player falling off the map
        if (body.transform.position.y < -200)
        {
            var room = GameObject.Find("Buffer Room(Clone)");
            if (body.transform.position.x > room.transform.position.x)
            {
                body.transform.position = new Vector3(room.transform.position.x, 0, room.transform.position.z);
            }
            else
            {
                body.transform.position = new Vector3(0, 0, 0);
            }

        }
    }

    IEnumerator pauseController()
    {
        walkSpeed = 0f;
        yield return new WaitForSeconds(0.4f);
        walkSpeed = 2f;
    }
}