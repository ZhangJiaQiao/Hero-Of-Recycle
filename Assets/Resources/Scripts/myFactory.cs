using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class myFactory : MonoBehaviour {
	public GameObject bullet;
	private List<GameObject> bulletsUsing;
	private List<GameObject> bulletsFree;

	void Start() {
		bulletsFree = new List<GameObject> ();
		bulletsUsing = new List<GameObject> ();
	}

	public GameObject getBullet() {
		if (bulletsFree.Count == 0) {
			GameObject newBullet = Instantiate<GameObject> (bullet);
			bulletsUsing.Add (newBullet);
			return newBullet;
		} else {
			GameObject bullet = bulletsFree [0];
			bulletsFree.RemoveAt (0);
			return bullet;
		}
	}
}

