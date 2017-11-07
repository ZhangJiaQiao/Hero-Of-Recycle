using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene2ToScene1 : MonoBehaviour {
	private int scene;
    private bool addEvents = false;
	private monstersCreator mC;
    private bool beginTalk = false;
    public GameObject Player;
    public GameObject Panel;
	// Use this for initialization
	void Start () {
		scene = 2;
		GetComponent<Renderer> ().enabled = false;
		mC = Singleton<monstersCreator>.Instance;
    }

    void Update()
    {
        if(mC.currentAmount == 0 && mC.totalAmount == 0)
        {
            if(!beginTalk)
            {
                SSDirector.currentTask = "当前任务:离开酒店";
                Panel.GetComponent<Dialog3>().SetTalk();
                if (!addEvents)
                {
                    Dialog3.FinishTalkingEvent += FinishTalk;
                    addEvents = true;
                }
                Player.GetComponent<Soldier>().BeginTalk();
                beginTalk = true;
            }
        }
    }
    void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && mC.currentAmount == 0 && mC.totalAmount == 0) {
			SSDirector.playerPosition = new Vector3 (52.13f, 1f, 14.509f);
			SSDirector.currentScene++;
			SceneManager.LoadScene ("WebDemoScene");
		}
	}

    void FinishTalk()
    {
        Player.GetComponent<Soldier>().StopTalk();
        Dialog3.FinishTalkingEvent -= FinishTalk;
    }
}
