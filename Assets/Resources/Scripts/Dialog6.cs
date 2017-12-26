using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog6 : MonoBehaviour {

    public delegate void FinishTalking();
    public static event FinishTalking FinishTalkingEvent;
    public string name1 = "Bill";
    public string name2 = "Sparta";
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
        Dialog.Add(new Speak("竟然被你发现我们的地下城基地，可恶", name2));
        Dialog.Add(new Speak("斯巴达，你的计划不会得逞的，我会消灭完你们的怪物的，我劝你不要无畏抵抗，放了克洛伊", name1));
        Dialog.Add(new Speak("克洛伊？哈哈哈哈，看来你还被蒙在鼓里，真是个傻瓜", name2));
        Dialog.Add(new Speak("什么。。。你说清楚。。。", name1));
        Dialog.Add(new Speak("反正你今天没办法活着离开这里了，告诉你事实也可以，克洛伊其实是我的搭档，是他提供了技术帮我研究生化垃圾怪物", name2));
        Dialog.Add(new Speak("不。。。这。。。不可能。。。你一定在骗我", name1));
        Dialog.Add(new Speak("他就在后面的生化地狱，你自己去问他，不过你恐怕没这个机会了，待会给你见识见识我的黑暗天使", name2));
    }

    void Talk()
    {
        if (Dialog[current].speaker == name1)
        {
            NameText.text = name1;
        }
        else
        {
            NameText.text = name2;
        }
        DialogText.text = Dialog[current].content;
    }
}
