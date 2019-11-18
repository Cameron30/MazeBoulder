using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    //player health
    public int health = 410;
    public Sprite medium;
    public Sprite low;

    private GameObject leftCanvas;
    private GameObject rightCanvas;

    private Image leftImage;
    private Image rightImage;

    private bool floating;

	// Use this for initialization
	void Start () {
        leftCanvas = GameObject.Find("/Player/HUD/CanvasL").gameObject;
        rightCanvas = GameObject.Find("/Player/HUD/CanvasR").gameObject;

        leftImage = GameObject.Find("/Player/HUD/CanvasL/HealthText").GetComponent<Image>();
        rightImage = GameObject.Find("/Player/HUD/CanvasR/HealthText").GetComponent<Image>();

    }
	
	// Update is called once per frame
	void Update () {

        if (floating)
        {
            var grav = GetComponent<Rigidbody>().useGravity;
            grav = GetComponent<Rigidbody>().useGravity;
        }

        if (health < 400 && health > 250)
        {
            leftImage.enabled = true;
            rightImage.enabled = true;
            leftImage.sprite = medium;
            rightImage.sprite = medium;
        } else if (health <= 250)
        {
            leftImage.sprite = low;
            rightImage.sprite = low;
            leftImage.enabled = true;
            rightImage.enabled = true;
        } else
        {
            leftImage.enabled = false;
            rightImage.enabled = false;
        }


        if (health <= 0 || transform.parent.position.y < -170)
        {
            var player = transform.parent.gameObject;

            var room = GameObject.Find("Buffer Room(Clone)");
            //if (room != null)
            //{
                //if (player.transform.position.x > room.transform.position.x)
                //{
                //    player.transform.position = new Vector3(room.transform.position.x, 0, room.transform.position.z);
                //} else
                //{
                    player.transform.position = Vector3.zero;

                    var remotePlayers = GameObject.FindGameObjectsWithTag("Remote");
                    if (remotePlayers.Length != 0)
                    {
                        foreach (GameObject remote in remotePlayers)
                        {
                            Debug.Log(remote.transform.position);
                            if ((remote.transform.position.z < -35f || remote.transform.position.z > -25) && (remote.transform.position.x < -130f || remote.transform.position.x > -85f))
                            {
                                //Debug.Log(remote.transform.position);
                                player.transform.position = new Vector3(remote.transform.position.x, remote.transform.position.y + 10f, remote.transform.position.z);
                            }
                        }

                        if (player.transform.position == Vector3.zero)
                        {
                            Debug.Log("Other players in danger zone!");
                            player.transform.position = new Vector3(-78, 10f, -31);
                        }
                    } else
                    {
                        Debug.Log("No remote players!");
                        player.transform.position = new Vector3(-15, 10f, -44);
                    }
               // }
            //}
            //else
            //{
                //player.transform.position = new Vector3(-15, 1.7f, -44);
            //}
            health = 410;
            // enable player movement
            var moveScript = player.transform.Find("OVRCameraRig");
            moveScript.GetComponent<PlayerMovement>().enabled = true;
            moveScript.GetComponent<PlayerMovement>().running = false;
        }

    }
}
