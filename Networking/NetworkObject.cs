using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;

public class NetworkObject : Common
{
    private byte[] readBuffer = new byte[Constants.MaxPacketSize];

    Snapshot lastSnapshot = new Snapshot();

    Vector3 prevPosition;
    Vector3 origin;

    byte isAvatar;
    int objNum;
    bool shotLight;
    int potionNum;
    public bool hasParent;
    

    bool prevShot;

    public void Start()
    {
        isAvatar = 00000000;
        shotLight = false;
        potionNum = 0;

        switch (gameObject.name)
        {
            case "Grappling Gun":
            case "Grappling Gun(Clone)":
                objNum = 0;
                break;
            case "Drone":
            case "Drone(Clone)":
                objNum = 1;
                break;
            case "Map":
            case "Map(Clone)":
                objNum = 2;
                break;
            case "HealthPotion(Clone)":
            case "PoopPotion(Clone)":
                objNum = 3;
                break;

            default:
                break;

        }
            
        if (objNum == 3)
        {
            Debug.Log(objNum);
            var pourPotion = GetComponent<PourLiquid>();

        if (pourPotion == null)
        {
                var pourPoopPotion = GetComponent<pourpooppotion>();
                shotLight = true;
                potionNum = pourPoopPotion.potionNum;
        } else
            {
                potionNum = pourPotion.potionNum;
            }
        }
    
    }

    public void FixedUpdate()
    {
        //Declare variables
        origin = this.gameObject.transform.position;
        var rigidBody = GetComponent<Rigidbody>();

        if (objNum == 0)
        {
            var hook = GetComponent<GrapplingHook>();
            if (hook.firstFired)
            {
                shotLight = true;
            } else
            {
                shotLight = false;
            }
        } else if (objNum == 1)
        {
            var light = GetComponent<DroneFly>();

            if (light.takenOff)
            {
                shotLight = true;
            } else
            {
                shotLight = false;
            }
        }
        
        if (transform.parent != null)
        {
            hasParent = true;

        } else if (transform.parent == null)
        {
            hasParent = false;
        }

        //if not moved,
        if (/*origin != prevPosition && */(hasParent || (shotLight && objNum == 1)))
        {
            //take new 'snapshot' (updates cubeState)
            Snapshot.GetState(rigidBody, ref cubeState, ref origin);
            SendPacket();
        }
        else
        {

            //ProcessPacket();
        }

        prevPosition = origin;

        //Snapshot.UpdateCubeState(rigidBody, ref cubeState, ref origin);
        //AddStateToRingBuffer();
    }



    ////////////////////////////////////
    //                                //
    //       CONTEXT FUNCTIONS        //
    //                                //
    ////////////////////////////////////
    public void ApplyCubeStateUpdates(ref State cubeState, bool applySmoothing = false)
    {
        Vector3 origin = Vector3.zero;
        
        Snapshot.UpdateCubeState(GetComponent<Rigidbody>(), ref cubeState, ref origin);
    }

    public Snapshot GetLastSnapshot()
    {
        return lastSnapshot;
    }


    ////////////////////////////////////
    //                                //
    //  PACKET SENDING AND RECEIVING  //
    //                                //
    ////////////////////////////////////
    public void ProcessPacket(byte[] readBuffer)
    {
            ProcessStateUpdatePacket(readBuffer);
    }

    void SendPacket()
    {
        WriteStateUpdatePacket(objNum, shotLight, potionNum, hasParent, ref cubeState);

        byte[] packetData = writeStream.GetData();

        // add the sent cube states to the send delta buffer

        //AddPacketToDeltaBuffer(ref cubeState);

        // reset cube priority for the cubes that were included in the packet (so other cubes have a chance to be sent...)

        //TODO REPLACE WITH SENDING PACKET TO ENTIRE ROOM
        Net.SendPacketToCurrentRoom(packetData, SendPolicy.Reliable);
    }

    public void ProcessStateUpdatePacket(byte[] packetData)
    {

        if (ReadStateUpdatePacket(packetData, ref isAvatar, ref objNum, ref shotLight, ref potionNum, ref hasParent, ref readCubeState))
        {
            // decode the predicted cube states from baselines

            //DecodePrediction(ref readPerfectPrediction, ref readHasPredictionDelta, ref readBaselineSequence, ref readCubeState, ref readPredictionDelta);

            // decode the not changed and delta cube states from baselines

            //DecodeNotChangedAndDeltas(ref readNotChanged, ref readHasDelta, ref readBaselineSequence, ref readCubeState, ref readCubeDelta);

            // add the cube states to the receive delta buffer

            //AddPacketToDeltaBuffer(ref readCubeState);

            // apply the state updates to cubes
            ApplyCubeStateUpdates(ref readCubeState);
        }
    }

}
