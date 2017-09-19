using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTexture : MonoBehaviour {
    private Animator ani;
    private bool show = false;
	// Use this for initialization
	void Start () {
        ani = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!show)
            {
                ani.SetBool("Show", true);
                show = true;
            }
            else
            {
                ani.SetBool("Show", false);
                show = false;
            }
        }
	}
}
