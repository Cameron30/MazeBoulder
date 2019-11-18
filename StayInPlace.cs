using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInPlace : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.parent != null)
        {
            if (gameObject.name == "ItemSlots")
            {
                transform.position = transform.parent.position;
                transform.rotation = transform.parent.rotation;
            } else if (gameObject.name == "ItemSlot")
            {
                transform.rotation = transform.parent.rotation;
                transform.position = transform.parent.position + new Vector3(.12f, -.865f, .368f);

            } else if (gameObject.name == "ItemSlot(1)")
            {
                transform.rotation = transform.parent.rotation;
                transform.position = transform.parent.position + new Vector3(-.08f, -.645f, .288f);
            }
            else if (gameObject.name == "ItemSlot(2)")
            {
                transform.rotation = transform.parent.rotation;
                transform.position = transform.parent.position + new Vector3(-.2f, -.335f, .188f);
            }
        }
	}
}
