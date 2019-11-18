using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour {

    private bool releasePlayer;

    private float moveCount;

	// Use this for initialization
	void Start () {

        //releasePlayer = transform.parent.GetComponent<KeyTrigger>().releasePlayer;

        moveCount = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        // TODO remove redundant code (some parts overlap with KeyTrigger.cs)
        // if player finds key and puts it in key collider
		//if (transform.parent.GetComponent<KeyTrigger>().releasePlayer)
        if (releasePlayer)
        {
            if (transform.parent.GetComponent<KeyTrigger>().releasePlayer)
            {
                /*
                 *             player.transform.position = new Vector3(player.transform.position.x,
                                                        player.transform.position.y + 0.1f,
                                                        player.transform.position.z);
                ++moveCount;

                // the following if statement should never run.
                // moveCount = 10 means player moves up 1f in y direction
                if (moveCount == 20.0f)
                {
                    gravity = true;
                    lerping = false;
                    moveCount = 0.0f;
                }
                 */

                // make key stick to key collider
                var key = transform.parent.parent.Find("Key Object").gameObject;

                // freeze position of the key and make it invisible
                key.transform.position = key.transform.position;
                key.GetComponent<MeshRenderer>().enabled = false;

                //key.transform.parent = transform.parent;
                /*
                // Unlock cage to free player
                var unlockWall = transform.Find("Cage").transform.Find("Floor (4)").gameObject;

                // move wall up in the y direction
                unlockWall.transform.position = new Vector3(unlockWall.transform.position.x,
                                                            unlockWall.transform.position.y + 0.15f,
                                                            unlockWall.transform.position.z);

                ++moveCount;

                if (moveCount == 35.0f)
                {
                    moveCount = 0.0f;

                    // turn off collider once player interaction is finished
                    transform.parent.GetComponent<BoxCollider>().enabled = false;

                    // destory key once it interacts with collider
                    Destroy(transform.parent.parent.Find("Key Object").gameObject);


                    // reset flags
                    transform.parent.GetComponent<KeyTrigger>().releasePlayer = false;
                }*/

            }
        }

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            var cage = transform.Find("Cage").gameObject;
            var key = transform.parent.parent.Find("Key Object").gameObject;

            cage.SetActive(true);
            key.SetActive(true);

            // enable key collider once player steps into trap
            transform.parent.GetComponent<BoxCollider>().enabled = true;
            transform.parent.GetComponent<MeshRenderer>().enabled = true;

            // disable cage trigger collider once cage has been dropped
            transform.GetComponent<BoxCollider>().enabled = false;


            releasePlayer = true;
        }
    }
}
