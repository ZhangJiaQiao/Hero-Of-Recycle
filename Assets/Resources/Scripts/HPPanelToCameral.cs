using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPanelToCameral : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.rotation = Camera.main.transform.rotation;
	}
}
