using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePotion : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (transform.parent != null)
        {
            if (transform.parent.gameObject.name == "RightHandAnchor")
            {
                
                // Spawn in Health Potion
                if (gameObject.tag == "Red")
                {
                    var grabber = transform.parent.GetComponent<Grabber>();
                    var potion = (GameObject)Instantiate(Resources.Load("HealthPotion"));
                    var placeable = potion.GetComponent<Placeable>();

                    var body = transform.parent.parent.parent.parent;
                    placeable.itemSlots = body.Find("ItemSlots").gameObject;

                    //sets proper items for grabbing
                    grabber.grabbed = true;
                    potion.transform.SetParent(transform.parent);
                    grabber.grabbedObject = potion;

                    potion.transform.position = transform.position;
                    potion.transform.rotation = transform.rotation;

                    // disable ItemTrigger script on flower
                    //transform.GetComponent<ItemTrigger>().enabled = false;
                    Destroy(transform.GetComponent<ItemTrigger>());
                    //deletes this object
                    Destroy(gameObject);
                }
                // Spawn in Poop Potion
                if (gameObject.tag == "Blue")
                {
                    var grabber = transform.parent.GetComponent<Grabber>();
                    var potion = (GameObject)Instantiate(Resources.Load("PoopPotion"));
                    var placeable = potion.GetComponent<Placeable>();

                    var body = transform.parent.parent.parent.parent;
                    placeable.itemSlots = body.Find("ItemSlots").gameObject;

                    //sets proper items for grabbing
                    grabber.grabbed = true;
                    potion.transform.SetParent(transform.parent);
                    grabber.grabbedObject = potion;

                    potion.transform.position = transform.position;
                    potion.transform.rotation = transform.rotation;

                    // disable ItemTrigger script on flower
                    //transform.GetComponent<ItemTrigger>().enabled = false;
                    Destroy(transform.GetComponent<ItemTrigger>());

                    //deletes this object
                    Destroy(gameObject);
                }
                
            }
            
        }
	}
}
