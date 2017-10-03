using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Congratulation : MonoBehaviour {
    private int Monsternum = 1;
    public GameObject monster_creator;
    public GameObject Boss;
    public GameObject congratulation;
    public GameObject player;
    private GameObject Player;
    private boss BossDetail;
    private monstersCreator monsterDetail;
    private bool IsBossDead = false;
	// Use this for initialization
	void Start () {
        BossDetail =  Boss.GetComponent<boss>();
        BossDetail.destroyEvent += bossdead;
        monsterDetail = monster_creator.GetComponent<monstersCreator>();
    }
	
	// Update is called once per frame
	void Update () {
        Monsternum = BossDetail.GetCallNum();
        if(Monsternum == 0 && monsterDetail.currentAmount == 0 && IsBossDead && monsterDetail.totalAmount == 0)
        {
            if(congratulation.activeSelf == false)
            {
                Cursor.lockState = CursorLockMode.None;
                congratulation.SetActive(true);
                Camera.main.transform.parent = null;
                player.SetActive(false);
            }
        }
	}

    void bossdead()
    {
        IsBossDead = true;
    }
}
