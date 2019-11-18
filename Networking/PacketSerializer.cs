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
using UnityEngine.Assertions;
using System.Collections.Generic;

public class PacketSerializer : Network.Serializer
{
    public void WriteStateUpdatePacket(Network.WriteStream stream, State cubeState)
    {

#if DEBUG_DELTA_COMPRESSION
            write_int( stream, cubeDelta.absolute_position_x, Constants.PositionMinimumXZ, Constants.PositionMaximumXZ );
            write_int( stream, cubeDelta.absolute_position_y, Constants.PositionMinimumY, Constants.PositionMaximumY );
            write_int( stream, cubeDelta.absolute_position_z, Constants.PositionMinimumXZ, Constants.PositionMaximumXZ );
#endif // #if DEBUG_DELTA_COMPRESSION

        write_int(stream, cubeState.position_x, Constants.PositionMinimumXZ, Constants.PositionMaximumXZ);
        write_int(stream, cubeState.position_y, Constants.PositionMinimumY, Constants.PositionMaximumY);
        write_int(stream, cubeState.position_z, Constants.PositionMinimumXZ, Constants.PositionMaximumXZ);

        
        write_bits(stream, cubeState.rotation_largest, 2);
        write_bits(stream, cubeState.rotation_a, Constants.RotationBits);
        write_bits(stream, cubeState.rotation_b, Constants.RotationBits);
        write_bits(stream, cubeState.rotation_c, Constants.RotationBits);
        /*
        if (cubeState.active)
        {
            write_int(stream, cubeState.linear_velocity_x, Constants.LinearVelocityMinimum, Constants.LinearVelocityMaximum);
            write_int(stream, cubeState.linear_velocity_y, Constants.LinearVelocityMinimum, Constants.LinearVelocityMaximum);
            write_int(stream, cubeState.linear_velocity_z, Constants.LinearVelocityMinimum, Constants.LinearVelocityMaximum);

            write_int(stream, cubeState.angular_velocity_x, Constants.AngularVelocityMinimum, Constants.AngularVelocityMaximum);
            write_int(stream, cubeState.angular_velocity_y, Constants.AngularVelocityMinimum, Constants.AngularVelocityMaximum);
            write_int(stream, cubeState.angular_velocity_z, Constants.AngularVelocityMinimum, Constants.AngularVelocityMaximum);
        }
        */
    }

    public void ReadStateUpdatePacket(Network.ReadStream stream, ref State cubeState)
    {
        //hasDelta = false;
        //perfectPrediction = false;
        //hasPredictionDelta = false;

#if DEBUG_DELTA_COMPRESSION
            read_int( stream, out cubeDelta.absolute_position_x, Constants.PositionMinimumXZ, Constants.PositionMaximumXZ );
            read_int( stream, out cubeDelta.absolute_position_y, Constants.PositionMinimumY, Constants.PositionMaximumY );
            read_int( stream, out cubeDelta.absolute_position_z, Constants.PositionMinimumXZ, Constants.PositionMaximumXZ );
#endif // #if DEBUG_DELTA_COMPRESSION
        /*
        read_bool(stream, out isAvatar);
        read_int(stream, out objNum, 0, 3);
        read_bool(stream, out shotLight);
        read_int(stream, out potionNum, 0, 15);
        */

        
        read_int(stream, out cubeState.position_x, Constants.PositionMinimumXZ, Constants.PositionMaximumXZ);
        read_int(stream, out cubeState.position_y, Constants.PositionMinimumY, Constants.PositionMaximumY);
        read_int(stream, out cubeState.position_z, Constants.PositionMinimumXZ, Constants.PositionMaximumXZ);
        
        read_bits(stream, out cubeState.rotation_largest, 2);
        read_bits(stream, out cubeState.rotation_a, Constants.RotationBits);
        read_bits(stream, out cubeState.rotation_b, Constants.RotationBits);
        read_bits(stream, out cubeState.rotation_c, Constants.RotationBits);
        
        /*
        if (cubeState.active)
        {
            read_int(stream, out cubeState.linear_velocity_x, Constants.LinearVelocityMinimum, Constants.LinearVelocityMaximum);
            read_int(stream, out cubeState.linear_velocity_y, Constants.LinearVelocityMinimum, Constants.LinearVelocityMaximum);
            read_int(stream, out cubeState.linear_velocity_z, Constants.LinearVelocityMinimum, Constants.LinearVelocityMaximum);

            read_int(stream, out cubeState.angular_velocity_x, Constants.AngularVelocityMinimum, Constants.AngularVelocityMaximum);
            read_int(stream, out cubeState.angular_velocity_y, Constants.AngularVelocityMinimum, Constants.AngularVelocityMaximum);
            read_int(stream, out cubeState.angular_velocity_z, Constants.AngularVelocityMinimum, Constants.AngularVelocityMaximum);
        }
        else
        {
            cubeState.linear_velocity_x = 0;
            cubeState.linear_velocity_y = 0;
            cubeState.linear_velocity_z = 0;

            cubeState.angular_velocity_x = 0;
            cubeState.angular_velocity_y = 0;
            cubeState.angular_velocity_z = 0;
        }
        */
    }

    public void ReadHeader(Network.ReadStream stream, ref byte isAvatar, ref int objNum, ref bool shotLight, ref int potionNum, ref bool hasParent, ref State cubeState)
    {
        read_bits(stream, out isAvatar, 8);
        //read_bool(stream, out isAvatar);
        if (isAvatar != 00000001)
        {
            read_int(stream, out objNum, 0, 3);
            read_bool(stream, out shotLight);
            read_int(stream, out potionNum, 0, 127);
            read_bool(stream, out hasParent);

            ReadStateUpdatePacket(stream, ref cubeState);
        }
    }

    public void WriteHeader(Network.WriteStream stream, byte isAvatar, int obj, bool shotLight, int potionNum, bool hasParent, State cubeState)
    {
            write_bits(stream, 00000000, 8);
            //write_bool(stream, false);
            write_int(stream, obj, 0, 3);
            write_bool(stream, shotLight);
            write_int(stream, potionNum, 0, 127);
            write_bool(stream, hasParent);

            WriteStateUpdatePacket(stream, cubeState);
    }
    /*
    void write_position_delta(Network.WriteStream stream, int delta_x, int delta_y, int delta_z)
    {

        uint unsigned_x = Network.Util.SignedToUnsigned(delta_x);
        uint unsigned_y = Network.Util.SignedToUnsigned(delta_y);
        uint unsigned_z = Network.Util.SignedToUnsigned(delta_z);

        bool small_x = unsigned_x <= Constants.PositionDeltaSmallThreshold;
        bool small_y = unsigned_y <= Constants.PositionDeltaSmallThreshold;
        bool small_z = unsigned_z <= Constants.PositionDeltaSmallThreshold;

        bool all_small = small_x && small_y && small_z;

        write_bool(stream, all_small);

        if (all_small)
        {
            write_bits(stream, unsigned_x, Constants.PositionDeltaSmallBits);
            write_bits(stream, unsigned_y, Constants.PositionDeltaSmallBits);
            write_bits(stream, unsigned_z, Constants.PositionDeltaSmallBits);
        }
        else
        {
            write_bool(stream, small_x);

            if (small_x)
            {
                write_bits(stream, unsigned_x, Constants.PositionDeltaSmallBits);
            }
            else
            {
                unsigned_x -= Constants.PositionDeltaSmallThreshold;

                bool medium_x = unsigned_x < Constants.PositionDeltaMediumThreshold;

                write_bool(stream, medium_x);

                if (medium_x)
                {
                    write_bits(stream, unsigned_x, Constants.PositionDeltaMediumBits);
                }
                else
                {
                    write_int(stream, delta_x, -Constants.PositionDeltaMax, +Constants.PositionDeltaMax);
                }
            }

            write_bool(stream, small_y);

            if (small_y)
            {
                write_bits(stream, unsigned_y, Constants.PositionDeltaSmallBits);
            }
            else
            {
                unsigned_y -= Constants.PositionDeltaSmallThreshold;

                bool medium_y = unsigned_y < Constants.PositionDeltaMediumThreshold;

                write_bool(stream, medium_y);

                if (medium_y)
                {
                    write_bits(stream, unsigned_y, Constants.PositionDeltaMediumBits);
                }
                else
                {
                    write_int(stream, delta_y, -Constants.PositionDeltaMax, +Constants.PositionDeltaMax);
                }
            }

            write_bool(stream, small_z);

            if (small_z)
            {
                write_bits(stream, unsigned_z, Constants.PositionDeltaSmallBits);
            }
            else
            {
                unsigned_z -= Constants.PositionDeltaSmallThreshold;

                bool medium_z = unsigned_z < Constants.PositionDeltaMediumThreshold;

                write_bool(stream, medium_z);

                if (medium_z)
                {
                    write_bits(stream, unsigned_z, Constants.PositionDeltaMediumBits);
                }
                else
                {
                    write_int(stream, delta_z, -Constants.PositionDeltaMax, +Constants.PositionDeltaMax);
                }
            }
        }
    }

    void read_position_delta(Network.ReadStream stream, out int delta_x, out int delta_y, out int delta_z)
    {
        bool all_small;

        read_bool(stream, out all_small);

        uint unsigned_x;
        uint unsigned_y;
        uint unsigned_z;

        if (all_small)
        {
            read_bits(stream, out unsigned_x, Constants.PositionDeltaSmallBits);
            read_bits(stream, out unsigned_y, Constants.PositionDeltaSmallBits);
            read_bits(stream, out unsigned_z, Constants.PositionDeltaSmallBits);

            delta_x = Network.Util.UnsignedToSigned(unsigned_x);
            delta_y = Network.Util.UnsignedToSigned(unsigned_y);
            delta_z = Network.Util.UnsignedToSigned(unsigned_z);
        }
        else
        {
            bool small_x;

            read_bool(stream, out small_x);

            if (small_x)
            {
                read_bits(stream, out unsigned_x, Constants.PositionDeltaSmallBits);

                delta_x = Network.Util.UnsignedToSigned(unsigned_x);
            }
            else
            {
                bool medium_x;

                read_bool(stream, out medium_x);

                if (medium_x)
                {
                    read_bits(stream, out unsigned_x, Constants.PositionDeltaMediumBits);

                    delta_x = Network.Util.UnsignedToSigned(unsigned_x + Constants.PositionDeltaSmallThreshold);
                }
                else
                {
                    read_int(stream, out delta_x, -Constants.PositionDeltaMax, +Constants.PositionDeltaMax);
                }
            }

            bool small_y;

            read_bool(stream, out small_y);

            if (small_y)
            {
                read_bits(stream, out unsigned_y, Constants.PositionDeltaSmallBits);

                delta_y = Network.Util.UnsignedToSigned(unsigned_y);
            }
            else
            {
                bool medium_y;

                read_bool(stream, out medium_y);

                if (medium_y)
                {
                    read_bits(stream, out unsigned_y, Constants.PositionDeltaMediumBits);

                    delta_y = Network.Util.UnsignedToSigned(unsigned_y + Constants.PositionDeltaSmallThreshold);
                }
                else
                {
                    read_int(stream, out delta_y, -Constants.PositionDeltaMax, +Constants.PositionDeltaMax);
                }
            }

            bool small_z;

            read_bool(stream, out small_z);

            if (small_z)
            {
                read_bits(stream, out unsigned_z, Constants.PositionDeltaSmallBits);

                delta_z = Network.Util.UnsignedToSigned(unsigned_z);
            }
            else
            {
                bool medium_z;

                read_bool(stream, out medium_z);

                if (medium_z)
                {
                    read_bits(stream, out unsigned_z, Constants.PositionDeltaMediumBits);

                    delta_z = Network.Util.UnsignedToSigned(unsigned_z + Constants.PositionDeltaSmallThreshold);
                }
                else
                {
                    read_int(stream, out delta_z, -Constants.PositionDeltaMax, +Constants.PositionDeltaMax);
                }
            }
        }
    }

    void write_linear_velocity_delta(Network.WriteStream stream, int delta_x, int delta_y, int delta_z)
    {

        uint unsigned_x = Network.Util.SignedToUnsigned(delta_x);
        uint unsigned_y = Network.Util.SignedToUnsigned(delta_y);
        uint unsigned_z = Network.Util.SignedToUnsigned(delta_z);

        bool small_x = unsigned_x <= Constants.LinearVelocityDeltaSmallThreshold;
        bool small_y = unsigned_y <= Constants.LinearVelocityDeltaSmallThreshold;
        bool small_z = unsigned_z <= Constants.LinearVelocityDeltaSmallThreshold;

        bool all_small = small_x && small_y && small_z;

        write_bool(stream, all_small);

        if (all_small)
        {
            write_bits(stream, unsigned_x, Constants.LinearVelocityDeltaSmallBits);
            write_bits(stream, unsigned_y, Constants.LinearVelocityDeltaSmallBits);
            write_bits(stream, unsigned_z, Constants.LinearVelocityDeltaSmallBits);
        }
        else
        {
            write_bool(stream, small_x);

            if (small_x)
            {
                write_bits(stream, unsigned_x, Constants.LinearVelocityDeltaSmallBits);
            }
            else
            {
                unsigned_x -= Constants.LinearVelocityDeltaSmallThreshold;

                bool medium_x = unsigned_x < Constants.LinearVelocityDeltaMediumThreshold;

                write_bool(stream, medium_x);

                if (medium_x)
                {
                    write_bits(stream, unsigned_x, Constants.LinearVelocityDeltaMediumBits);
                }
                else
                {
                    write_int(stream, delta_x, -Constants.LinearVelocityDeltaMax, +Constants.LinearVelocityDeltaMax);
                }
            }

            write_bool(stream, small_y);

            if (small_y)
            {
                write_bits(stream, unsigned_y, Constants.LinearVelocityDeltaSmallBits);
            }
            else
            {
                unsigned_y -= Constants.LinearVelocityDeltaSmallThreshold;

                bool medium_y = unsigned_y < Constants.LinearVelocityDeltaMediumThreshold;

                write_bool(stream, medium_y);

                if (medium_y)
                {
                    write_bits(stream, unsigned_y, Constants.LinearVelocityDeltaMediumBits);
                }
                else
                {
                    write_int(stream, delta_y, -Constants.LinearVelocityDeltaMax, +Constants.LinearVelocityDeltaMax);
                }
            }

            write_bool(stream, small_z);

            if (small_z)
            {
                write_bits(stream, unsigned_z, Constants.LinearVelocityDeltaSmallBits);
            }
            else
            {
                unsigned_z -= Constants.LinearVelocityDeltaSmallThreshold;

                bool medium_z = unsigned_z < Constants.LinearVelocityDeltaMediumThreshold;

                write_bool(stream, medium_z);

                if (medium_z)
                {
                    write_bits(stream, unsigned_z, Constants.LinearVelocityDeltaMediumBits);
                }
                else
                {
                    write_int(stream, delta_z, -Constants.LinearVelocityDeltaMax, +Constants.LinearVelocityDeltaMax);
                }
            }
        }
    }

    void read_linear_velocity_delta(Network.ReadStream stream, out int delta_x, out int delta_y, out int delta_z)
    {
        bool all_small;

        read_bool(stream, out all_small);

        uint unsigned_x;
        uint unsigned_y;
        uint unsigned_z;

        if (all_small)
        {
            read_bits(stream, out unsigned_x, Constants.LinearVelocityDeltaSmallBits);
            read_bits(stream, out unsigned_y, Constants.LinearVelocityDeltaSmallBits);
            read_bits(stream, out unsigned_z, Constants.LinearVelocityDeltaSmallBits);

            delta_x = Network.Util.UnsignedToSigned(unsigned_x);
            delta_y = Network.Util.UnsignedToSigned(unsigned_y);
            delta_z = Network.Util.UnsignedToSigned(unsigned_z);
        }
        else
        {
            bool small_x;

            read_bool(stream, out small_x);

            if (small_x)
            {
                read_bits(stream, out unsigned_x, Constants.LinearVelocityDeltaSmallBits);

                delta_x = Network.Util.UnsignedToSigned(unsigned_x);
            }
            else
            {
                bool medium_x;

                read_bool(stream, out medium_x);

                if (medium_x)
                {
                    read_bits(stream, out unsigned_x, Constants.LinearVelocityDeltaMediumBits);

                    delta_x = Network.Util.UnsignedToSigned(unsigned_x + Constants.LinearVelocityDeltaSmallThreshold);
                }
                else
                {
                    read_int(stream, out delta_x, -Constants.LinearVelocityDeltaMax, +Constants.LinearVelocityDeltaMax);
                }
            }

            bool small_y;

            read_bool(stream, out small_y);

            if (small_y)
            {
                read_bits(stream, out unsigned_y, Constants.LinearVelocityDeltaSmallBits);

                delta_y = Network.Util.UnsignedToSigned(unsigned_y);
            }
            else
            {
                bool medium_y;

                read_bool(stream, out medium_y);

                if (medium_y)
                {
                    read_bits(stream, out unsigned_y, Constants.LinearVelocityDeltaMediumBits);

                    delta_y = Network.Util.UnsignedToSigned(unsigned_y + Constants.LinearVelocityDeltaSmallThreshold);
                }
                else
                {
                    read_int(stream, out delta_y, -Constants.LinearVelocityDeltaMax, +Constants.LinearVelocityDeltaMax);
                }
            }

            bool small_z;

            read_bool(stream, out small_z);

            if (small_z)
            {
                read_bits(stream, out unsigned_z, Constants.LinearVelocityDeltaSmallBits);

                delta_z = Network.Util.UnsignedToSigned(unsigned_z);
            }
            else
            {
                bool medium_z;

                read_bool(stream, out medium_z);

                if (medium_z)
                {
                    read_bits(stream, out unsigned_z, Constants.LinearVelocityDeltaMediumBits);

                    delta_z = Network.Util.UnsignedToSigned(unsigned_z + Constants.LinearVelocityDeltaSmallThreshold);
                }
                else
                {
                    read_int(stream, out delta_z, -Constants.LinearVelocityDeltaMax, +Constants.LinearVelocityDeltaMax);
                }
            }
        }
    }

    void write_angular_velocity_delta(Network.WriteStream stream, int delta_x, int delta_y, int delta_z)
    {

        uint unsigned_x = Network.Util.SignedToUnsigned(delta_x);
        uint unsigned_y = Network.Util.SignedToUnsigned(delta_y);
        uint unsigned_z = Network.Util.SignedToUnsigned(delta_z);

        bool small_x = unsigned_x <= Constants.AngularVelocityDeltaSmallThreshold;
        bool small_y = unsigned_y <= Constants.AngularVelocityDeltaSmallThreshold;
        bool small_z = unsigned_z <= Constants.AngularVelocityDeltaSmallThreshold;

        bool all_small = small_x && small_y && small_z;

        write_bool(stream, all_small);

        if (all_small)
        {
            write_bits(stream, unsigned_x, Constants.AngularVelocityDeltaSmallBits);
            write_bits(stream, unsigned_y, Constants.AngularVelocityDeltaSmallBits);
            write_bits(stream, unsigned_z, Constants.AngularVelocityDeltaSmallBits);
        }
        else
        {
            write_bool(stream, small_x);

            if (small_x)
            {
                write_bits(stream, unsigned_x, Constants.AngularVelocityDeltaSmallBits);
            }
            else
            {
                unsigned_x -= Constants.AngularVelocityDeltaSmallThreshold;

                bool medium_x = unsigned_x < Constants.AngularVelocityDeltaMediumThreshold;

                write_bool(stream, medium_x);

                if (medium_x)
                {
                    write_bits(stream, unsigned_x, Constants.AngularVelocityDeltaMediumBits);
                }
                else
                {
                    write_int(stream, delta_x, -Constants.AngularVelocityDeltaMax, +Constants.AngularVelocityDeltaMax);
                }
            }

            write_bool(stream, small_y);

            if (small_y)
            {
                write_bits(stream, unsigned_y, Constants.AngularVelocityDeltaSmallBits);
            }
            else
            {
                unsigned_y -= Constants.AngularVelocityDeltaSmallThreshold;

                bool medium_y = unsigned_y < Constants.AngularVelocityDeltaMediumThreshold;

                write_bool(stream, medium_y);

                if (medium_y)
                {
                    write_bits(stream, unsigned_y, Constants.AngularVelocityDeltaMediumBits);
                }
                else
                {
                    write_int(stream, delta_y, -Constants.AngularVelocityDeltaMax, +Constants.AngularVelocityDeltaMax);
                }
            }

            write_bool(stream, small_z);

            if (small_z)
            {
                write_bits(stream, unsigned_z, Constants.AngularVelocityDeltaSmallBits);
            }
            else
            {
                unsigned_z -= Constants.AngularVelocityDeltaSmallThreshold;

                bool medium_z = unsigned_z < Constants.AngularVelocityDeltaMediumThreshold;

                write_bool(stream, medium_z);

                if (medium_z)
                {
                    write_bits(stream, unsigned_z, Constants.AngularVelocityDeltaMediumBits);
                }
                else
                {
                    write_int(stream, delta_z, -Constants.AngularVelocityDeltaMax, +Constants.AngularVelocityDeltaMax);
                }
            }
        }
    }

    void read_angular_velocity_delta(Network.ReadStream stream, out int delta_x, out int delta_y, out int delta_z)
    {
        bool all_small;

        read_bool(stream, out all_small);

        uint unsigned_x;
        uint unsigned_y;
        uint unsigned_z;

        if (all_small)
        {
            read_bits(stream, out unsigned_x, Constants.AngularVelocityDeltaSmallBits);
            read_bits(stream, out unsigned_y, Constants.AngularVelocityDeltaSmallBits);
            read_bits(stream, out unsigned_z, Constants.AngularVelocityDeltaSmallBits);

            delta_x = Network.Util.UnsignedToSigned(unsigned_x);
            delta_y = Network.Util.UnsignedToSigned(unsigned_y);
            delta_z = Network.Util.UnsignedToSigned(unsigned_z);
        }
        else
        {
            bool small_x;

            read_bool(stream, out small_x);

            if (small_x)
            {
                read_bits(stream, out unsigned_x, Constants.AngularVelocityDeltaSmallBits);

                delta_x = Network.Util.UnsignedToSigned(unsigned_x);
            }
            else
            {
                bool medium_x;

                read_bool(stream, out medium_x);

                if (medium_x)
                {
                    read_bits(stream, out unsigned_x, Constants.AngularVelocityDeltaMediumBits);

                    delta_x = Network.Util.UnsignedToSigned(unsigned_x + Constants.AngularVelocityDeltaSmallThreshold);
                }
                else
                {
                    read_int(stream, out delta_x, -Constants.AngularVelocityDeltaMax, +Constants.AngularVelocityDeltaMax);
                }
            }

            bool small_y;

            read_bool(stream, out small_y);

            if (small_y)
            {
                read_bits(stream, out unsigned_y, Constants.AngularVelocityDeltaSmallBits);

                delta_y = Network.Util.UnsignedToSigned(unsigned_y);
            }
            else
            {
                bool medium_y;

                read_bool(stream, out medium_y);

                if (medium_y)
                {
                    read_bits(stream, out unsigned_y, Constants.AngularVelocityDeltaMediumBits);

                    delta_y = Network.Util.UnsignedToSigned(unsigned_y + Constants.AngularVelocityDeltaSmallThreshold);
                }
                else
                {
                    read_int(stream, out delta_y, -Constants.AngularVelocityDeltaMax, +Constants.AngularVelocityDeltaMax);
                }
            }

            bool small_z;

            read_bool(stream, out small_z);

            if (small_z)
            {
                read_bits(stream, out unsigned_z, Constants.AngularVelocityDeltaSmallBits);

                delta_z = Network.Util.UnsignedToSigned(unsigned_z);
            }
            else
            {
                bool medium_z;

                read_bool(stream, out medium_z);

                if (medium_z)
                {
                    read_bits(stream, out unsigned_z, Constants.AngularVelocityDeltaMediumBits);

                    delta_z = Network.Util.UnsignedToSigned(unsigned_z + Constants.AngularVelocityDeltaSmallThreshold);
                }
                else
                {
                    read_int(stream, out delta_z, -Constants.AngularVelocityDeltaMax, +Constants.AngularVelocityDeltaMax);
                }
            }
        }
    }
    */
}
