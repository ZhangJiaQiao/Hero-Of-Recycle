using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour {
    public List<Sprite> icon = new List<Sprite>(2);
    public Text task;
    private Animator ani;
    private bool show = false;
	// Use this for initialization
	void Start () {
        ani = task.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        task.text = SSDirector.currentTask;
        if (Input.GetKeyDown(KeyCode.X))
        {
            ShowTask();
        }
	}

    public void ShowTask()
    {
        if(show)
        {
            ani.SetBool("show", false);
            this.GetComponent<Image>().sprite = icon[1];
            show = false;
        }
        else
        {
            ani.SetBool("show", true);
            this.GetComponent<Image>().sprite = icon[0];
            show = true;
        }
    }
}
