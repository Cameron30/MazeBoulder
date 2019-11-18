using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerTouch : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            var hud = other.transform.Find("HUD");
            hud.gameObject.GetComponent<Health>().health = 0;
        }
    }
}

