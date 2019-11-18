using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;

public class NetworkManager : MonoBehaviour
{

    GameObject updated;

    byte isAvatar;
    int objNum = 5;
    bool shotLight;
    int potionNum = -1;
    bool hasParent = false;
    State cubeState;

    int counter;

    HookDetector hookDetector;

    PacketSerializer serializer = new PacketSerializer();

    private byte[] readBuffer = new byte[Constants.MaxPacketSize];
    protected Network.ReadStream readStream = new Network.ReadStream();

    protected Network.Simulator networkSimulator = new Network.Simulator();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Packet packet;
        while ((packet = Net.ReadPacket()) != null)
        {
            packet.ReadBytes(readBuffer);
            
            readStream.Start(readBuffer);
            
            serializer.ReadHeader(readStream, ref isAvatar, ref objNum, ref shotLight, ref potionNum, ref hasParent, ref cubeState);
            
            updated = null;
            
            if (isAvatar != 00000001)
            {
                switch (objNum)
                {
                    case 0:
                        updated = GameObject.Find("Grappling Gun(Clone)");

                        if (updated == null)
                        {
                            updated = GameObject.Find("Grappling Gun");
                        }
                        break;
                    case 1:
                        updated = GameObject.Find("Drone");

                        if (updated == null)
                        {
                            updated = GameObject.Find("Drone(Clone)");
                        }
                        break;
                    case 2:
                        updated = GameObject.Find("Map");

                        if (updated == null)
                        {
                            updated = GameObject.Find("Map(Clone)");
                        }
                        break;
                    case 3:
                        var potions = GameObject.FindGameObjectsWithTag("Potion");
                        var poops = GameObject.FindGameObjectsWithTag("PoopPot");

                        foreach (GameObject potion in potions)
                        {
                            if (potion.GetComponent<PourLiquid>().potionNum == potionNum)
                            {
                                //Debug.Log("Found health");
                                updated = potion;
                            }
                        }

                        foreach (GameObject poop in poops)
                        {
                            if (poop.GetComponent<pourpooppotion>().potionNum == potionNum)
                            {
                                //Debug.Log("Found poop");
                                updated = poop;
                            }
                        }

                        if (updated == null)
                        {
                            //I am using shotLight for poop vs health potions along with fired and light.
                            //true => poop
                            Debug.Log(potionNum);
                            Debug.Log("Potion should spawn in!");
                            if (shotLight == true)
                            {
                                var potion = (GameObject)Instantiate(Resources.Load("PoopPotion"));
                                Debug.Log("Poop potion spawned in");
                                potion.GetComponent<pourpooppotion>().potionNum = potionNum;
                                Debug.Log(potion.GetComponent<pourpooppotion>().potionNum + " is the newly spawned num");
                                potion.transform.position = new Vector3(cubeState.position_x, cubeState.position_y / Constants.UnitsPerMeter, cubeState.position_z / Constants.UnitsPerMeter);
                                updated = potion;

                                var blues = GameObject.FindGameObjectsWithTag("Blue");

                                float current = 1000;
                                GameObject saved = new GameObject();
                                foreach (GameObject flower in blues)
                                {
                                    if (Vector3.Distance(new Vector3(cubeState.position_x / Constants.UnitsPerMeter, cubeState.position_y / Constants.UnitsPerMeter, cubeState.position_z / Constants.UnitsPerMeter), flower.transform.position) < current)
                                    {
                                        
                                        current = Vector3.Distance(new Vector3(cubeState.position_x / Constants.UnitsPerMeter, cubeState.position_y / Constants.UnitsPerMeter, cubeState.position_z / Constants.UnitsPerMeter), flower.transform.position);

                                        saved = flower;
                                    }
                                }

                                GameObject.Destroy(saved);
                            } else
                            {
                                var potion = (GameObject)Instantiate(Resources.Load("HealthPotion"));
                                potion.GetComponent<PourLiquid>().potionNum = potionNum;
                                potion.transform.position = new Vector3(cubeState.position_x / Constants.UnitsPerMeter, cubeState.position_y / Constants.UnitsPerMeter, cubeState.position_z / Constants.UnitsPerMeter);
                                updated = potion;
                                
                                var reds = GameObject.FindGameObjectsWithTag("Red");

                                float current = 1000;
                                GameObject saved = new GameObject();
                                foreach (GameObject flower in reds)
                                {
                                    if (Vector3.Distance(new Vector3(cubeState.position_x / Constants.UnitsPerMeter, cubeState.position_y / Constants.UnitsPerMeter, cubeState.position_z / Constants.UnitsPerMeter), flower.transform.position / Constants.UnitsPerMeter) < current)
                                    {
                                        current = Vector3.Distance(new Vector3(cubeState.position_x / Constants.UnitsPerMeter, cubeState.position_y / Constants.UnitsPerMeter, cubeState.position_z / Constants.UnitsPerMeter), flower.transform.position);
                                        Debug.Log("New Distance:" + current);
                                        saved = flower;
                                    }
                                }

                                GameObject.Destroy(saved);
                            }
                        } else
                        {
                        }
                        break;
                    default:
                        break;
                }

                if (objNum == 0)
                {
                    var shot = updated.GetComponent<GrapplingHook>();
                    if (shotLight && updated.transform.parent == null)
                    {
                        shot.fired = true;
                    } else
                    {
                        
                    }
                } else if (objNum == 1)
                {
                   var light = updated.GetComponent<DroneFly>();
                    if (shotLight)
                    {
                        light.isRemote = true;
                    } else
                    {
                        light.isRemote = false;
                    }
                }
                var obj = updated.GetComponent<NetworkObject>();

                if (hasParent)
                {
                    counter = counter + 1;

                    if (objNum == 3)
                    {
                    }
                }

                //if item already has parent, disengage from hand
                if (hasParent && (updated.transform.parent != null && counter > 60))
                {
                    updated.transform.parent = null;
                    var hand = GameObject.Find("Player/OVRCameraRig/TrackingSpace/RightHandAnchor");
                    var grabber = hand.GetComponent<Grabber>();
                    grabber.grabbedObject = null;
                    grabber.grabbed = false;

                    counter = 0;
                    
                    Debug.Log("Detaching!");
                }

                obj.ProcessPacket(readBuffer);
            } else
            {
                var playerController = gameObject.GetComponent<PlayerController>();
                var p2pManager = playerController.p2pManager;
                p2pManager.GetRemotePackets(packet);
            }
            
        }
    }
}
