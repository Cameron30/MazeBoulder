using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mosquitoSwarm : MonoBehaviour {
    // this code is also tacked onto the plant thing
    public GameObject player;
    public Transform target; // target location (player)
    private float speed; // mosquito flight speed.
    private bool inCloud; // when the player is in the cloud
    private bool droneIntruder; // when the drone is in the cloud
    private int frameNum; // frame counter
    private Vector3 initialPosition;
    private GameObject drone;

    private int frameCount;

    private Vector3 offset;

    public bool playerDead;

    // Use this for initialization
    void Start () {
        inCloud = false;
        droneIntruder = false;
        player = GameObject.Find("Player");
        target = player.transform;
        speed = 2.0f;
        frameNum = 0;
        initialPosition = transform.position;
        playerDead = false;
        frameCount = 0;
        offset = new Vector3(0, -90, 0);
        drone = GameObject.Find("Drone");
	}
	
	// Update is called once per frame
	void Update () {
        /*
        ++frameCount;
        // TODO rotate towards player lol
        // too bad these lines just makes the mosquito spaz to hell
        if (frameCount > 1 * 60)
        {
            transform.forward = Vector3.RotateTowards(transform.forward, target.position,
                                                        -1 * 20.0f * Time.deltaTime, 0.0f);
            frameCount = 0;
        }*/

        //Quaternion.LookRotation(target.transform.position - transform.position);
        transform.LookAt(player.transform);
        transform.Rotate(offset); // orientate monster correctly


        if (inCloud)
        {
           frameNum += 1;
            if (frameNum == 60 * 2) // in theory, deals damage per few seconds, knowing that 60 frames = 1s
            {
                player.GetComponent<Health>().health -= 85;
                frameNum = 0;
            }
            //player.GetComponent<Health>().health -= (int)(2*Time.deltaTime); // hm....
        }
        /*if (!droneIntruder)
        {
            transform.position = Vector3.MoveTowards(transform.position, 
                target.position, 
                speed * Time.deltaTime);
        }
        else if (droneIntruder)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                target.position, -1 * speed * Time.deltaTime); // a negative vector means it moves away
        }
        */
        /*
        if (Vector3.Distance(drone.transform.position, transform.position) < 30f && drone.GetComponent<DroneFly>().takenOff)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                                     drone.transform.position + new Vector3(1, -1, 1),
                                                     speed * Time.deltaTime);
            transform.LookAt(drone.transform);
        }
        else
        {
        */
            transform.position = Vector3.MoveTowards(transform.position,
                                                     target.position+new Vector3(-1,0,-1),
                                                     (speed/4) * Time.deltaTime);
            transform.LookAt(player.transform);
        //}

        if (GameObject.Find("Poop Trap") == null)
            Destroy(gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            inCloud = true;
            player = other.gameObject;
            player = player.transform.Find("HUD").gameObject;

            if (player.GetComponent<Health>().health <= 0)
            {
                // notify player has died
                playerDead = true;
                // reset monster position and set inactive
                transform.position = initialPosition;
                inCloud = false;
                transform.parent.Find("spoopyplantmonster").gameObject.SetActive(false);
                // disable poop particles when player dies
                //transform.parent.Find("Poop").GetComponent<ParticleSystem>().enableEmission = false;
            }
            
        }
        /*
        else if (other.name == "Drone")
        {
            droneIntruder = true;
            player = other.gameObject; // the "player" in this line is actually just the drone.

        }*/
    }
    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            inCloud = false;
        }
        /*if (other.name == "Drone")
        {
            droneIntruder = false;
        }*/
    }

    public Vector3 getInitPos()
    {
        return initialPosition;
    }

    public void randomInitPos()
    {
        initialPosition.x += Random.Range(-120, 120);
        initialPosition.z += Random.Range(-120, 120);
    }
}
