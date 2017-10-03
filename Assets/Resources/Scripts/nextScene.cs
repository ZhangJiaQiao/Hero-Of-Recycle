using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextScene : MonoBehaviour {
	public List<monstersCreator> mCs;
	public string nextSceneName;
	public Vector3 nextScenePosition;

	// Update is called once per frame
	void Update () {
		int count = 0;
		for (int i = 0; i < mCs.Count; i++) {
			if (mCs[i].currentAmount == 0 && mCs[i].totalAmount == 0) {
				count++;
			}
		}
		if (count == mCs.Count) {
			SSDirector.currentScene++;
			SSDirector.currentTask = "消灭怪物";
			SSDirector.playerPosition = nextScenePosition;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene (nextSceneName);
		}
	}
}
