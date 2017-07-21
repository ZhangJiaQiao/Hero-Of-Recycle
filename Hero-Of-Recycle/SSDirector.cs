using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSDirector : System.Object {//不被Unity内存管理控制
	private static SSDirector _instance;

	public ISceneController currentSceneController { get; set;}

	public static SSDirector getInstance() {
		if (_instance == null) {
			_instance = new SSDirector ();
		}
		return _instance;
	}

	public int getFPS() {
		return Application.targetFrameRate;
	}

	public int setFPS(int fps) {
		Application.targetFrameRate = fps;
	}

	public void start() {//控制游戏开始
	}

	public void exit() {//控制游戏退出
		
	}

	public void nextScene() {//切换至下一游戏场景
	}

	public void selectScene(int index) {//选择某一场景
	}

	public int getCurrentScene() {//返回当前场景
		return 0;
	}
}




