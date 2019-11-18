using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitSlide : MonoBehaviour {

    SlidingTrap st;

	// Use this for initialization
	void Start () {
        st = GameObject.Find("/SlidingTrap/Trigger").GetComponent<SlidingTrap>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            st.playerCount -= 1;
        }
    }
}
