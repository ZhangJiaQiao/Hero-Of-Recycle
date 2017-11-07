using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog2 : MonoBehaviour {
    public delegate void FinishTalking();
    public static event FinishTalking FinishTalkingEvent;
    public string name1 = "Bill";
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
        Dialog.Add(new Speak("为什么今天前面火光冲天？前面是...", name1));
        Dialog.Add(new Speak("不好，前面是工业区，难道垃圾怪物是要去那里吸收工业区能量？我一定得阻止你们", name1));
        Dialog.Add(new Speak("我决不允许你们破坏我的家乡，这里装载一切我最美好的记忆", name1));
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
