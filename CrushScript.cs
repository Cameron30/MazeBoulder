using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushScript : MonoBehaviour
{

    private bool startMove;

    private GameObject wallA;
    private GameObject wallB;
    private GameObject cube;
    private GameObject player;

    // Use this for initialization
    void Start()
    {

        wallA = transform.Find("Wall 1").gameObject;
        wallB = transform.Find("Wall 2").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        var wallAFlag = wallA.GetComponent<KillPlayer>().touchingPlayer;
        var wallBFlag = wallB.GetComponent<KillPlayer>().touchingPlayer;


        if (startMove)
        {
            float speed = 1.5f;
            float step = speed * Time.deltaTime;

            // Move Towards the reference point (same y axis direction)
            //wallA.transform.position = Vector3.MoveTowards(wallA.transform.position, cube.transform.position, step);
            //wallB.transform.position = Vector3.MoveTowards(wallB.transform.position, cube.transform.position, step);

            wallA.transform.position = Vector3.MoveTowards(wallA.transform.position,
                                                           new Vector3(cube.transform.position.x, 
                                                                       wallA.transform.position.y, 
                                                                       cube.transform.position.z),
                                                           step);

            wallB.transform.position = Vector3.MoveTowards(wallB.transform.position,
                                                           new Vector3(cube.transform.position.x,
                                                                       wallB.transform.position.y,
                                                                       cube.transform.position.z),
                                                           step);
        }
        
        if (wallAFlag && wallBFlag)
        {
            // this way works, var does not
            player.GetComponent<Health>().health = 0;
            wallA.GetComponent<KillPlayer>().touchingPlayer = false;
            wallB.GetComponent<KillPlayer>().touchingPlayer = false;
        }
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            // turn off the collider on trigger
            transform.GetComponent<BoxCollider>().enabled = false;

            startMove = true;

            cube = transform.Find("Reference Point").gameObject;

            player = other.gameObject;
            player = player.transform.Find("HUD").gameObject;

        }
    }
}
