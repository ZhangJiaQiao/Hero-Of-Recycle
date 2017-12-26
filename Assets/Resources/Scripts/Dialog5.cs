using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog5 : MonoBehaviour {
    public delegate void FinishTalking();
    public static event FinishTalking FinishTalkingEvent;
    public string name1 = "Bill";
    public string name2 = "Sparta";
    public GameObject TalkingBubble;
    public Text NameText;
    public Text DialogText;
    private List<Speak> Dialog = new List<Speak>();
    private bool beginTalk = false;
    private int current = 0;
    // Use this for initialization
    void Start()
    {

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
        Dialog.Add(new Speak("这。。。这是什么。。。为什么怪物会不断从垃圾堆产生", name1));
        Dialog.Add(new Speak("难道这就是变异的垃圾怪物产生的原因？", name1));
        Dialog.Add(new Speak("比尔博士！！！竟然被你找到这里来了，我现在给你两个选择", name2));
        Dialog.Add(new Speak("第一为我做事，帮助我一起用垃圾怪物征服世界，我还可以放了你的好友克洛伊和家人，第二那就是你只能葬身此地，成为我的垃圾怪物的食物，哈哈哈哈！！！", name2));
        Dialog.Add(new Speak("我绝对不会为你做事的，一定会阻止到底，克洛伊我也一定会救出来的", name1));
        Dialog.Add(new Speak("那我们没什么可说的了，让我的怪物们陪你玩玩，正好他们饿了", name2));
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
