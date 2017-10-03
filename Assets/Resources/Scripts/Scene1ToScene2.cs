using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1ToScene2 : MonoBehaviour {
	private int scene;
    public GameObject Player;
    public GameObject DialogPanel;
    public GameObject Explosion;
    public GameObject Villager;
    public GameObject Des;
    // Use this for initialization
    void Start () {
		scene = 1;
		GetComponent<Renderer> ().enabled = false;
        if (SSDirector.currentScene == 3)
        {
            Dialog2.FinishTalkingEvent += FinishTalk;
        }
    }

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && SSDirector.currentScene == scene) {
			SSDirector.currentScene++;
            SSDirector.currentTask = "消灭怪物";
			SSDirector.playerPosition = new Vector3 (2.81f, 1.06f, 18.03f);
			SceneManager.LoadScene ("Hotel");
		}
        if (other.gameObject.tag == "Player" && SSDirector.currentScene == 3)
        {
            DialogPanel.GetComponent<Dialog2>().SetTalk();
            Player.GetComponent<Soldier>().BeginTalk();
            Villager.SetActive(false);
            Explosion.SetActive(true);
            Des.SetActive(true);
            SSDirector.currentTask = "前往查看";
            StartCoroutine("Dispear");
        }
    }

    void FinishTalk()
    {
        DialogPanel.SetActive(false);
        Player.GetComponent<Soldier>().StopTalk();
        Dialog2.FinishTalkingEvent -= FinishTalk;
    }

    IEnumerator Dispear()
    {
        yield return new WaitForSeconds(5);
        Explosion.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
