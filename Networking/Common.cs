/**
 * Copyright (c) 2017-present, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the Scripts directory of this source tree. An additional grant 
 * of patent rights can be found in the PATENTS file in the same directory.
 */

using System;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Assertions;
using Oculus.Platform;
using Oculus.Platform.Models;

public class Common: MonoBehaviour
{
    public const int ConnectTimeout = 15;

    public const int ConnectionTimeout = 5;

    //public GameObject localAvatar;

    //protected AvatarState[] interpolatedAvatarState = new AvatarState[Constants.MaxClients];

    protected bool enableJitterBuffer = true;

    protected long frameNumber = 0;

    protected double renderTime = 0.0;
    protected double physicsTime = 0.0;

    
    protected bool notChanged;
    protected bool hasDelta;
    protected bool perfectPrediction;
    protected bool hasPredictionDelta;
    protected ushort baselineSequence;
    protected State cubeState;
    protected Delta cubeDelta;
    protected Delta predictionDelta;
    
    protected bool readNotChanged;
    protected bool readHasDelta;
    protected bool readPerfectPrediction;
    protected bool readHasPredictionDelta;
    protected ushort readBaselineSequence;
    protected State readCubeState;
    protected Delta readCubeDelta;
    protected Delta readPredictionDelta;

    protected uint[] packetBuffer = new uint[Constants.MaxPacketSize / 4];

    protected Network.ReadStream readStream = new Network.ReadStream();
    protected Network.WriteStream writeStream = new Network.WriteStream();

    protected PacketSerializer packetSerializer = new PacketSerializer();

    protected Network.Simulator networkSimulator = new Network.Simulator();

    protected bool WriteStateUpdatePacket(int objNum, bool shotLight, int potionNum, bool hasParent, ref State cubeState)
    {
        writeStream.Start( packetBuffer );

        bool result = true;

        try
        {
            packetSerializer.WriteHeader(writeStream, 00000000, objNum, shotLight, potionNum, hasParent, cubeState);
            
            writeStream.Finish();
        }
        catch ( Network.SerializeException )
        {
            Debug.Log( "error: failed to write state update packet packet" );
            result = false;
        }
        return result;
    }

    protected bool ReadStateUpdatePacket( byte[] packetData, ref byte isAvatar, ref int objNum, ref bool shotLight, ref int potionNum, ref bool hasParent, ref State cubeState)
    {
        readStream.Start( packetData );

        bool result = true;

        try
        {
            packetSerializer.ReadHeader(readStream, ref isAvatar, ref objNum, ref shotLight, ref potionNum, ref hasParent, ref cubeState);
            
        }
        catch ( Network.SerializeException )
        {
            Debug.Log( "error: failed to read state update packet" );
            
            result = false;
        }

        readStream.Finish();
        return result;
    }
    protected void WritePacketSizeToFile( System.IO.StreamWriter file, int packetBytes )
    {
        if ( file == null )
            return;

        file.WriteLine( packetBytes );

        file.Flush();
    }
}
