using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPanel : MonoBehaviour {
    public GameObject Solider;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape))
        {
            PanelExit();
            Solider.GetComponent<Soldier>().StopTalk();
        }
    }

    public void PanelExit()
    {
        this.gameObject.SetActive(false);
    }
}
