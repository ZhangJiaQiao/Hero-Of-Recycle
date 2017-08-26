using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bulletType : MonoBehaviour {
	private myFactory mF;
	private Image img;
	public List<Sprite> bullet_sprites;
	// Use this for initialization
	void Start () {
		mF = Singleton<myFactory>.Instance;
		img = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		setBulletType ();
	}

	void setBulletType() {
		string type = mF.getCurrentBulletType ();
		switch(type) {
		case "foodTrash":
			img.sprite = bullet_sprites [0];
			break;
		case "recyclableTrash":
			img.sprite = bullet_sprites [1];
			break;
		case "otherTrash":
			img.sprite = bullet_sprites [2];
			break;
		case "harmfulTrash":
			img.sprite = bullet_sprites [3];
			break;
		}
	}
}
