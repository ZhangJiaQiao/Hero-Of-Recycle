using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModelSelect : MonoBehaviour {
    public void SelectStoryMode()
    {
        SceneManager.LoadScene("SceneIntroduction01");
    }

    public void SelectNetMode()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("menu");
    }
}
