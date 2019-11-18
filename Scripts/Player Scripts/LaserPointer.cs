using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour {

    //public Material LaserMat;
    //public Material HighlightMat;
    public float rayDistance;
    
    //private Collider savedCol;

    public bool active;
	// Use this for initialization
	void Start () {
        //creates line renderer with material and width
        /*
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = LaserMat;
        lineRenderer.widthMultiplier = .005f;
        lineRenderer.positionCount = 0;
        */
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //gets line renderer created at start
        //LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();

        //if trigger is pressed and not grabbing anything
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && !GetComponent<Grabber>().grabbed)
        {
            active = true;
        } 

        //if actively pointing
        if (active)
        {
            var contPos = transform.position;
            var contOrient = transform.rotation;

            //create line renderer
            //lineRenderer.positionCount = 2;
            //lineRenderer.SetPosition(0, transform.position);

            //create variables for use in raycast
            RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);

            

            if (Physics.Raycast(transform.position, fwd, out hit, rayDistance))
            {
                //lineRenderer.SetPosition(1, transform.forward * hit.distance + transform.position);

                //savedCol = hit.collider;
                
                //check if you can pick up object
                var placeable = hit.collider.gameObject.GetComponent<Placeable>();

                if (placeable != null)
                {
                    //lineRenderer.material = HighlightMat;

                    if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
                    {
                        //initiate all the code to grab object
                        //hit.collider.isTrigger = true;
                        //hit.collider.attachedRigidbody.useGravity = false;
                        GetComponent<Grabber>().grabbed = true;
                        GetComponent<Grabber>().grabbedObject = hit.collider.gameObject;
                        hit.collider.transform.SetParent(transform);
                        hit.collider.transform.position = transform.position;

                        //new additions
                        placeable.placed = false;
                        GetComponent<Grabber>().transition = false;

                        hit.collider.transform.rotation = transform.rotation;
                    }
                        
                    
                } else
                {
                    //sets color to red because invalid
                    //lineRenderer.material = LaserMat;
                }

            } else
            {
                //sets ending point to valid object and changes color to red
                //lineRenderer.SetPosition(1, transform.forward * rayDistance + transform.position);
                //lineRenderer.material = LaserMat;
            }

            //sets to not active
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
            {
                active = false;
            }
        } else
        {
            //lineRenderer.positionCount = 0;
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            //lineRenderer.positionCount = 0;
            active = false;
        }
    }
}
