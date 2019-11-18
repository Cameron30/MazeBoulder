using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorWallScript : MonoBehaviour {

    private bool startMove;

    private GameObject wallA;
    private GameObject wallB;
    private GameObject cubeA;
    private GameObject cubeB;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (startMove)
        {
            // move both walls slowly
            float speed = 0.3f;
            float step = speed * Time.deltaTime;

            // Move Towards the reference point (same y axis direction)
            //wallA.transform.position = Vector3.MoveTowards(wallA.transform.position, cube.transform.position, step);
            //wallB.transform.position = Vector3.MoveTowards(wallB.transform.position, cube.transform.position, step);

            wallA.transform.position = Vector3.MoveTowards(wallA.transform.position,
                                                           new Vector3(cubeA.transform.position.x,
                                                                       wallA.transform.position.y,
                                                                       cubeA.transform.position.z),
                                                           step);

            wallB.transform.position = Vector3.MoveTowards(wallB.transform.position,
                                                           new Vector3(cubeB.transform.position.x,
                                                                       wallB.transform.position.y,
                                                                       cubeB.transform.position.z),
                                                           step);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            startMove = true;

            // turn off corridor trigger
            transform.GetComponent<BoxCollider>().enabled = false;

            //var crushWall = transform.Find("Crushing Wall Trap");

            // find the walls and reference points for the walls to move 
            wallA = transform.parent.Find("Crushing Wall Trap").transform.Find("Wall 1").gameObject;
            wallB = transform.parent.Find("Crushing Wall Trap").transform.Find("Wall 2").gameObject;
            cubeA = transform.parent.Find("Crushing Wall Trap").transform.Find("Reference Point A").gameObject;
            cubeB = transform.parent.Find("Crushing Wall Trap").transform.Find("Reference Point B").gameObject;
        }    
    }


}
