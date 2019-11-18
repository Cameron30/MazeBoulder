using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingSound : MonoBehaviour {
    AudioSource audio;
    [SerializeField] float audioCutoffYPos = -18.0f;

    // Use this for initialization
    void Start() {
        audio = GetComponent<AudioSource>();
        audio.playOnAwake = true;
        audio.loop = true;

        audio.Play();


    }

    // Update is called once per frame
    void Update() {
        if(this.transform.position.y < audioCutoffYPos)
        {
            audio.Pause();
        }
    } 
}
