using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCycle : MonoBehaviour {

    private Vector3 _startPosition;
    private Vector3 _currentPosition;

    [SerializeField] public float randTime;
    [SerializeField] public int randMag;

    float vectY = 0.0f;
    bool switchOn = false;
    bool secondSwitch = false;


    void Start()
    {

        _startPosition = transform.position;
        _currentPosition = transform.position;
        randTime = Random.Range(3.0f, 7.0f); // 3, 10

        // make each spear mirror first spear in each group
        //var first_spear = transform.parent.Find("First Spear");

        // assign random value if script is on "First Spear"
       // if (transform.name == "First Spear")
        //    randTime = Random.Range(3.0f, 10.0f);
        //else // gives other spears same randTime as "First Spear"
         //   randTime = first_spear.GetComponent<SpikeCycle>().randTime;
    }

    void Update()
    {

        if (switchOn && !secondSwitch)
        {
            launchSpike();
        }
        else if (!secondSwitch && !switchOn)
        {
            Invoke("turnOn", randTime);

        }
        else if (secondSwitch)
        {
            Invoke("resetSpike", 0.25f); // 1
        }

    }

    void launchSpike()
    {

        if (vectY >= 3.0f)
        {
            switchOn = false;
            secondSwitch = true;
        }
        else
        {
            vectY += 0.2f; // 0.2
            transform.position = _startPosition + new Vector3(0.0f, vectY, 0.0f);

            // make spear visible
            transform.GetComponent<MeshRenderer>().enabled = true;
        }

    }

    void resetSpike()
    {
        if (vectY < -5.5f)
        {
            transform.position = _startPosition;
            switchOn = false;
            secondSwitch = false;

            // make spear invisible
            transform.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            vectY -= 0.2f;
            transform.position = _startPosition + new Vector3(0.0f, vectY, 0.0f);
        }

    }

    void turnOn()
    {
        switchOn = true;
    }
}
