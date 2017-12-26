using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog4 : MonoBehaviour {
    public delegate void FinishTalking();
    public static event FinishTalking FinishTalkingEvent;
    public string name1 = "Bill";
    public GameObject TalkingBubble;
    public GameObject player;
    public Text NameText;
    public Text DialogText;
    public GameObject MonsterCreater;
    private List<Speak> Dialog = new List<Speak>();
    private bool beginTalk = false;
    private int current = 0;
    // Use this for initialization
    void Start()
    {
        SetTalk();
        player.GetComponent<Soldier>().BeginTalk();
    }

    // Update is called once per frame
    void Update()
    {
        if (beginTalk)
        {
            if (current < Dialog.Count)
            {
                Talk();
            }
            else
            {
                if (FinishTalkingEvent != null)
                {
                    //Debug.Log("finishEvent");
                    FinishTalkingEvent();
                }
                MonsterCreater.SetActive(true);
                player.GetComponent<Soldier>().StopTalk();
                TalkingBubble.SetActive(false);
                enabled = false;
            }


            if (Input.GetKeyDown(KeyCode.Space))
            {
                current++;
            }
        }
    }

    public void SetTalk()
    {
        TalkingBubble.SetActive(true);
        beginTalk = true;
        IniDialog();
    }

    void IniDialog()
    {
        Dialog.Add(new Speak("这些怪物是怎么来的，城门紧锁，却在酒店垃圾堆出现", name1));
        Dialog.Add(new Speak("难道是垃圾堆能产生怪物吗？我还是跟上去一看究竟", name1));
    }

    void Talk()
    {
        if (Dialog[current].speaker == name1)
        {
            NameText.text = name1;
        }
        DialogText.text = Dialog[current].content;
    }
}
