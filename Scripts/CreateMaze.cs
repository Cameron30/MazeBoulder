using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMaze : MonoBehaviour {

    //player for checking when to load in
    public GameObject player;

    private bool created = false;

    private void Start()
    {
        //player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update () {

        

        //check to see if player is within bounds of buffer room
        if (player != null)
        {
            if (player.transform.position.x < transform.position.x + 10 && player.transform.position.x > transform.position.x - 10)
            {
                if (player.transform.position.z < transform.position.z + 10 && player.transform.position.z > transform.position.z - 10)
                {
                    if (!created)
                    {
                        Destroy(GameObject.Find("Base(Clone)"));
                        //creates floor object at appropriate location
                        var floor = (GameObject)Instantiate(Resources.Load("Base"));
                        floor.transform.position = new Vector3(transform.position.x + 92.5f, transform.position.y - 2.5f, transform.position.z + 75);
                        created = true;
                    }
                }
            }

        }
        
		
	}
}
