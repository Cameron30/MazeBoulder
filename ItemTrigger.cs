using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{

    //public GameObject gun;
    public Transform[] items;

    private Transform[] itemPos;

    [HideInInspector]
    public bool stayFlag = false;

    bool exitFlag = false;


    // Use this for initialization
    void Start()
    {
        itemPos = new Transform[3];
    }

    // Update is called once per frame
    void Update()
    {
        if (itemPos != null)
        {
            // updates each item's position every frame
            foreach (Transform child in itemPos)
            {
                if (child != null)
                    child.position = child.parent.position;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            if (!stayFlag)
            {
                //Debug.Log("[ItemTrigger: OnTriggerStay] Player");
                Invoke("stayDelay", 5f);
                stayFlag = true;
            }

            /*
            foreach (Transform drone in itemPos)
            {
                try
                {
                    drone.GetComponent<DroneFly>().enabled = false;
                }
                catch (Exception)
                {
                    // bahabahb
                }
            }
            */
            try
            {
                // itemslot -> actual item (gun, potion, etc.)
                Destroy(other.transform.Find("ItemSlots").GetChild(0).GetChild(0).GetComponent<Placeable>());
                itemPos[0] = other.transform.Find("ItemSlots").GetChild(0).GetChild(0);
            }
            catch (Exception)
            {
                //if (stayFlag)
                //    print(e);
            }
            try
            {
                Destroy(other.transform.Find("ItemSlots").GetChild(1).GetChild(0).GetComponent<Placeable>());
                itemPos[1] = other.transform.Find("ItemSlots").GetChild(1).GetChild(0);
            }
            catch (Exception)
            {
                //if (stayFlag)
                //    print(e);
            }
            try
            {
                Destroy(other.transform.Find("ItemSlots").GetChild(2).GetChild(0).GetComponent<Placeable>());
                itemPos[2] = other.transform.Find("ItemSlots").GetChild(2).GetChild(0);
            }
            catch (Exception)
            {
                //if (stayFlag)
                //   print(e);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            if (!exitFlag)
            {
                //Debug.Log("[ItemTrigger: OnTriggerExit] Player");
                //Invoke("exitDelay", 5f);
                //exitFlag = true;
            }

            /*
            foreach (Transform drone in itemPos)
            {
                try
                {
                    drone.GetComponent<DroneFly>().enabled = true;
                }
                catch (Exception)
                {
                    // bahabahb
                }
            }
            */

            try
            {
                // itemslot -> actual item (gun, potion, etc.)
                other.transform.Find("ItemSlots").GetChild(0).GetChild(0).gameObject.AddComponent<Placeable>();

                // give items appropriate values when adding back Placeable script
                other.transform.Find("ItemSlots").GetChild(0).GetChild(0).GetComponent<Placeable>().placed = true;
            }
            catch (Exception e)
            {
                if (exitFlag)
                    print(e);
            }
            try
            {
                other.transform.Find("ItemSlots").GetChild(1).GetChild(0).gameObject.AddComponent<Placeable>();

                other.transform.Find("ItemSlots").GetChild(1).GetChild(0).GetComponent<Placeable>().placed = true;
            }
            catch (Exception e)
            {
                if (exitFlag)
                    print(e);
            }
            try
            {
                other.transform.Find("ItemSlots").GetChild(2).GetChild(0).gameObject.AddComponent<Placeable>();

                other.transform.Find("ItemSlots").GetChild(2).GetChild(0).GetComponent<Placeable>().placed = true;
            }
            catch (Exception e)
            {
                if (exitFlag)
                    print(e);
            }
        }
    }
    void stayDelay()
    {
        stayFlag = false;
    }
    void exitDelay()
    {
        exitFlag = false;
    }

}
