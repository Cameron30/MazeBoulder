using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pourpooppotion : MonoBehaviour
{

    //Cameron's CUSTOM POURING BOOLEAN
    bool regen;
    public int potionNum = -1;

    // Use this for initialization
    //how quickly it takes to empty
    public float rateOfFlow = 1.0f;
    //such b....stuff
    public BottleSmash smashScript;
    public LiquidVolumeAnimator liquid;
    public Transform controllingTransform;
    public ParticleSystem pouringParticleSystem;
    //how many particles it takes to empty
    public float volumeOfParticles = 70.0f;
    private Rigidbody corkRB;
    GameObject player;
    Transform moveScript;

    GameObject pooptrap;
    Transform poop;
    StuckTrigger poopHP;
    void Start()
    {
        player = GameObject.Find("Player").gameObject;
        moveScript = player.transform.Find("OVRCameraRig");
        pooptrap = GameObject.Find("Poop Trap");

        poop = pooptrap.transform.Find("Poop");

        poopHP = poop.gameObject.GetComponent<StuckTrigger>();

        if (smashScript != null)
        {
            if (smashScript.Cork != null)
            {
                corkRB = smashScript.Cork.GetComponent<Rigidbody>();
            }
        }

        if (potionNum == -1)
        {
            var healthPotions = GameObject.FindGameObjectsWithTag("Potion");
            var poopPotions = GameObject.FindGameObjectsWithTag("PoopPot");
            var max = 0;

            foreach (GameObject health in healthPotions)
            {
                if (health.GetComponent<PourLiquid>().potionNum > max)
                {
                    max = health.GetComponent<PourLiquid>().potionNum;
                }
            }

            foreach (GameObject poop in poopPotions)
            {
                if (poop.GetComponent<pourpooppotion>().potionNum > max)
                {
                    max = poop.GetComponent<pourpooppotion>().potionNum;
                }
            }

            Debug.Log("Max potionNum: " + max);

            potionNum = max + 1;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {

        if (poop != null && poopHP != null && pooptrap != null)
        {
            if (poopHP.potionTracker >= 1 && liquid.level < 0.07)
            {
                Debug.Log("Potion Count = " + poopHP.potionTracker);
                moveScript.GetComponent<PlayerMovement>().enabled = true;

                Debug.Log("Destroying mosq!");
                Destroy(poopHP.mosq);
                Destroy(poop.gameObject);
                Destroy(gameObject);
            }
        }
        

        if (corkRB != null)
        {
            if (corkRB.isKinematic)
                return;
        }

        float d = Vector3.Dot(controllingTransform.up, Vector3.up);
        float d2 = (d + 1.0f) / 2;
        float particleVal = 0.0f;
        
        if (d2 < liquid.level)
        {
            particleVal = (liquid.level - d2) * rateOfFlow;
            liquid.level = Mathf.Lerp(liquid.level, d2, Time.deltaTime * rateOfFlow);

            //THIS IS CAMERON"S OWN CODE TO ADD TO HEALTH. MOST APPLICABLE SPOT I COULD FIND
            //modified by Jordan to meet poo needs
            if (liquid.level < .05)
            {

                if (poop != null && poopHP != null && pooptrap != null)
                {
                    if (regen)
                    {
                        poopHP.potionTracker++;
                    }
                }

                var player = GameObject.Find("Player");
                var rig = player.transform.Find("OVRCameraRig");
                var space = rig.Find("TrackingSpace");
                var hand = space.Find("RightHandAnchor");
                var grabber = hand.gameObject.GetComponent<Grabber>();

                grabber.grabbed = false;

                Destroy(gameObject);
            }
        }
        if (d <= 0.0f)
        {
            if (liquid.level > float.Epsilon)
                particleVal = liquid.level;
            liquid.level = Mathf.Lerp(liquid.level, 0, Time.deltaTime * rateOfFlow);

            //THIS IS CAMERON"S OWN CODE TO ADD TO HEALTH. MOST APPLICABLE SPOT I COULD FIND
            //that's like two spots bro
            //modified by jordan for poo
            if (liquid.level < .05)
            {

                if (poop != null && poopHP != null && pooptrap != null)
                {
                    if (regen)
                    {
                        poopHP.potionTracker++;
                    }
                }

                var player = GameObject.Find("Player");
                var rig = player.transform.Find("OVRCameraRig");
                var space = rig.Find("TrackingSpace");
                var hand = space.Find("RightHandAnchor");
                var grabber = hand.gameObject.GetComponent<Grabber>();

                grabber.grabbed = false;
                Destroy(gameObject);
            }
        }
        if (pouringParticleSystem != null)
        {
            //Debug.Log("[poop potion] pouring potion");
            ParticleSystem.MinMaxCurve emi = pouringParticleSystem.emission.rateOverTime;
            emi.constant = volumeOfParticles * particleVal;
            ParticleSystem.EmissionModule emod = pouringParticleSystem.emission;
            emod.rateOverTime = emi;
            //pouringParticleSystem.emission = emod;
            //pouringParticleSystem.Play();
            if (particleVal > 0)
            {
                //Debug.Log("partical value = " + particleVal);
                //Debug.Log("particleVal > 0");
                if (!pouringParticleSystem.isEmitting)
                {
                    pouringParticleSystem.Play();
                    //Debug.Log(".Play()");
                }


            }
            else
            {
                //Debug.Log("stops emitting");
                pouringParticleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.gameObject.name == "Poop")
            {
                regen = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            if (other.gameObject.name == "Poop")
            {
                regen = false;
            }
        }
    }

}
