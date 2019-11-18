using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign_Text : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject text = new GameObject();
        TextMesh t = text.AddComponent<TextMesh>();
        t.text = "new text set";
        t.fontSize = 30;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
