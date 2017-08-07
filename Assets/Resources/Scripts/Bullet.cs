using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	void OnCollisionEnter(Collision other) {
		myFactory mF = Singleton<myFactory>.Instance;
		if (other.gameObject.tag != "Player") {
			mF.recycleBullet(this.gameObject);	
		}
	}
}
