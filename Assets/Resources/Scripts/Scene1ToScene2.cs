using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1ToScene2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Renderer> ().enabled = false;
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			SSDirector.getInstance ().nextScene ();
			Debug.Log ("haha");
		}
	}
}
