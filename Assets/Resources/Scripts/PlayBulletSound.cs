using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBulletSound : MonoBehaviour {
    private AudioSource _audiosource;
    public List<AudioClip> BulletSound;
	// Use this for initialization
	void Start () {
        _audiosource = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!_audiosource.isPlaying)
        {
            myFactory mF = Singleton<myFactory>.Instance;
            mF.recycleBulletSound(this.gameObject);
        }
	}

    public void PlaySound()
    {
        _audiosource = this.GetComponent<AudioSource>();
        _audiosource.clip = BulletSound[SSDirector.CurrentWeapon];
        _audiosource.Play();
    }
}
