using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingHinge : MonoBehaviour {

    private GameObject player;
    private GameObject moveScript;
    private bool readyJump;
    private bool onRope;
    private bool lerping; // when player is moving
    private bool gravity; // when gravity is on, this bool is false.
    private bool colliderCheck;
    private bool flag;
    //private int framecount;

    public GameObject triggerReference;
    public bool swinging = false;
    bool oneGrab = false;
    bool doubleGrab = false;

    //Audio stuff
    AudioSource m_MyAudioSource;
    bool isPlaying = false;

    void Start()
    {
        // acquires the properties of the player for transformations
        player = GameObject.Find("Player");
        moveScript = player.transform.Find("OVRCameraRig").gameObject;
        //var trigger = transform.parent.parent.parent.parent.parent.Find("BoulderTrigger").gameObject;

        flag = false;
        //framecount = 0;

        /* Audio */
        m_MyAudioSource = GetComponent<AudioSource>();

    }


    // Update is called once per frame
    void FixedUpdate () {

        // hold onto the rope 
        //if (swinging && colliderCheck)
        if (swinging)
        {
            // turn off running
            moveScript.GetComponent<PlayerMovement>().running = false;

            // player movement while on the rope
            // make player the child of the grappling gun
            player.transform.parent = transform.Find("Swing Weight");
            player.transform.position = new Vector3(transform.position.x,
                                                    transform.position.y,
                                                    transform.position.z);

            // disable player movement temporarily
            moveScript.GetComponent<PlayerMovement>().enabled = false;

            // turn off player gravity
            player.transform.GetComponent<Rigidbody>().useGravity = false; // make this false?

            readyJump = true;
            onRope = true;

            /***************Audio Rope Swing ***************/

            // NOTE: audio was playing and stopping quickly before,
            //  so moved stopping noise to where the player jumps off the rope
            //Adding footsteps sounds
            if (/*OVRInput.Get(OVRInput.Touch.PrimaryTouchpad) &&*/ !isPlaying)
            {
                m_MyAudioSource.Play();
                isPlaying = true;
                Debug.Log("Rope is swinging");
            }
            /*else
            {
                m_MyAudioSource.Stop();
                isPlaying = false;
                Debug.Log("Rope stopped swinging");
            }*/

            // debug statement
            if (triggerReference.GetComponent<BoulderTrigger>().playerDied)
              Debug.Log("trig ref = " + triggerReference.GetComponent<BoulderTrigger>().playerDied);
        }

        // movement of the player
        if (lerping)
        {
            player.GetComponent<Rigidbody>().mass = 1;
            player.transform.GetComponent<Rigidbody>().useGravity = true;

            gravity = true;
            lerping = false;
            /*playerRotation = (int)player.transform.localEulerAngles.y;
            // do mod function check rotation below or above 360s
            playerRotation %= 360;
            // move player according to their orientation
            
            // last set rope positions (-39.5, 0.26, 0)
            if (playerRotation > 315 && playerRotation <= 45)
            {
                //player.transform.position = new Vector3(player.transform.position.x,
                //                                    player.transform.position.y + 0.1f,
                //                                    player.transform.position.z - 0.1f);
                //player.GetComponent<Rigidbody>().AddForce(0, 0, -8, ForceMode.Impulse);
                  //player.GetComponent<Rigidbody>().AddForce(0, 100, -500, ForceMode.Force);

            }
            if (playerRotation > 45 && playerRotation <= 135)
            {
                //player.transform.position = new Vector3(player.transform.position.x + 0.1f,
                //                                    player.transform.position.y + 0.1f,
                //                                    player.transform.position.z);
                //player.GetComponent<Rigidbody>().AddForce(8, 0, 0, ForceMode.Impulse);
                //player.GetComponent<Rigidbody>().AddForce(500, 100, 0, ForceMode.Force);
            }
            if (playerRotation > 135 && playerRotation <= 225)
            {
                //player.transform.position = new Vector3(player.transform.position.x,
                //                                    player.transform.position.y + 0.1f,
                //                                    player.transform.position.z + 0.1f);
                //player.GetComponent<Rigidbody>().AddForce(0, 0, 8, ForceMode.Impulse);
                  //player.GetComponent<Rigidbody>().AddForce(0, 100, 500, ForceMode.Force);
            }
            if (playerRotation > 225 && playerRotation <= 315)
            {
                //player.transform.position = new Vector3(player.transform.position.x - 0.1f,
                //                                    player.transform.position.y + 0.1f,
                //                                    player.transform.position.z);
                //player.GetComponent<Rigidbody>().AddForce(-8, 0, 0, ForceMode.Impulse);
                  //player.GetComponent<Rigidbody>().AddForce(-500, 100, 0, ForceMode.Force);
            }*/

            /*
            player.transform.position = new Vector3(player.transform.position.x,
                                                    player.transform.position.y + 0.1f,
                                                    player.transform.position.z);
                                                    */
        }
        // turn on gravity once lerping is done
        if (gravity && !lerping)
        {
            gravity = false;
            colliderCheck = false;          
        }

        // jump off the rope-----------------------------------------------------------
        // Press trigger while on the rope
        //if (framecount != 0)
        if (!oneGrab && !doubleGrab)
        {
            //framecount = 0;
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)
                && swinging && readyJump && onRope && colliderCheck)
            {
                Debug.Log("[Jumping off rope branch]");
                //Debug.Log("!oneGrab && !doubleGrab");
                // disassociate the relation between the player and its parent (grappling gun)
                player.transform.parent = null;

                lerping = true;

                // enable player movement after jumping off
                moveScript.GetComponent<PlayerMovement>().enabled = true;

                swinging = false;
                readyJump = false;
                onRope = false;

                // stop audio noise here
                Debug.Log("Stopping audio noise for rope");
                m_MyAudioSource.Stop();
                isPlaying = false;
                // this is supposed to make it so you can't grab onto the thing again?
                oneGrab = true;
                Invoke("oneGrabDelay", 0.5f);
                // prevents jump off branch from happening immediately
                doubleGrab = true;
                Invoke("doubleGrabDelay", 0.5f);
            }

        }

        // makes player let go of the rope if player dies on the rope
        if (triggerReference.GetComponent<BoulderTrigger>().playerDied
            && swinging)
        {
            Debug.Log("[SwingHinge: Update] player ded on rope");
            //Debug.Log("trigger ref bool = " + triggerReference.GetComponent<BoulderTrigger>().playerDied);

            // reset all flags
            swinging = false;
            readyJump = false;
            onRope = false;
            //jumpOff = false;
            colliderCheck = false;

            oneGrab = false;
            doubleGrab = false;

            // stop audio noise when player dies
            m_MyAudioSource.Stop();
            isPlaying = false;

            // turn gravity back on
            player.transform.GetComponent<Rigidbody>().useGravity = true;
            //var moveScript = player.transform.Find("OVRCameraRig");
            moveScript.GetComponent<PlayerMovement>().enabled = true;

            // make player let go of rope
            player.transform.parent = null;

            // reset bool flag on other script
            triggerReference.GetComponent<BoulderTrigger>().playerDied = false;
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        // u cant grab anything when stuff in ur hand owo
        var grabber = other.gameObject.GetComponent<Grabber>();
        if (grabber != null)
        {
            var grabbed = grabber.grabbed;
            oneGrab = true;
            // Pressing Trigger
            //Debug.Log(framecount);
            //if (framecount == 0)
            if (oneGrab && !doubleGrab)
            {
                //Invoke("oneGrabDelay", 0.5f);
                //framecount++;
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)
                    && !swinging && !readyJump && !grabbed && !colliderCheck && !flag)
                {
                    //Debug.Log("[SwingHinge: OnTriggerStay] oneGrab && !doubleGrab");
                    Debug.Log("GRAB :O");
                    //framecount++;
                    flag = true;
                    swinging = true;
                    colliderCheck = true;

                    player = GameObject.Find("Player");
                    //playerRotation = (int)player.transform.localEulerAngles.y;
                    //Debug.Log(framecount);
                    //oneGrab = false;

                    // prevents jump off branch from happening immediately
                    doubleGrab = true;
                    Invoke("doubleGrabDelay", 0.5f);
                }
                else
                {
                    flag = false;
                }
                oneGrab = false;
            }
        }
    }

    void oneGrabDelay()
    {
        oneGrab = false;
    }
    void doubleGrabDelay()
    {
        doubleGrab = false;
    }
}
