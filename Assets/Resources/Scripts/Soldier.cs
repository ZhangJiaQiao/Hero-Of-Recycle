using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {
	public float maxVelocity = 50;
	public Camera c;

	void Start () {
		c = Camera.main;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update () {
		if (Input.GetKey(KeyCode.Escape)) {
			Cursor.lockState = CursorLockMode.None;
		}
		directionCtrl ();
		move ();
		shoot ();
	}
		
	void directionCtrl() {
		float rotationX = transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * 5;
		float rotationY = transform.localEulerAngles.x;
		transform.localEulerAngles = new Vector3 (rotationY, rotationX, 0);
	}

	void move() {
		Animator animator = GetComponent<Animator> ();
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
			Vector3 v = GetComponent<Rigidbody> ().velocity;
			if (v.sqrMagnitude < maxVelocity) {
				v += transform.forward.normalized;
				GetComponent<Rigidbody> ().velocity = v;	
			}
			animator.SetBool("run", true);	
		} else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			Vector3 v = GetComponent<Rigidbody> ().velocity;
			if (v.sqrMagnitude < maxVelocity) {
				v -= transform.forward.normalized;
				GetComponent<Rigidbody> ().velocity = v;	
			}
			animator.SetBool("run", true);
		} else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			Vector3 v = GetComponent<Rigidbody> ().velocity;
			if (v.sqrMagnitude < maxVelocity) {
				v -= transform.right.normalized;
				GetComponent<Rigidbody> ().velocity = v;	
			}
			animator.SetBool("run", true);	
		} else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			Vector3 v = GetComponent<Rigidbody> ().velocity;
			if (v.sqrMagnitude < maxVelocity) {
				v += transform.right.normalized;
				GetComponent<Rigidbody> ().velocity = v;	
			}
			animator.SetBool("run", true);	
		} else {
			animator.SetBool("run", false);
		}
	}

	void shoot() {
		Animator animator = GetComponent<Animator> ();
		string currentClip = animator.GetCurrentAnimatorClipInfo (0) [0].clip.name;
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			switch(currentClip) {
			case "Idle":
				animator.SetTrigger ("standShoot");
				break;	
			case "Run":
				animator.SetTrigger ("runShoot");
				break;
			} 
			myFactory mF = Singleton<myFactory>.Instance;
			GameObject bullet = mF.getBullet ();
			Vector3 point = c.ScreenToWorldPoint (new Vector3 (Screen.width/2, Screen.height/2, c.nearClipPlane));
			bullet.transform.position = point + transform.forward * 2;
			bullet.transform.forward = c.transform.forward;
			bullet.GetComponent<Rigidbody> ().AddForce (bullet.transform.forward.normalized * 10, ForceMode.Impulse);
		}
	}

}
 