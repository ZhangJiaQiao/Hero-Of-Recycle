using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {
	public float maxVelocity = 25;
	public GameObject gun;
    public UIProgressBar HPBar;
    public UIProgressBar MPBar;
    private Role role;
	private float jumpColdTime = 0;
    private bool isTalking = false;

	void Start () {
        role = GetComponent<Role>();
	}

	void Update () {
        HPBar.GetComponent<HpUISlider>().UpdateVal(role.hp / 100);
        MPBar.GetComponent<MpUISlider>().UpdateVal(role.mp / 100);
		if (Input.GetKey(KeyCode.Escape)) {
			Cursor.lockState = CursorLockMode.None;
		}
        if(!isTalking)
        {
            directionCtrl();
            move();
			jump ();
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

	void jump() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (jumpColdTime <= 0) {
				GetComponent<Rigidbody> ().AddForce (transform.up.normalized * 7, ForceMode.Impulse);
				jumpColdTime = 1;
			} 
		} else {
			if (jumpColdTime > 0) {
				jumpColdTime -= Time.deltaTime;
			}
		} 
	}

    public void BeginTalk()
    {
        isTalking = true;
		gun.GetComponent<Gun> ().BeginTalk ();
    }
    public void StopTalk()
    {
        isTalking = false;
		gun.GetComponent<Gun> ().StopTalk ();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "otherTrash" || collision.gameObject.tag == "recyclableTrash"
            || collision.gameObject.tag == "foodTrash" || collision.gameObject.tag == "harmfulTrash")
        {
            role.hp -= collision.gameObject.GetComponent<characterProperty>().damageValue;
        }
    }
}
 