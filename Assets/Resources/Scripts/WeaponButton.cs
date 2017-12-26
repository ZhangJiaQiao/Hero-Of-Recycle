using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour {
    private Button btn;
    private bool isHorver = false;
    public Image img;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        EventTriggerListener.Get(btn.gameObject).onEnter = OnButtonEnter;
        EventTriggerListener.Get(btn.gameObject).onExit = OnButtonExit;
        EventTriggerListener.Get(btn.gameObject).onClick = OnButtonClick;
    }

    void Update()
    {

    }
    private void OnButtonEnter(GameObject g)
    {
        if(g == btn.gameObject)
        {
            img.gameObject.SetActive(true);
            isHorver = true;
        }
    }

    private void OnButtonExit(GameObject g)
    {
        if (g == btn.gameObject)
        {
            img.gameObject.SetActive(false);
            isHorver = false;
        }
    }

    private void OnButtonClick(GameObject g)
    {
        if (g == btn.gameObject)
        {
            img.gameObject.SetActive(false);
            isHorver = false;
        }
    }
}
