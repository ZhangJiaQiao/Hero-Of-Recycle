using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog3 : MonoBehaviour {
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
                this.gameObject.SetActive(false);
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
        Dialog.Add(new Speak("终于打完了，看来科技子弹的研发还是成功的", name1));
        Dialog.Add(new Speak("外面突然巨响，莫非出了什么事", name1));
        Dialog.Add(new Speak("我得先离开酒店，看看外面究竟发生什么事了", name1));
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
