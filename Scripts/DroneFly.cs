using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneFly : MonoBehaviour {
    AudioSource clickStart;

    [HideInInspector]
    public bool takenOff = false;
    private bool grabbed = false;
    private bool landed = false;

    [HideInInspector]
    public bool[,] grid = new bool[33, 33];

    [HideInInspector]
    public bool update;

    private Transform parent;

    private Light spotlight;

    bool audioPlayedOnce = false;
    bool firstGrab = false;
    public bool moveForward = false;
    bool doneMove = false;

    public bool isRemote;

    private Transform initialSpot;
    private GameObject boulderTrap;
    private GameObject itemSlots;

    // Use this for initialization
    void Start ()
    {
        spotlight = transform.Find("Spotlight").GetComponent<Light>();
        clickStart = GetComponent<AudioSource>();
        initialSpot = GameObject.Find("Drone/Initial Spot").transform;
        boulderTrap = GameObject.Find("Boulder Trap");
        itemSlots = GameObject.Find("Player/ItemSlots");
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (transform.parent != null)
        {
            parent = transform.parent;
        }

        for (int i = 0; i < 3; ++i)
        {
            if (transform.parent == itemSlots.transform.GetChild(i))
                grabbed = false;
        }
        // null exceptions on BoulderTrap
        //if (!boulderTrap.transform.Find("ItemTrigger").GetComponent<ItemTrigger>().stayFlag)
        //{
            if (!GetComponent<Placeable>().placed && !GetComponent<Placeable>().validSpot && (transform.parent != null || grabbed == true))
            {
                grabbed = true;

                if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
                {
                    //Debug.Log("Grabbing Drone (Pressing Trigger)");

                    // only for when picking up the drone from ground/floor/etc.
                    // NOTE: only sound doesn't play, spotlight is still there
                    if (!firstGrab)
                    {
                        // ignore audio and taken off
                        audioPlayedOnce = true;
                        takenOff = true;
                        spotlight.enabled = false;
                        spotlight.color = Color.clear;
                        firstGrab = true;

                        //transform.parent = null;
                        update = true;
                        landed = true;
                        transform.parent = parent;
                        transform.position = transform.parent.position;
                        transform.rotation = transform.parent.rotation;
                        Invoke("audioDelay", 2.0f);
                        //Invoke("takenOffDelay", 1.0f);
                        //Debug.Log("firstGrab = " + firstGrab);
                    }

                    if (!takenOff)
                    {
                        //Debug.Log("takenOff = " + takenOff);
                        //REPLACE THIS WITH ANIMATION
                        //transform.position = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);

                        /*Audio*/
                        if (!audioPlayedOnce)
                        {
                            Debug.Log("Playing Drone Audio");
                            clickStart.Play();
                            audioPlayedOnce = true;
                        }

                        Invoke("audioDelay", 2.0f); // resets bool

                        spotlight.enabled = true;
                        // spotlight color
                        spotlight.color = Color.green;
                        takenOff = true;
                        transform.parent = null;
                        //spotlight.intensity = 1.5f;
                        // moveForward
                        moveForward = true;


                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                        landed = true;
                    }

                    //shoot drone up 1 meter(s)
                    if (!(transform.position.y > parent.transform.position.y + 1))
                    {
                        transform.position = new Vector3(transform.position.x,
                                                         transform.position.y + .25f, // 0.25
                                                         transform.position.z);
                    }
                    // shoot the drone forward relative to "Initial Spot" on Drone prefab
                    if (moveForward)
                    {
                        //Debug.Log("[moveForward]");
                        // reference "Initial Spot" in Drone

                        //initialSpot.parent = null;

                        // move towards reference point
                        transform.position = Vector3.MoveTowards(transform.position,
                                                                 initialSpot.transform.position,
                                                                 5.0f * Time.deltaTime); //1
                        Invoke("moveForwardDelay", 0.4f); //0.2
                        Invoke("doneMoveDelay", 0.4f);
                    }

                    //start driving drone
                    if (doneMove)
                    {
                        float angleX = parent.localEulerAngles.x;
                        angleX = (angleX > 180) ? angleX - 360 : angleX;

                        transform.position += transform.forward * angleX * .005f;

                        float angleZ = parent.localEulerAngles.z;
                        angleZ = (angleZ > 180) ? angleZ - 360 : angleZ;

                        transform.position -= transform.right * angleZ * .002f;
                    }
                    //find traps, etc.
                    var maze = GameObject.Find("Base(Clone)");
                    if (maze != null)
                    {
                        if (transform.position.x > maze.transform.position.x - 82.5 && transform.position.x < maze.transform.position.x + 82.5
                            && transform.position.z > maze.transform.position.z - 82.5 && transform.position.z < maze.transform.position.z + 82.5)
                        {
                            spotlight.color = Color.green;

                            //TODO: not sure if these work
                            int i = (int)((transform.position.x - maze.transform.position.x + 82.5f) / 5);
                            int j = (int)((transform.position.z - maze.transform.position.z + 82.5f) / 5);
                            if (i < 32 && i > 0 && j < 32 && j > 0)
                            {
                                grid[i, j] = true;
                                grid[i - 1, j] = true;
                                grid[i + 1, j] = true;
                                grid[i, j - 1] = true;
                                grid[i, j + 1] = true;

                                if (maze != null)
                                {
                                    var mazeGen = maze.GetComponent<MazeGen>();
                                    if (mazeGen.done)
                                    {
                                        if (mazeGen.grid[i, j] == '!')
                                        {
                                            spotlight.color = Color.red;
                                        }
                                    }
                                }
                            }
                        }
                    }


                }
                else if (landed)
                {
                    transform.parent = parent;
                    transform.position = transform.parent.position;
                    transform.rotation = transform.parent.rotation;
                    takenOff = false;
                    spotlight.enabled = false;
                    update = true;
                    landed = false;

                    doneMove = false;
                }
            }
        //} // boulderTrap

        if (takenOff || isRemote)
        {
            spotlight.enabled = true;
        }
        else
        {
            spotlight.enabled = false;
        }

    }
    void audioDelay()
    {
        audioPlayedOnce = false;
    }
    void takenOffDelay()
    {
        takenOff = false;
    }
    void moveForwardDelay()
    {
        moveForward = false;
    }
    void doneMoveDelay()
    {
        doneMove = true;
    }
}
