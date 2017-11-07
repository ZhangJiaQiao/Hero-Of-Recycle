using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog1 : MonoBehaviour {
    public delegate void FinishTalking();
    public static event FinishTalking FinishTalkingEvent;
    public string name1 = "Bill";
    public string name2 = "Villager";
    public GameObject TalkingBubble;
    public Text NameText;
    public Text DialogText;
    private List<Speak> Dialog = new List<Speak>();
    private bool beginTalk = false;
    private int current = 0; 
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if(beginTalk)
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
                this.enabled = false;
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
        Dialog.Add(new Speak("奇怪了？为什么今天威尼镇的大街空无一人？", name1));
        Dialog.Add(new Speak("救命！！！救命！！！救命！！！", name2));
        Dialog.Add(new Speak("发生什么事了？", name1));
        Dialog.Add(new Speak("因为小镇被怪物包围，镇长下令坚守城门，大家都躲起来，才过了一两天平静的日子。", name2));
        Dialog.Add(new Speak("我们以为怪物危机过了，酒店准备重新开业，没想到在酒店的垃圾堆，出现很多垃圾怪物。", name2));
        Dialog.Add(new Speak("现在酒店的人都在逃命，太可怕了。", name2));
        Dialog.Add(new Speak("我是科学研究所的比尔，我带了新研发的科技子弹过来消灭怪物，酒店在哪里？", name1));
        Dialog.Add(new Speak("酒店就沿着这条路直走就行，就在前面，我先走了", name2));
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
