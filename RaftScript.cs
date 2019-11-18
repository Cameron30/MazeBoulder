using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftScript : MonoBehaviour
{

    private bool moveRaft;
    private bool controllerMode;
    //private bool raftFlag;
    private bool playerDead;
    private bool engageTrap;

    private GameObject raft;
    private GameObject controller;
    private GameObject cube;
    private GameObject hookObject;
    private GameObject player;
    private GameObject grapple;
    private GameObject swingWeight;
    private GameObject hook;
    private GameObject flyA;
    private GameObject flyB;
    private GameObject flyC;

    private LineRenderer line;

    //private int playerHealth;

    float speed;
    float step;

    private Vector3 finalPosition;
    private Vector3 pathPosition;

    private Quaternion pathRotation;

    // Use this for initialization
    void Start()
    {

        //player = GameObject.Find("Player");

        GrabRaftObjects();

        // grab raft path position and orientation
        pathPosition = transform.position;
        pathRotation = transform.rotation;

        // set speed/step
        speed = 1.5f;
        step = speed * Time.deltaTime;

        finalPosition = Vector3.zero;

        // set bools
        moveRaft = false;
        controllerMode = false;
        //raftFlag = false;
        playerDead = false;
        engageTrap = false;

        //SpawnRaftPath();
    }

    void GrabRaftObjects()
    {
        // grab controller properties
        player = GameObject.Find("Player");
        controller = player.transform.Find("OVRCameraRig").transform.Find("TrackingSpace").transform.Find("RightHandAnchor").gameObject;

        // grab player health value
        //playerHealth = player.transform.Find("HUD").GetComponent<Health>().health;

        // grab raft properties
        raft = GameObject.Find("Raft Trap").transform.Find("Raft").gameObject;

        // grab reference point
        hookObject = GameObject.Find("Raft Trap").transform.Find("Raft Hookable Objects").transform.Find("Hook Object").gameObject;
        cube = hookObject.transform.Find("Reference Point").gameObject;

        // grab fly objects
        flyA = transform.parent.parent.Find("MosquitoZone").transform.Find("mosquito0").gameObject;
        flyB = transform.parent.parent.Find("MosquitoZone").transform.Find("mosquito1").gameObject;
        flyC = transform.parent.parent.Find("MosquitoZone").transform.Find("mosquito2").gameObject;
    }

    void SpawnGun()
    {
        // spawn in new grapple gun
        GameObject newGrapple = (GameObject)Instantiate(Resources.Load("Grappling Gun"));

        newGrapple.transform.position = controller.transform.position;
        newGrapple.transform.rotation = controller.transform.rotation;
        newGrapple.transform.parent = controller.transform;

        // BUG: for some reason player is not actually grabbing onto grappling gun 
        // this line should make player grab onto gun but not working
        controller.GetComponent<Grabber>().grabbed = true;
    }

    void SpawnRaftPath()
    {
        // spawn in new raft path
        GameObject newPath = (GameObject)Instantiate(Resources.Load("Raft Trap"));

        // fix the new raft path's position and rotation to the old one
        newPath.transform.position = pathPosition;
        newPath.transform.rotation = pathRotation;

    }

    // Update is called once per frame
    void Update()
    {

        //controller.GetComponent<Grabber>().grabbed = true;
        //var health = player.GetComponent<Health>().health;

        if (controllerMode)
        {
            line.SetPosition(1, swingWeight.transform.position);
            if (controller.transform.localEulerAngles.x <= 90 && controller.transform.localEulerAngles.x >= 0)
            {
                moveRaft = true;
                //playerHealth = player.transform.Find("HUD").GetComponent<Health>().health;
                //playerDead = flyA.GetComponent<mosquitoSwarm>().playerDead;
                //playerDead = flyB.GetComponent<mosquitoSwarm>().playerDead;
                //playerDead = flyC.GetComponent<mosquitoSwarm>().playerDead;

                if (flyA.GetComponent<mosquitoSwarm>().playerDead || flyB.GetComponent<mosquitoSwarm>().playerDead
                    || flyC.GetComponent<mosquitoSwarm>().playerDead)
                {
                    //controllerMode = false;
                    playerDead = true;
                    //moveRaft = false;

                }
                    //playerDead = true;
            }
        }

        if (moveRaft)
        {

            raft.transform.position = Vector3.MoveTowards(raft.transform.position, cube.transform.position, step);

            // make hook's x rotation follow the controller's position
            hook.transform.localRotation = Quaternion.Euler(player.transform.position.x,
                                                            hook.transform.localEulerAngles.y,
                                                            player.transform.position.z);

            line.SetPosition(1, swingWeight.transform.position);

            //raftFlag = true;

            //playerHealth = player.transform.Find("HUD").GetComponent<Health>().health;
            //playerDead = flyA.GetComponent<mosquitoSwarm>().playerDead;
            //playerDead = flyB.GetComponent<mosquitoSwarm>().playerDead;
            //playerDead = flyC.GetComponent<mosquitoSwarm>().playerDead;

            if (flyA.GetComponent<mosquitoSwarm>().playerDead || flyB.GetComponent<mosquitoSwarm>().playerDead
                || flyC.GetComponent<mosquitoSwarm>().playerDead)
                playerDead = true;
        }

        if (engageTrap)
        {
            if (raft.transform.position == cube.transform.position)
            {
                player.transform.parent = null;
                // also delete hook and swing weight
                swingWeight.transform.parent = null;
                Destroy(swingWeight);
                Destroy(hookObject.transform.Find("Reference Point").gameObject);
                //hookObject.transform.Find("Reference Point").gameObject.SetActive(false);
                //swingWeight.SetActive(false);
                //hookObject.SetActive(false);
                // delete the current log and reference point
                Destroy(hookObject);

                // reset flags
                moveRaft = false;
                //raftFlag = false;
                controllerMode = false;
                // set parent of player to nothing.
                //GameObject.Find("Player").transform.parent = null;
                // learned not to destroy the player during runtime lol
                SpawnGun(); // function spawns new gun in hand

                //playerHealth = player.transform.Find("HUD").GetComponent<Health>().health;
                //playerDead = flyA.GetComponent<mosquitoSwarm>().playerDead;
                //playerDead = flyB.GetComponent<mosquitoSwarm>().playerDead;
                //playerDead = flyC.GetComponent<mosquitoSwarm>().playerDead;
                if (flyA.GetComponent<mosquitoSwarm>().playerDead || flyB.GetComponent<mosquitoSwarm>().playerDead
                    || flyC.GetComponent<mosquitoSwarm>().playerDead)
                    playerDead = true;
            }
        }
       

        // flag is enabled if player's current health is zero
        //if (player.transform.Find("HUD").GetComponent<Health>().health <= 0)
        //{
          //  playerDead = true;
            //Destroy(player);
        //}



        // reset raft trap if player dies 
        if (playerDead)
        {
            // TODO: Fix later, does not work atm
            /*
            //Destroy(player);
            // TODO: should reset raft trap when ALL players ded, not just 1

            // disassociate player and controller before destroying raft trap
            if (moveRaft || controllerMode)
            {
                player.transform.position = player.transform.position;
                player.transform.parent = null;

                swingWeight.transform.position = swingWeight.transform.position;
                swingWeight.transform.rotation = swingWeight.transform.rotation;
                swingWeight.transform.parent = null;
            }
                //player.transform.parent = null;
            //swingWeight.transform.parent = null;

                //swingWeight.transform.parent = null;

            // Destroy current raft trap
            Destroy(transform.parent.parent.gameObject);
            //Destroy(GameObject.Find("Raft Trap"));
            //Destroy(raft);

            // Spawn in new raft trap with same place
            SpawnRaftPath();
            GrabRaftObjects();

            // reset flags
            moveRaft = false;
            raftFlag = false;
            controllerMode = false;
            playerDead = false;
            */
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hook")
        {
            // Update raft object's properties
            GrabRaftObjects();

            // sets the x, y and z components of existing Vector3, which should start at Vector3.zero
            finalPosition.Set(cube.transform.position.x, raft.transform.position.y, cube.transform.position.z);

            // make player a child of the controller
            player.transform.parent = raft.transform;

            // destroy player's current grappling gun
            grapple = other.transform.parent.gameObject;
            Destroy(grapple);

            // make preset grapple on prefab visible
            hook = hookObject.transform.Find("Hook").gameObject;
            swingWeight = hookObject.transform.Find("Swing Weight").gameObject;
            var gunRotated = swingWeight.transform.Find("gun_rotated");

            hook.GetComponent<MeshRenderer>().enabled = true;
            gunRotated.GetComponent<MeshRenderer>().enabled = true;

            // put line renderer on hook and set positions
            line = hook.AddComponent<LineRenderer>();

            line.useWorldSpace = true;

            line.startWidth = 0.1f;
            line.endWidth = 0.1f;

            line.SetPosition(0, hook.transform.position);

            swingWeight.transform.position = controller.transform.position;
            swingWeight.transform.rotation = controller.transform.rotation;
            swingWeight.transform.parent = controller.transform;

            line.SetPosition(1, swingWeight.transform.position);

            //playerHealth = player.GetComponent<Health>().health;

            //moveRaft = true;

            //playerDead = flyA.GetComponent<mosquitoSwarm>().playerDead;
            //playerDead = flyB.GetComponent<mosquitoSwarm>().playerDead;
            //playerDead = flyC.GetComponent<mosquitoSwarm>().playerDead;

            if (flyA.GetComponent<mosquitoSwarm>().playerDead || flyB.GetComponent<mosquitoSwarm>().playerDead
                || flyC.GetComponent<mosquitoSwarm>().playerDead)
                playerDead = true;

            controllerMode = true;
            engageTrap = true;
        }
    }
}
