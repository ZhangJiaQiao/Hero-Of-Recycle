using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pain : MonoBehaviour {
	private Image img;
	public float a;
	// Use this for initialization
	void Start () {
		setA (0);
	}
	
	// Update is called once per frame
	void Update () {
		setA (this.a);
		if (this.a > 0) {
			this.a -= Time.deltaTime * 0.3f;
		}
	}

	public void setA(float a) {
		img = GetComponent<Image> ();
		Color c = img.color;
		c.a = a;
		img.color = c;
	}

	public void showPain() {
		this.a = 1;
	}
}
