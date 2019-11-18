using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerOnPlatform : MonoBehaviour {
    GameObject player;
    //GameObject target;
    //Vector3 offset;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").gameObject;
        //target = null;
	}
    /*
	// Update is called once per frame
	void Update () {
        //ᕕ( ᐛ )ᕗ
    }

    */
    /*
    void LateUpdate()
    {
        if (target != null)
        {
            target.transform.position = transform.position + offset;
        }    
    }*/


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            // Set player's parent to the cube so that player can move with the cube
            collision.transform.SetParent(transform);

            //collision.transform.position = transform.position - collision.transform.position;

            // offset target position attempt
            //target = collision.gameObject;
            //offset = target.transform.position - transform.position;
            

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            // Clear player's parent when player leaves cube
            collision.transform.SetParent(null);

            //target = null;

            // TODO: make player move in sync with platform, regardless of where the player is on the platform

            // player's parent is set to null when player moves on platform
            // some situations where player is NOT moving in sync with platform
            // player's position gravitiates towards the middle
                // far right side of cube, player DOES NOT move with cube
                    //------ when player gets towards the middle of cube, player starts moving with cube
                // far left side of cube, player DOES NOT move with cube
                    //------ player falls off the cube due to cube moving to the right
        }
    }
}
