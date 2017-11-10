using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdSceneController : MonoBehaviour, ISceneController, AI_Request, UI_Request, Player_Request {
	// Use this for initialization
	public GameObject player;
    public GameObject PausePanel;
    private bool pause = false;

    void Awake() {
		SSDirector director = SSDirector.getInstance ();
		director.currentSceneController = this;
	}

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!pause)
            {
                player.GetComponent<Soldier>().BeginTalk();
                PausePanel.SetActive(true);
                SSDirector.getInstance().pause();
                pause = true;
            }
            else
            {
                player.GetComponent<Soldier>().StopTalk();
                SSDirector.getInstance().Recover();
                PausePanel.SetActive(false);
                pause = false;
            }
        }
        //判断游戏是否结束
        //判断任务是否完成，如果完成，切换到下一关
    }

	public void LoadRecourses() {

	}

    public void SetPause()
    {
        player.GetComponent<Soldier>().StopTalk();
        pause = false;
        PausePanel.SetActive(false);
    }

    public Transform getPlayer() {
		return this.player.transform;
	}
}
