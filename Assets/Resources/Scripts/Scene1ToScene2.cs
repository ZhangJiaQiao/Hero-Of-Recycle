using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1ToScene2 : MonoBehaviour {
	private int scene;
	// Use this for initialization
	void Start () {
		scene = 1;
		GetComponent<Renderer> ().enabled = false;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && SSDirector.currentScene == scene) {
			SSDirector.currentScene++;
			SSDirector.playerPosition = new Vector3 (2.91f, 1.066504f, 18.03f);
			SceneManager.LoadScene ("Hotel");
		}
	}
}
