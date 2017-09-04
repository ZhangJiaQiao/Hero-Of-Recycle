using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, AI_Request, UI_Request, Player_Request {
	// Use this for initialization
	public GameObject player;

	void Awake() {
		SSDirector director = SSDirector.getInstance ();
		director.currentSceneController = this;
	}

	void Start() {
		player.transform.position = SSDirector.playerPosition;
	}
	
	// Update is called once per frame
	void Update () {
		//判断游戏是否结束
		//判断任务是否完成，如果完成，切换到下一关
	}

	public void LoadRecourses() {
		
	}

	public Transform getPlayer() {
		return this.player.transform;
	}
}
