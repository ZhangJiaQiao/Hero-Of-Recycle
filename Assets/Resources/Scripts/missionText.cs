using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class missionText : MonoBehaviour {

    public Text Mission;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Mission.text = SSDirector.currentTask;
	}
}
