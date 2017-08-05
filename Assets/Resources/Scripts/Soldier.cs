using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {
	public float maxVelocity = 25;
	public GameObject gun;
	private float shootColdTime = 0;
    private bool isTalking = false;

	void Start () {
		
	}

	void Update () {
		if (Input.GetKey(KeyCode.Escape)) {
			Cursor.lockState = CursorLockMode.None;
		}
        if(!isTalking)
        {
            directionCtrl();
            move();
        }
	}
		
	void directionCtrl() {
		float rotationX = transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * 5;
		float rotationY = transform.localEulerAngles.x;
		transform.localEulerAngles = new Vector3 (rotationY, rotationX, 0);
	}

	void move() {
		Animator animator = gun.GetComponent<Animator> ();
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
			Vector3 v = GetComponent<Rigidbody> ().velocity;
			if (v.sqrMagnitude < maxVelocity) {
				v += transform.forward.normalized;
				GetComponent<Rigidbody> ().velocity = v;	
			}
			animator.SetBool("walk", true);	
		} else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			Vector3 v = GetComponent<Rigidbody> ().velocity;
			if (v.sqrMagnitude < maxVelocity) {
				v -= transform.forward.normalized;
				GetComponent<Rigidbody> ().velocity = v;	
			}
			animator.SetBool("walk", true);
		} else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			Vector3 v = GetComponent<Rigidbody> ().velocity;
			if (v.sqrMagnitude < maxVelocity) {
				v -= transform.right.normalized;
				GetComponent<Rigidbody> ().velocity = v;	
			}
			animator.SetBool("walk", true);	
		} else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			Vector3 v = GetComponent<Rigidbody> ().velocity;
			if (v.sqrMagnitude < maxVelocity) {
				v += transform.right.normalized;
				GetComponent<Rigidbody> ().velocity = v;	
			}
			animator.SetBool("walk", true);	
		} else {
			animator.SetBool("walk", false);
		}
	}

    public void BeginTalk()
    {
        isTalking = true;
    }
    public void StopTalk()
    {
        isTalking = false;
    }

}
 