using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public GameObject Main;
    public float damageValue = 8f;
    // Use this for initialization
    void Start () {
        if(Main != null)
        {
            damageValue = Main.GetComponent<characterProperty>().damageValue;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Main != null)
        {
            damageValue = Main.GetComponent<characterProperty>().damageValue;
        }
    }
}
