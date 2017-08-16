using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class myFactory : MonoBehaviour {
	public GameObject bullet;
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
	}

	public GameObject getBullet() {
		if (bulletsFree.Count == 0) {
			GameObject newBullet = Instantiate<GameObject> (bullet);
			bulletsUsing.Add (newBullet);
			return newBullet;
		} else {
			GameObject bl = bulletsFree [0];
			bulletsFree.RemoveAt (0);
			bl.SetActive (true);
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
}

