using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog1 : MonoBehaviour {
    public delegate void FinishTalking();
    public static event FinishTalking FinishTalkingEvent;
    public string name1 = "Bill";
    public string name2 = "Villager";
    public Image BillBubble;
    public Image VillageBubble;
    private Animator BillBubbleAni;
    private Animator VillageBubbleAni;
    public Text BillText;
    public Text VillageText;
    private List<Speak> Dialog = new List<Speak>();
    private int current = 0;
    private int Time = 0;
    private int Index = 0;
    private int count = 0;
	// Use this for initialization
	void Start () {
        IniDialog();
        BillBubbleAni = BillBubble.GetComponent<Animator>();
        VillageBubbleAni = VillageBubble.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Time = 0;
            Index = 0;
            count = 0;
            StopCoroutine("Next");
        }
		if(current < Dialog.Count)
        {
            if(Dialog[current].speaker == name1)
            {
                Talk(BillText);
            }
            else
            {
                Talk(VillageText);
            }
        }
        else
        {
            if(FinishTalkingEvent != null)
            {
                FinishTalkingEvent();
            }
        }
	}

    void IniDialog()
    {
        Dialog.Add(new Speak("为什么今天", name1));
        Dialog.Add(new Speak("大街空无一人", name1));
        Dialog.Add(new Speak("救命", name2));
        Dialog.Add(new Speak("发生什么事了", name1));
        Dialog.Add(new Speak("因为小镇被怪物包围", name2));
        Dialog.Add(new Speak("镇长下令坚守城门", name2));
        Dialog.Add(new Speak("大家都躲起来", name2));
        Dialog.Add(new Speak("才过了一两天平静的日子", name2));
        Dialog.Add(new Speak("酒店准备重新开业", name2));
        Dialog.Add(new Speak("没想到在酒店的垃圾堆",name2));
        Dialog.Add(new Speak("出现很多垃圾怪物", name2));
        Dialog.Add(new Speak("酒店的人都在逃命", name2));
        Dialog.Add(new Speak("酒店就在前面直走", name2));
    }

    void Talk(Text TalkText)
    {
        if(Time == 0)
        {
            TalkText.text = Dialog[current].content;
            if (Dialog[current].speaker == name1)
            {
                BillBubbleAni.SetBool("Big", true);
                Time = 1;
                StartCoroutine("Show");
            }
            else
            {
                VillageBubbleAni.SetBool("Big", true);
                Time = 1;
                StartCoroutine("Show");
            }
        }

        if(Time == -1)
        {
            if(Dialog[current].speaker == name1)
            {
                BillBubbleAni.SetBool("Big", false);
            }
            else
            {
                VillageBubbleAni.SetBool("Big", false);
            }
            TalkText.text = "";
            current++;
            Time = 2;
            StartCoroutine("Next");
        }
        if(TalkText.text.Length > 5 && count == 70)
        {
            Index++;
            TalkText.text = TalkText.text.Substring(Index);
            count = 0;
        }
        count++;
    }

    IEnumerator Show()
    {
        yield return new WaitForSeconds(3);
        Time = -1;
    }

    IEnumerator Next()
    {
        yield return new WaitForSeconds(1);
        Time = 0;
        Index = 0;
        count = 0;
    }
}
