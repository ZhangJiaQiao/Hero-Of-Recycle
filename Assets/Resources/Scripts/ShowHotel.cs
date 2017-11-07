using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHotel : MonoBehaviour {
    public GameObject panel;
    public GameObject effect;
	// Use this for initialization
	void Start () {
        Dialog1.FinishTalkingEvent += Show;
	}
	
	// Update is called once per frame
	void Update () {
        panel.transform.rotation = Camera.main.transform.rotation;
	}

    void Show()
    {
        panel.SetActive(true);
        effect.SetActive(true);
        Dialog1.FinishTalkingEvent -= Show;
    }
}
