using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cagedudtrap : MonoBehaviour
{

    private bool releasePlayer;

    //private float moveCount;

    // Use this for initialization
    void Start()
    {

        //releasePlayer = transform.parent.GetComponent<KeyTrigger>().releasePlayer;

        //moveCount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            var cage = transform.Find("Cage").gameObject;
            //var key = transform.parent.parent.Find("Key Object").gameObject;

            cage.SetActive(true);
            //key.SetActive(true);

            // enable key collider once player steps into trap
            //transform.parent.GetComponent<BoxCollider>().enabled = true;
            //transform.parent.GetComponent<MeshRenderer>().enabled = true;

            // disable cage trigger collider once cage has been dropped
            transform.GetComponent<BoxCollider>().enabled = false;


            //releasePlayer = true;
        }
    }
}
