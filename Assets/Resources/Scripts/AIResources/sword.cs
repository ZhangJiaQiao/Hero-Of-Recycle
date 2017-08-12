using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour {
	public GameObject boneMonster;
	private Animator animator;

	void Start() {
		animator = boneMonster.GetComponent<Animator> ();
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player") {
			string clipName = animator.GetCurrentAnimatorClipInfo (0) [0].clip.name;
			if (clipName == "Attack") {
				HpUISlider hpCtrl = Singleton<HpUISlider>.Instance;
				Debug.Log (hpCtrl.getVal ());
				hpCtrl.UpdateVal (hpCtrl.getVal() - 0.01f);
			}
		}
	}
}
