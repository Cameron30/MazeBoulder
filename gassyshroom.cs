using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gassyshroom : MonoBehaviour {

    private GameObject player;
    private int p_health;
    private GameObject spores;
    private bool inShroom;
    private bool shroomDebuff;
    private int frameNum;

	// Use this for initialization
	void Start () {
        inShroom = false;
        shroomDebuff = false;
        frameNum = 0;
        //spores.GetComponent<MeshRenderer>().enabled = false;
        spores = transform.gameObject;
        spores.GetComponent<ParticleSystem>().enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () {
        frameNum += 1;

		if (inShroom)
        {
            // TODO: throw particle effects in the mushroom, get mesh renderer to work
            player.GetComponent<Health>().health -= 1;
            //p_health -= 1; // damage over time while in shroom
            shroomDebuff = true; // debuff on!
            //spores.GetComponent<MeshRenderer>().enabled = true;
            spores.GetComponent<ParticleSystem>().enableEmission = true;
        }
        
        if (shroomDebuff && !inShroom)
        {
            // TODO: throw particle effects around the player
            if (frameNum == 20)
            {
                player.GetComponent<Health>().health -= 1;
                //p_health -= 1;
                //frameNum = 0;
            }
            if (frameNum == 8 * 60)
            {
                frameNum = 0;
                shroomDebuff = false;
            }

        }
	}
    void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            inShroom = true;
            player = other.gameObject;
            //Destroy(player);
            player = player.transform.Find("HUD").gameObject;

            //Destroy(player);

            //spores = transform.gameObject;
            //Destroy(spores);
        }
    }
    // trigger
    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            inShroom = false;
            //spores.GetComponent<MeshRenderer>().enabled = false;
            spores.GetComponent<ParticleSystem>().enableEmission = false;
        }      
    }
}
