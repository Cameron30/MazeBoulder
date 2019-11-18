using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour {

    AudioSource m_MyAudioSource;

    bool running;

	// Use this for initialization
	void Start () {

        running = GetComponent<PlayerMovement>().running;
        /*Audio Source */
        m_MyAudioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        // Grab players' current running status
        running = GetComponent<PlayerMovement>().running;

		// Adding footstep sounds to player--------------------------------------------------

        // if player is running and audio source is NOT playing
        if (running && !m_MyAudioSource.isPlaying)
        {
            m_MyAudioSource.Play();
            Debug.Log("Running sound is playing");
        }
        // if audio source is playing and player is not running OR
        // PlayerMovement script is disabled
        else if ((m_MyAudioSource.isPlaying && !running) || GetComponent<PlayerMovement>() == false)
        {
            m_MyAudioSource.Stop();
            Debug.Log("Running sound has stopped");
        }
        else
        {
            // do nothing
        }
	}
}
