using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BoulderTrigger : MonoBehaviour {

    //private GameObject player;
    private GameObject healthObject;
    private GameObject boulder;
    private int numPlayers;
    private int totPlayers;
    private bool isGone;

    public GameObject gun;
    public bool playerDied = false;
    public bool sync = false;

	// Use this for initialization
	void Start () {
        boulder = transform.parent.Find("Boulder").gameObject;
        isGone = false;
        gun = GameObject.Find("Grappling Gun");
    }
	
	// Update is called once per frame
	void Update () {

        if (boulder.GetComponent<KillPlayer>().touchingPlayer)
        {
            Debug.Log("[BoulderTrigger]:  Player is ded");
            playerDied = true;
            healthObject.GetComponent<Health>().health = 0;
            boulder.GetComponent<KillPlayer>().touchingPlayer = false;

            // reset flags if player is swinging on the rope
            Debug.Log("playerDied = " + playerDied);
        }
        // null reference exception when boulder is gone (FIXED i think)
        if (!isGone)
        {
            if (boulder.transform.position.y <= -100)
            {
                //boulder.GetComponent<MeshRenderer>().enabled = false;
                //boulder.GetComponent<Rigidbody>().isKinematic = true;
                boulder.SetActive(false);
                isGone = true;
            }
        }

        var remotes = GameObject.FindGameObjectsWithTag("Remote");

        numPlayers = 0;

        foreach (GameObject remote in remotes)
        {
            if (Math.Abs(remote.transform.position.x - transform.position.x) < 25.25f && Math.Abs(remote.transform.position.z - transform.position.z) < 2.75f &&
                Math.Abs(remote.transform.position.y - transform.position.y) < 3f)
            {
                sync = true;
                numPlayers = numPlayers + 1;
            }
        }

        totPlayers = remotes.Length;

    }

    private void OnTriggerStay(Collider other)
    {

        //Debug.Log(other.name);
        if (other.name == "Player")
        {
            sync = true;
            if (totPlayers == numPlayers)
            {
                boulder.SetActive(true);
            }
           
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (other.name == "Player")
        {
            healthObject = other.gameObject.transform.Find("HUD").gameObject;
            Debug.Log("[BoulderTrigger: OnTriggerStay] Player");

        }
        if (other.name == "Boulder")
            other.GetComponent<Rigidbody>().useGravity = !other.GetComponent<Rigidbody>().useGravity;

    }
}
