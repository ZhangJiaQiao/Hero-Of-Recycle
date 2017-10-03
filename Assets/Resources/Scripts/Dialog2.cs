using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog2 : MonoBehaviour {
    public delegate void FinishTalking();
    public static event FinishTalking FinishTalkingEvent;
    public Image BillBubble;
    public Text BillText;
    public string name1 = "Bill";
    private Animator BillBubbleAni;
    private List<Speak> Dialog = new List<Speak>();
    private int current = 0;
    private int Time = 0;
    private int Index = 0;
    private int count = 0;
    private bool beginTalk = false;
    // Use this for initialization
    void Start () {
        BillBubbleAni = BillBubble.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if(beginTalk)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Time = 0;
                Index = 0;
                count = 0;
                StopCoroutine("Next");
            }
            if (current < Dialog.Count)
            {
                if (Dialog[current].speaker == name1)
                {
                    Talk(BillText);
                }
            }
            else
            {
                if (FinishTalkingEvent != null)
                {
                    FinishTalkingEvent();
                }
            }
        }
    }

    public void SetTalk()
    {
        beginTalk = true;
        IniDialog();
    }

    void IniDialog()
    {
        Dialog.Add(new Speak("为什么前面", name1));
        Dialog.Add(new Speak("火光冲天", name1));
        Dialog.Add(new Speak("肯定出事了", name1));
        Dialog.Add(new Speak("我得去看看", name1));
    }

    void Talk(Text TalkText)
    {
        if (Time == 0)
        {
            TalkText.text = Dialog[current].content;
            if (Dialog[current].speaker == name1)
            {
                BillBubbleAni.SetBool("Big", true);
                Time = 1;
                StartCoroutine("Show");
            }
        }

        if (Time == -1)
        {
            if (Dialog[current].speaker == name1)
            {
                BillBubbleAni.SetBool("Big", false);
            }
            TalkText.text = "";
            current++;
            Time = 2;
            StartCoroutine("Next");
        }
        if (TalkText.text.Length > 5 && count == 70)
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
