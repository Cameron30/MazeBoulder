using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTrigger : MonoBehaviour {

    public bool releasePlayer;
    public GameObject objectHave;
    public float moveCount;
    GameObject unlockWall;

	// Use this for initialization
	void Start () {
        moveCount = 0.0f;
        objectHave = null;
        releasePlayer = false;
        //unlockWall = transform.Find("Cage").transform.Find("Floor (4)").gameObject;
        unlockWall = transform.GetChild(0).GetChild(0).GetChild(3).gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if (releasePlayer)
        {
            // Unlock cage to free player
                
                // move wall up in the y direction
                unlockWall.transform.position = new Vector3(unlockWall.transform.position.x,
                                                            unlockWall.transform.position.y + 0.15f,
                                                            unlockWall.transform.position.z);

                ++moveCount;

                if (moveCount == 35.0f)
                {
                    moveCount = 0.0f;

                    // turn off collider once player interaction is finished
                    //transform.parent.GetComponent<BoxCollider>().enabled = false;

                    // destory key once it interacts with collider
                    //Destroy(transform.parent.parent.Find("Key Object").gameObject);


                    // reset flags
                    //transform.parent.GetComponent<KeyTrigger>().releasePlayer = false;
                    releasePlayer = false;
                }
        }
    }

    GameObject whatHaveIGotInMyPocket()
    {
        objectHave = 
            GameObject.Find("Player").transform.Find("OVRCameraRig").
            GetChild(0).Find("RightHandAnchor").GetComponent<Grabber>().
            grabbedObject;
        return objectHave;
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            if (whatHaveIGotInMyPocket().name == "Key Object")
            {
                //releasePlayer = true; who cares
                //Destroy(transform.gameObject);
                // turn off cage trigger's box collider.
                transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = false;
                // turn off lock
                transform.gameObject.GetComponent<BoxCollider>().enabled = false;
                transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
                // turn off door
                //transform.GetChild(0).GetChild(0).GetChild(3).gameObject.GetComponent<BoxCollider>().enabled = false;
                //transform.GetChild(0).GetChild(0).GetChild(3).gameObject.GetComponent<MeshRenderer>().enabled = false;
                // turn off force on the door.
                unlockWall.GetComponent<ConstantForce>().enabled = false;
                //destroy key
                Destroy(GameObject.Find("Key Object").gameObject);
                GameObject.Find("Player").transform.Find("OVRCameraRig").
                    GetChild(0).Find("RightHandAnchor").GetComponent<Grabber>().grabbed = false;
                releasePlayer = true;
            }
        }

        /*if (other.name == "Key Object") //&&
        //OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            releasePlayer = true;
        }*/
    }
    
}
