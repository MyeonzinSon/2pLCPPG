using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {

    public AudioClip clip;

    void Start()
    {
        StartCoroutine(PlaySound());
    }
    IEnumerator PlaySound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = clip;
        audio.Play();
        yield return null;
    }
}
