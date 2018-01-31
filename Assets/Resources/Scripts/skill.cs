using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill : MonoBehaviour {
	private Role role;
	private Camera c;
	private bool isTalking;
	private bool isDied;
	private float countTime;
	public GameObject trashCar;
	public GameObject tornado;
	// Use this for initialization
	void Start () {
		countTime = 0;
		role = GetComponent<Role>();
		c = Camera.main;
		isTalking = false;
		isDied = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!isDied && Input.GetKeyDown(KeyCode.Mouse1)) {
			useSkill ();
		}
		//addMp ();
	}

	void useSkill() {
		float Mp = role.mp;
		if (Mp >= 100f) {
			useTornado ();
		} else if (Mp >= 67f) {
			useTrashCar ();
		} else if( Mp >= 34f){
			useGrenade ();
		}
	}

	//void addMp() {
	//	countTime += Time.fixedDeltaTime;
	//	if (countTime > 1) {					
	//		role.mp += 1;
	//		countTime = 0;
	//	}
	//}

	void useTornado () {
		GameObject tornado1 = Instantiate<GameObject> (tornado);
		GameObject tornado2 = Instantiate<GameObject> (tornado);
		tornado1.transform.position = this.transform.position + 6* this.transform.forward.normalized + 3* this.transform.right.normalized;
		tornado2.transform.position = this.transform.position + 6* this.transform.forward.normalized - 3* this.transform.right.normalized;

		tornado1.transform.forward = this.transform.forward;
		Vector3 rotation1 = tornado1.transform.localEulerAngles; 
		rotation1.x = -90;
		tornado1.transform.localEulerAngles = rotation1;

		tornado2.transform.forward = this.transform.forward;
		Vector3 rotation2 = tornado2.transform.localEulerAngles; 
		rotation2.x = -90;
		tornado2.transform.localEulerAngles = rotation2;
		role.mp -= 33;
	}

	void useTrashCar() {
		GameObject trashcar = Instantiate<GameObject> (trashCar);
		trashcar.transform.position = this.transform.position + 6* this.transform.forward.normalized;

		trashcar.transform.forward = this.transform.forward;
		Vector3 rotation = trashcar.transform.localEulerAngles; 
		rotation.x = -90;
		trashcar.transform.localEulerAngles = rotation;
		role.mp -= 33;

	}

	void useGrenade() {
		myFactory mF = Singleton<myFactory>.Instance;
		GameObject bullet = mF.getGrenade ();
		Vector3 point = c.ScreenToWorldPoint (new Vector3 (Screen.width/2, Screen.height/2, c.nearClipPlane));
		bullet.transform.right = c.transform.forward * -1;
		bullet.transform.position = point + c.transform.forward.normalized;
		bullet.GetComponent<Rigidbody> ().AddForce (c.transform.forward.normalized * 20, ForceMode.Impulse);
		role.mp -= 33;

	//	role.mp -= 33;

	}
	public void BeginTalk() {
		isTalking = true;
	}

	public void StopTalk() {
		isTalking = false;
	}

	public void isDead() {
		isDied = true;
	}
}
