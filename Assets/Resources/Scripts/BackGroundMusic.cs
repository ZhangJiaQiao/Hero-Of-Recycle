using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour {
    public GameObject MusicList;
    public GameObject Sound;
    public AudioClip[] music = new AudioClip[3];
    private AudioSource _audioSource;
    private int choice = 0;
	// Use this for initialization
	void Start () {
        _audioSource = this.GetComponent<AudioSource>();
        _audioSource.clip = music[SSDirector.choice];
        _audioSource.volume = SSDirector.volume;
        _audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
        if (MusicList.GetComponent<UIPopupList>().value == "Music1")
        {
            if(choice != 0)
            {
                choice = 0;
                _audioSource.clip = music[0];
                _audioSource.Play();
            }
        }
        else if(MusicList.GetComponent<UIPopupList>().value == "Music2")
        {
            if(choice != 1)
            {
                choice = 1;
                _audioSource.clip = music[1];
                _audioSource.Play();
            }
        }
        else if (MusicList.GetComponent<UIPopupList>().value == "Music3")
        {
            if (choice != 2)
            {
                choice = 2;
                _audioSource.clip = music[2];
                _audioSource.Play();
            }
        }

        _audioSource.volume = Sound.GetComponent<UISlider>().value;
        SSDirector.choice = choice;
        SSDirector.volume = _audioSource.volume;
    }
}
