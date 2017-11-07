using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingPoint : MonoBehaviour {
    public GameObject Player;
    public GameObject DialogPanel;
    public GameObject Villager;
	// Use this for initialization
	void Start () {
        if (SSDirector.currentScene == 1)
        {
            Dialog1.FinishTalkingEvent += FinishTalk;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && SSDirector.currentScene == 1)
        {
            Player.GetComponent<Soldier>().BeginTalk();
            Villager.GetComponent<Villager>().canMove(Player.transform.position);
        }
        if(other.gameObject.tag == "Villager" && SSDirector.currentScene == 1)
        {
            DialogPanel.GetComponent<Dialog1>().SetTalk();
            Villager.GetComponent<Villager>().Talk();
        }
    }
    
    void FinishTalk()
    {
        SSDirector.currentTask = "当前任务:前往酒店";
        Villager.GetComponent<Villager>().canMove(new Vector3(110, 0.5f, 14.5f));
        Player.GetComponent<Soldier>().StopTalk();
        DialogPanel.SetActive(false);
        this.gameObject.SetActive(false);
        Dialog1.FinishTalkingEvent -= FinishTalk;
    }
}
