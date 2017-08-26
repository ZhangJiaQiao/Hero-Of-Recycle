using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHotel : MonoBehaviour {
    public GameObject panel;
	// Use this for initialization
	void Start () {
        Dialog1.FinishTalkingEvent += Show;
	}
	
	// Update is called once per frame
	void Update () {
		if (Camera.main) {
			Animator animator = GetComponent<Animator> ();	
		}
	}

    void Show()
    {
        panel.SetActive(true);
    }
}
