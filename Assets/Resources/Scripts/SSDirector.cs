using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSDirector : System.Object {
	private static SSDirector _instance;
	public static int currentScene = 1;
	public static Vector3 playerPosition = new Vector3(112.1f, 1f, 8.6f);
    public static string currentTask = "消灭怪物";

	public ISceneController currentSceneController;

	public static SSDirector getInstance() {
		if (_instance == null) {
			_instance = new SSDirector ();
		}
		return _instance;
	}

	public int getFPS() {
		return Application.targetFrameRate;
	}

	public void setFPS(int fps) {
		Application.targetFrameRate = fps;
	}

	public void start() {
		//开始游戏
	}

	public void exit() {
		//退出游戏
	}
}