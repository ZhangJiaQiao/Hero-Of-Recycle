using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSDirector : System.Object {
	private static SSDirector _instance;
	private int currentScene = 0;

	public ISceneController currentSceneController;

	public SSDirector getInstance() {
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

	public int getCurrentScene() {
		return currentScene;
	}

	public void nextScene() {
		//切换下一场景
	}
}