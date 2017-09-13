using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour {
	// Use this for initialization
	public Text text;
	public GameObject target;
	int flag = 0;
	string []word = {"比尔：\n为什么今天大街空无一人？", "服务员：\n救命！", "比尔：\n发生什么事了？", "服务员：\n酒店里出现了很多怪物！", ""};
	int index = 0;
	void start() {

	}
	
	// Update is called once per frame
	void Update () {
		if (flag == 0)
			text.text = word[0];
		if (Input.GetKeyDown(KeyCode.Space)) {
			index++;
			text.text = word[index];
			flag = 1;
		}
		if (index >= 4) {
			Destroy (target);
		}
	}
}
