using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour {

    public void BackToGame()
    {
        SSDirector.getInstance().Recover();
    }

    public void BackToMain()
    {
        SSDirector.currentScene = 1;
        SSDirector.playerPosition = new Vector3(112.1f, 1f, 8.6f);
        SSDirector.currentTask = "消灭怪物";
        SceneManager.LoadScene("menu");
    }
}
