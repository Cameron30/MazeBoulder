using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformPuzzle : MonoBehaviour {
    List<Transform> fallBlocks;                   // a list for blocks that will fall
    List<Transform> rotateBlocks;                 // a list for blocks that will rotate around axis
    List<Transform> slugs;                        // a list for SLUGS
    public int playerCount;                       // how many players are in the trigger
    public bool playersReady;                     // GAME START
    Vector3 axis;                                 // the axis that the rotateBlocks spin around
    int frameCount;                               // count frames for interval stuff
    int speed;                                    // how fast thing move
	// Use this for initialization
	void Start () {
        // initalizing stuff
        fallBlocks = new List<Transform>();
        rotateBlocks = new List<Transform>();
        slugs = new List<Transform>();
        playerCount = 0; // count of players in area
        frameCount = 0;
        speed = 0;
        playersReady = false;
        axis = transform.Find("AXISBALL").position;
        // for loop to add the appropriate 10x10s to their lists
		foreach (Transform child in transform)
        {
            if (child.name == "FallBlocks")
            {
                foreach (Transform gChild in child)
                {
                    fallBlocks.Add(gChild);
                    foreach (Transform ground in gChild)
                    {
                        if (ground.name.EndsWith("r"))
                          ground.GetComponent<Rigidbody>().mass = 50000;
                    }
                }
            }
            
            else if (child.name == "RotateBlocks")
            {
                foreach (Transform gChild in child)
                {
                    rotateBlocks.Add(gChild);
                    
                }

            }
            else if (child.name == "Slugs")
            {
                foreach (Transform gChild in child)
                {
                    slugs.Add(gChild);
                    gChild.gameObject.SetActive(false);
                }
            }// dont care about endblocks lololol
        }
	}

	// Update is called once per frame
	void FixedUpdate () {
        if (playerCount > 0) // TODO: change count when multiplayer
        {
            playersReady = true;
        }
		if (playersReady == true)
        {
            frameCount += 1;
            foreach (Transform fallB in fallBlocks)
            {
                // FAAAAALL
                foreach (Transform ground in fallB)
                {
                    //if (ground.name.EndsWith("r"))
                      //  ground.GetComponent<Rigidbody>().mass =50000;
                    ground.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
            foreach (Transform sluggo in slugs)
            {
                // FAAAAALL MAGGOOOOTS
                sluggo.gameObject.SetActive(true);
                if (sluggo.name.EndsWith("3)"))
                {
                    sluggo.RotateAround(axis, Vector3.up, speed * Time.deltaTime);
                } else
                {
                    sluggo.RotateAround(axis, Vector3.up, -speed * Time.deltaTime);
                }
            }
            if (frameCount == 60 * 9)
            {
                speed = 5;
            }
            if (frameCount == 60 * 18)
            {
                speed = 0;
                frameCount = 0;
            }
            foreach (Transform rotateB in rotateBlocks)
            {

                // SPINNNNNN
                if (rotateB.name.EndsWith("1)"))
                    /*|| rotateB.name.EndsWith("5)") || rotateB.name.EndsWith("8)")*/
                {
                    rotateB.RotateAround(axis, Vector3.up, speed * Time.deltaTime);

                }
                else
                {
                    rotateB.RotateAround(axis, Vector3.up, -speed * Time.deltaTime);

                }

            }

        }

        // Destroy the falling blocks that fall below a certain altitude
        DestroyBlocks(fallBlocks);
	}

    private void DestroyBlocks(List<Transform> blockList)
    {
        Transform block = null;
        foreach (Transform fallB in fallBlocks)
        {
            int counter = 0;
            foreach (Transform ground in fallB)
            {
                if (ground.position.y < -50)
                {
                    ground.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    counter++;
                }
                if (counter == 4)
                {
                    block = fallB;
                }
            }
        }
        if (block != null)
        {
            blockList.Remove(block);
            Destroy(block.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            playerCount += 1;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            playerCount -= 1;
        }
    }
}
