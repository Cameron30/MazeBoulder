using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveWallPart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.parent != null)
        {
            if (transform.parent.gameObject.name == "RightHandAnchor")
            {
                var grabber = transform.parent.GetComponent<Grabber>();

              

                //sets proper items for grabbing
                grabber.grabbed = true;

                //deletes this object
                Destroy(gameObject);
            }

        }
    }
}
