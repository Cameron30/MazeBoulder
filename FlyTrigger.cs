using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTrigger : MonoBehaviour
{

    private GameObject player;
    private GameObject healthObject;

    private bool flyTowards;
    private bool hurtPlayer;
    private bool moveToward;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var fly = transform.parent.Find("Bug").gameObject;
        var point = transform.parent.Find("Origin Point");

        if (flyTowards)
        {
            // move fly immediately towards player
            fly.transform.position = player.transform.position;
            flyTowards = false;
            moveToward = true;
        }

        // hurt player once
        if (hurtPlayer)
        {
            healthObject.GetComponent<Health>().health -= 80;
            hurtPlayer = false;
        }

        if (moveToward && !flyTowards && !hurtPlayer)
        {
            float speed = 1.5f;
            float step = speed * Time.deltaTime;

            // Move Towards Origin Point
            fly.transform.position = Vector3.MoveTowards(fly.transform.position, point.transform.position, step);
        }



        // make fly inactive once back to origin point
        if (fly.transform.position == point.transform.position)
        {
            fly.SetActive(false);

            // reset flags
            moveToward = false;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            // enable fly renderer
            var fly = transform.parent.Find("Bug").gameObject;

            // grab player object
            player = other.gameObject;

            healthObject = player.transform.Find("HUD").gameObject;

            fly.SetActive(true);

            flyTowards = true;
            hurtPlayer = true;
        }
    }
}
