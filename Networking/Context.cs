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
using System.Collections.Generic;

public class Context : MonoBehaviour
{
    //public GameObject[] remoteAvatar = new GameObject[Constants.MaxClients];
    //public GameObject[] remoteLinePrefabs = new GameObject[Constants.MaxClients];
    //public Material[] authorityMaterials = new Material[Constants.MaxAuthority];
    
    Snapshot lastSnapshot = new Snapshot();
    
    public void FixedUpdate()
    {
        CaptureSnapshot(lastSnapshot);

        ApplySnapshot(lastSnapshot, true);

        //AddStateToRingBuffer();
    }


    public Vector3 GetOrigin()
    {
        return this.gameObject.transform.position;
    }

    public void ApplyCubeStateUpdates(ref State cubeState, bool applySmoothing = false)
    {
        Vector3 origin = this.gameObject.transform.position;

        Snapshot.UpdateCubeState(GetComponent<Rigidbody>(), ref cubeState, ref origin);
    }

    public void CaptureSnapshot(Snapshot snapshot)
    {
        Vector3 origin = this.gameObject.transform.position;

        var rigidBody = GetComponent<Rigidbody>();

        Snapshot.GetState(rigidBody, ref snapshot.state, ref origin);
    }

    public void ApplySnapshot(Snapshot snapshot, bool skipHeldObjects)
    {
        Vector3 origin = this.gameObject.transform.position;

        var rigidBody = GetComponent<Rigidbody>();

        if (!snapshot.state.active && rigidBody.IsSleeping())
            return;

        Snapshot.UpdateCubeState(rigidBody, ref snapshot.state, ref origin);

    }

    public Snapshot GetLastSnapshot()
    {
        return lastSnapshot;
    }
}
