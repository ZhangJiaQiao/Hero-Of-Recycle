using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class myFactory : MonoBehaviour {
	public GameObject bullet;
	public GameObject grenade;
	private List<string> bullet_types;
	private int currentBulletType;
	private List<GameObject> bulletsUsing;
	private List<GameObject> bulletsFree;

	private List<GameObject> grenadeUsing;
	private List<GameObject> grenadeFree;

	public List<GameObject> otherTrash;
	public List<GameObject> foodTrash;
	public List<GameObject> recyclableTrash;
	public List<GameObject> harmfulTrash;

	public GameObject Firebullet;
	private List<GameObject> FirebulletsUsing;
	private List<GameObject> FirebulletsFree;
	void Start() {
		bulletsFree = new List<GameObject> ();
		bulletsUsing = new List<GameObject> ();
		grenadeUsing = new List<GameObject> ();
		grenadeFree = new List<GameObject> ();
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



	public GameObject getGrenade() {
		if (grenadeFree.Count == 0) {
			GameObject newGrenade= Instantiate<GameObject> (grenade);
			grenadeUsing.Add (newGrenade);
			return newGrenade;
		} else {
			GameObject bl = grenadeFree [0];
			grenadeFree.RemoveAt (0);
			bl.SetActive (true);
			return bl;
		}
	}

	public void recycleGrenade(GameObject bullet) {
		//		bullet.GetComponent<SphereCollider> ().radius = 0.06f;
		grenadeUsing.Remove(bullet);
		grenadeFree.Add (bullet);
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

	public GameObject getMonster(string type) {
		GameObject monster = null;
		int index;
		switch (type) {
		case "foodTrash": 
			index = Random.Range (0, foodTrash.Count);
			monster = Instantiate<GameObject> (foodTrash [index]);
			break;
		case "harmfulTrash": 
			index = Random.Range(0, harmfulTrash.Count);
			monster = Instantiate<GameObject>(harmfulTrash[index]);
			break;
		case "recyclableTrash":
			index = Random.Range (0, recyclableTrash.Count);
			monster = Instantiate<GameObject> (recyclableTrash [index]);
			break;
		case "otherTrash":
			index = Random.Range (0, otherTrash.Count);
			monster = Instantiate<GameObject> (otherTrash [index]);
			break;
		}
		return monster;
	}
}

