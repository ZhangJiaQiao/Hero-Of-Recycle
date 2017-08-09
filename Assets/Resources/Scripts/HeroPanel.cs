using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(30, Screen.height - 42, 4));
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(30, Screen.height - 42, 4));
        this.transform.rotation = Camera.main.transform.rotation;
    }
}
