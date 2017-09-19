using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class core : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter(Collision collision) {

		if (collision.gameObject.name != "Bullet(Clone)") {
			return;
		}
			
		if (collision.gameObject.tag == this.gameObject.tag) {
			this.SendMessageUpwards ("substract");
		} else {
			this.SendMessageUpwards ("add");
		}
	}

}
