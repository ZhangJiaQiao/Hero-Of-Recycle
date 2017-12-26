using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingPoint1 : MonoBehaviour {
    public GameObject DialogPanel;
    public GameObject Player;
    // Use this for initialization
    void Start () {
        Dialog5.FinishTalkingEvent += FinishTalk;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Player.GetComponent<Soldier>().BeginTalk();
            DialogPanel.GetComponent<Dialog5>().SetTalk();
        }
    }

    void FinishTalk()
    {
        Player.GetComponent<Soldier>().StopTalk();
        DialogPanel.GetComponent<Dialog5>().enabled = false;
        Dialog5.FinishTalkingEvent -= FinishTalk;
        this.gameObject.SetActive(false);
    }
}
