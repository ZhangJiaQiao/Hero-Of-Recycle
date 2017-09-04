using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene2ToScene1 : MonoBehaviour {
	private int scene;
	private monstersCreator mC;
	// Use this for initialization
	void Start () {
		scene = 2;
		GetComponent<Renderer> ().enabled = false;
		mC = Singleton<monstersCreator>.Instance;
	}


	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && mC.totalAmount <= 0) {
			SSDirector.playerPosition = new Vector3 (-8.59f, 1f, 5.53f);
			SSDirector.currentScene++;
			SceneManager.LoadScene ("WebDemoScene");
		}
	}


}
