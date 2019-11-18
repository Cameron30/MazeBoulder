using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDamage : MonoBehaviour {

    public bool playerTouch;
    public bool addForceFlag;
    GameObject player;
    Transform referencePoint;
	// Use this for initialization
	void Start () {
        playerTouch = false;
        addForceFlag = false;
        player = GameObject.Find("Player").gameObject;
        referencePoint = GameObject.Find("/SlidingTrap/Trigger").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (playerTouch)
        {
            //player.GetComponent<Rigidbody>().AddForce(-500, 200, 100);
            playerTouch = false;
        }
	}

    void OnTriggerEnter(Collider other)
    {
   
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player")
        {
            // damage player for half of their health
            // 2 hits = ded
            var player = other.gameObject;

            var hud = player.transform.Find("HUD").gameObject;

            hud.GetComponent<Health>().health -= 250;

            playerTouch = true;


            
            // left rock (C)
            if (transform.tag == "Left")
            {
                player.GetComponent<Rigidbody>().AddForce(-500, 200, -50);
                addForceFlag = true;
            }
            // center rock (A)
            if (transform.tag == "Center")
            {
                player.GetComponent<Rigidbody>().AddForce(-500, 200, Random.Range(-50f,50f));
                addForceFlag = true;
            }
            // right rock (B)
            if (transform.tag == "Right")
            {
                player.GetComponent<Rigidbody>().AddForce(-500, 200, 50);
                addForceFlag = true;
            }
        }
    }
}
