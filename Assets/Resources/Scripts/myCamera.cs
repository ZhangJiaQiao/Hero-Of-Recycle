using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myCamera : MonoBehaviour {

	void Start () {

	}

	void Update () {
		float rotationX = transform.localEulerAngles.y;
		float rotationY = transform.localEulerAngles.x - Input.GetAxis ("Mouse Y");
		if (rotationY < 10 || rotationY > 350) {
			transform.localEulerAngles = new Vector3 (rotationY, rotationX, 0);		
		}
	}
}
