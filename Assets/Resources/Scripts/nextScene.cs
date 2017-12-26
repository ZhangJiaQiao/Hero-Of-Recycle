using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextScene : MonoBehaviour {
	public List<monstersCreator> mCs;
	public string nextSceneName;
	public Vector3 nextScenePosition;
    public GameObject Boss;
    private int MonsterCount;
    private int BossCount = 0;
    private bool IsBossDead = false;
    private boss2 BossDetail;

    void Start()
    {
        MonsterCount = mCs.Count;
        if(Boss != null)
        {
            BossCount = 1;
            BossDetail = Boss.GetComponent<boss2>();
            BossDetail.destroyEvent += bossdead;
        }
    }
    // Update is called once per frame
    void Update () {
        if(BossDetail != null && BossDetail.GetCallNum() == 0 && IsBossDead)
        {
            BossCount = 0;
        }
		int count = 0;
		for (int i = 0; i < MonsterCount; i++) {
			if (mCs[i].currentAmount == 0 && mCs[i].totalAmount == 0) {
				count++;
			}
		}
		if (count == MonsterCount + BossCount) {
			SSDirector.currentScene++;
			SSDirector.currentTask = "消灭怪物";
			SSDirector.playerPosition = nextScenePosition;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene (nextSceneName);
		}
	}

    void bossdead()
    {
        IsBossDead = true;
    }
}
