using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playMusic : MonoBehaviour {
    public AudioClip[] music = new AudioClip[3];
    private AudioSource _audioSource;
    // Use this for initialization
    void Start () {
        _audioSource = this.GetComponent<AudioSource>();
        _audioSource.clip = music[SSDirector.choice];
        _audioSource.volume = SSDirector.volume;
        _audioSource.Play();
	}
}
