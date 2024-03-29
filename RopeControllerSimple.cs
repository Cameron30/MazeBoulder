﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//If we have a stiff rope, such as a metal wire, then we need a simplified solution
//this is also an accurate solution because a metal wire is not swinging as much as a rope made of a lighter material
public class RopeControllerSimple : MonoBehaviour
{
    //Objects that will interact with the rope
    public Transform whatTheRopeIsConnectedTo;
    public Transform whatIsHangingFromTheRope;
    public float loadMass;

    //Line renderer used to display the rope
    private LineRenderer lineRenderer;

    //A list with all rope sections
    public List<Vector3> allRopeSections = new List<Vector3>();

    //Rope data
    private float ropeLength = 7f; // 4 default
                                    // 7 value
    private float minRopeLength = 1f; // 1 default
    private float maxRopeLength = 15f; // 20 default
    //Mass of what the rope is carrying
    //private float loadMass = 100f;
    //How fast we can add more/less rope
    // 3.79, -5.55, 0
     float winchSpeed = 1.5f; // 2 default

    //The joint we use to approximate the rope
    SpringJoint springJoint;

    float frameNum;
    float beginFrame;

    bool syncFlag;
    BoulderTrigger trig;
    Vector3 startPos;
    void Start()
    {
        startPos = whatIsHangingFromTheRope.position;

        trig = GameObject.Find("Boulder Trap/BoulderTrigger").GetComponent<BoulderTrigger>();

        frameNum = 0;
        beginFrame = 0;
        springJoint = whatTheRopeIsConnectedTo.GetComponent<SpringJoint>();
        
        //Init the line renderer we use to display the rope
        lineRenderer = GetComponent<LineRenderer>();

        //lineRenderer.useWorldSpace = true;

        //Init the spring we use to approximate the rope from point a to b
        UpdateSpring();

        //Add the weight to what the rope is carrying
        whatIsHangingFromTheRope.GetComponent<Rigidbody>().mass = loadMass;
    }

    void Update()
    {
        //Add more/less rope
        UpdateWinch();

        //Display the rope with a line renderer
        DisplayRope();
        Vector3 a = new Vector3(4, 0, 0);
        Vector3 b = new Vector3(5, 0, 0);
        Vector3 c = new Vector3(-4, 0, 0);
        Vector3 d = new Vector3(-5, 0, 0);

        if (trig.sync == true && syncFlag == false)
        {
            whatIsHangingFromTheRope.position = startPos;
            syncFlag = true;
        }
            


        //var velocity = whatIsHangingFromTheRope.GetComponent<Rigidbody>().velocity;
        ++frameNum;
        ++beginFrame;
        if (frameNum == 180)
        {
            //Debug.Log("weight velocity = " + whatIsHangingFromTheRope.GetComponent<Rigidbody>().velocity);
            frameNum = 0;
        }
        //Debug.Log("weight (A) velocity = " + whatIsHangingFromTheRope.GetComponent<Rigidbody>().angularVelocity);
        /*if (velocity.x >= a.x && velocity.x <= b.x)
        {
            Debug.Log("[between 4 and 5]");
            //velocity = -1 * (velocity);
            velocity = Vector3.zero;
        }
        if (velocity.x <= c.x && velocity.x >= d.x)
        {
            Debug.Log("[between -4 and -5]");
            //velocity = -1 * (velocity);
            velocity = Vector3.zero;
        }*/
        //whatIsHangingFromTheRope.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //velocity = Vector3.zero;
        /*if (beginFrame == 45) {
            if (whatIsHangingFromTheRope.GetComponent<Rigidbody>().velocity.x >= -1 &&
                whatIsHangingFromTheRope.GetComponent<Rigidbody>().velocity.x <= 0)
            {
                Debug.Log("     [-1 and 0]");
                whatIsHangingFromTheRope.GetComponent<Rigidbody>().velocity = b + Vector3.up;
                Debug.Log("     velocity = " + whatIsHangingFromTheRope.GetComponent<Rigidbody>().velocity);

            }
            if (whatIsHangingFromTheRope.GetComponent<Rigidbody>().velocity.x <= 0 &&
                whatIsHangingFromTheRope.GetComponent<Rigidbody>().velocity.x >= 1)
            {
                Debug.Log("     [0 and 1]");
                whatIsHangingFromTheRope.GetComponent<Rigidbody>().velocity = d + (-1 * Vector3.up);
                Debug.Log("     velocity = " + whatIsHangingFromTheRope.GetComponent<Rigidbody>().velocity);
            }
        }*/
        
    }

    //Update the spring constant and the length of the spring
    private void UpdateSpring()
    {
        //Someone said you could set this to infinity to avoid bounce, but it doesnt work
        //kRope = float.inf

        //
        //The mass of the rope
        //
        //Density of the wire (stainless steel) kg/m3
        float density = 7750f; // default 7750f
        //The radius of the wire
        float radius = 0.02f; // 0.02

        float volume = Mathf.PI * radius * radius * ropeLength;

        float ropeMass = volume * density;

        //Add what the rope is carrying
        ropeMass += loadMass;


        //
        //The spring constant (has to recalculate if the rope length is changing)
        //
        //The force from the rope F = rope_mass * g, which is how much the top rope segment will carry
        float ropeForce = ropeMass * 9.81f;

        //Use the spring equation to calculate F = k * x should balance this force, 
        //where x is how much the top rope segment should stretch, such as 0.01m

        //Is about 146000
        float kRope = ropeForce / 0.01f;

        //print(ropeMass);

        //Add the value to the spring
        springJoint.spring = kRope * 1.0f;
        springJoint.damper = kRope * 0.8f;

        //Update length of the rope
        springJoint.maxDistance = 10;
    }

    //Display the rope with a line renderer
    private void DisplayRope()
    {
        
        //This is not the actual width, but the width use so we can see the rope
        float ropeWidth = 0.1f; // 0.2 original

        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;


        //Update the list with rope sections by approximating the rope with a bezier curve
        //A Bezier curve needs 4 control points
        Vector3 A = whatTheRopeIsConnectedTo.position;
        Vector3 D = whatIsHangingFromTheRope.position;

        //Upper control point
        //To get a little curve at the top than at the bottom
        Vector3 B = A + whatTheRopeIsConnectedTo.up * (-(A - D).magnitude * 0.1f);
        //B = A;

        //Lower control point
        Vector3 C = D + whatIsHangingFromTheRope.up * ((A - D).magnitude * 0.5f);
        //C = D;
        //Get the positions
        BezierCurve.GetBezierCurve(A, B, C, D, allRopeSections);


        //An array with all rope section positions
        Vector3[] positions = new Vector3[allRopeSections.Count];

        for (int i = 0; i < allRopeSections.Count; i++)
        {
            positions[i] = allRopeSections[i];
        }

        //Just add a line between the start and end position for testing purposes
        //Vector3[] positions = new Vector3[2];

        //positions[0] = whatTheRopeIsConnectedTo.position;
        //positions[1] = whatIsHangingFromTheRope.position;


        //Add the positions to the line renderer
        lineRenderer.positionCount = positions.Length;

        lineRenderer.SetPositions(positions);
    }

    //Add more/less rope
    private void UpdateWinch()
    {
        bool hasChangedRope = false;

        //More rope
        if (Input.GetKey(KeyCode.O) && ropeLength < maxRopeLength)
        {
            ropeLength += winchSpeed * Time.deltaTime;

            hasChangedRope = true;
        }
        else if (Input.GetKey(KeyCode.I) && ropeLength > minRopeLength)
        {
            ropeLength -= winchSpeed * Time.deltaTime;

            hasChangedRope = true;
        }


        if (hasChangedRope)
        {
            ropeLength = Mathf.Clamp(ropeLength, minRopeLength, maxRopeLength);

            //Need to recalculate the k-value because it depends on the length of the rope
            UpdateSpring();
        }
    }
}

//Approximate the rope with a bezier curve
public static class BezierCurve
{
    //Update the positions of the rope section
    public static void GetBezierCurve(Vector3 A, Vector3 B, Vector3 C, Vector3 D, List<Vector3> allRopeSections)
    {
        //The resolution of the line
        //Make sure the resolution is adding up to 1, so 0.3 will give a gap at the end, but 0.2 will work
        float resolution = 0.1f;

        //Clear the list
        allRopeSections.Clear();


        float t = 0;

        while (t <= 1f)
        {
            //Find the coordinates between the control points with a Bezier curve
            Vector3 newPos = DeCasteljausAlgorithm(A, B, C, D, t);

            allRopeSections.Add(newPos);

            //Which t position are we at?
            t += resolution;
        }

        allRopeSections.Add(D);
    }

    //The De Casteljau's Algorithm
    static Vector3 DeCasteljausAlgorithm(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t)
    {
        //Linear interpolation = lerp = (1 - t) * A + t * B
        //Could use Vector3.Lerp(A, B, t)

        //To make it faster
        float oneMinusT = 1f - t;

        //Layer 1
        Vector3 Q = oneMinusT * A + t * B;
        Vector3 R = oneMinusT * B + t * C;
        Vector3 S = oneMinusT * C + t * D;

        //Layer 2
        Vector3 P = oneMinusT * Q + t * R;
        Vector3 T = oneMinusT * R + t * S;

        //Final interpolated position
        Vector3 U = oneMinusT * P + t * T;

        return U;
    }
}
