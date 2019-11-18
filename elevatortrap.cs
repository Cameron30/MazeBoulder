using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevatortrap : MonoBehaviour {
    public int playerCount; // how many players
    public int speed;
    Transform target; // bop bop bop, bop to the top
    List<Transform> fallBlocks;
    int framecount;

	// Use this for initialization
	void Start () {
        playerCount = 0;
        speed = 34;
        target = GameObject.Find("/ElevatorTrap/the top").transform;
        fallBlocks = new List<Transform>();
        foreach (Transform child in GameObject.Find("ElevatorTrap").transform)
        {
            if (child.name == "FallBlocks")
            {
                foreach (Transform gChild in child)
                {
                    fallBlocks.Add(gChild);
                }
            }
        }
    
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if (playerCount > 0) // when the amount of players is enough:
        {
            foreach (Transform fallB in fallBlocks)
            {
                // FAAAAALL
                foreach (Transform ground in fallB)
                {
                    ground.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            framecount++;
            if (framecount%20 == 0 && !(speed <= 0) && transform.position.y < target.position.y - 5)
            {
                speed--;
            }
            else if (transform.position.y > target.position.y - 5)
            {
                framecount = 0;
            }
            DestroyBlocks(fallBlocks);
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            // Set player's parent to the cube so that player can move with the cube
            collision.transform.SetParent(transform);
            // increase player count
            playerCount++;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            // Clear player's parent when player leaves cube
            collision.transform.SetParent(null);
            // decrease player count
            playerCount--;
        }
    }

}
