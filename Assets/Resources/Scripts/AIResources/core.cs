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
		
		Debug.Log ("ccc");


		if (collision.gameObject.name != "Bullet(Clone)") {
			Debug.Log ("qqqq");
			return;
		}
			
		if (collision.gameObject.tag == this.gameObject.tag) {
			Debug.Log ("aaaa");
			this.SendMessageUpwards ("substract");
		} else {
			Debug.Log ("bbbb");
			this.SendMessageUpwards ("add");
		}
	}

}
