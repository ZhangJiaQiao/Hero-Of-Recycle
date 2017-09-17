using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashCarControl : MonoBehaviour {
	private float damageValue;
	private float speed;
	// Use this for initialization
	void Start () {
		damageValue = 50;
		speed = 5;
	}

	// Update is called once per frame
	void Update () {
		this.gameObject.GetComponent<Rigidbody> ().velocity = speed * this.transform.up.normalized * -1;
	}

	void OnCollisionEnter(Collision other) {
		if (other.rigidbody) {
			if (other.gameObject.GetComponent<characterProperty> ()) {
				other.gameObject.GetComponent<characterProperty> ().life -= damageValue;
			}
		}
	}
}
