using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class myFactory : MonoBehaviour {
	public GameObject bullet;
	private List<string> bullet_types;
	private int currentBulletType;
	private List<GameObject> bulletsUsing;
	private List<GameObject> bulletsFree;

	public GameObject Firebullet;
	private List<GameObject> FirebulletsUsing;
	private List<GameObject> FirebulletsFree;
	void Start() {
		bulletsFree = new List<GameObject> ();
		bulletsUsing = new List<GameObject> ();
		FirebulletsUsing = new List<GameObject> ();
		FirebulletsFree = new List<GameObject> ();
		bullet_types = new List<string> {"foodTrash", "recyclableTrash", "otherTrash", "harmfulTrash"};
		currentBulletType = 0;
	}
		
	void Update() {
		setBulletType ();
	}

	public GameObject getBullet() {
		if (bulletsFree.Count == 0) {
			GameObject newBullet = Instantiate<GameObject> (bullet);
			newBullet.tag = bullet_types[currentBulletType];
			bulletsUsing.Add (newBullet);
			return newBullet;
		} else {
			GameObject bl = bulletsFree [0];
			bulletsFree.RemoveAt (0);
			bl.SetActive (true);
			bl.tag = bullet_types [currentBulletType];
			//bl.GetComponent<Rigidbody> ().WakeUp ();
			//bl.GetComponent<Rigidbody> ().isKinematic = false;
			return bl;
		}
	}
		
	public void recycleBullet(GameObject bullet) {
		bulletsUsing.Remove(bullet);
		bulletsFree.Add (bullet);
		//Renderer renderer = bullet.GetComponent<Renderer> ();
		Rigidbody rb = bullet.GetComponent<Rigidbody> ();
		rb.velocity = Vector3.zero;
		bullet.SetActive (false);
	}

	public GameObject getFireBullet() {
		if (FirebulletsFree.Count == 0) {
			GameObject newBullet = Instantiate<GameObject> (Firebullet);
			FirebulletsUsing.Add (newBullet);
			return newBullet;
		} else {
			GameObject bl = FirebulletsFree [0];
			FirebulletsFree.RemoveAt (0);
			bl.SetActive (true);
			//bl.GetComponent<Rigidbody> ().WakeUp ();
			//bl.GetComponent<Rigidbody> ().isKinematic = false;
			return bl;
		}
	}

	private void setBulletType() {
		if (Input.GetKeyDown(KeyCode.Tab)) {
			currentBulletType += 1;
			currentBulletType %= 4;
		}
	}

	public string getCurrentBulletType() {
		return bullet_types[currentBulletType];
	}
}

