using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayScript : MonoBehaviour {
	public string targetScene;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnClick() {
		Debug.Log ("the Play button is Clicked!");
		SceneManager.LoadScene (targetScene);
	}
}
