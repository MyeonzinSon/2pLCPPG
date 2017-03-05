using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmPlayer : MonoBehaviour {

    public AudioClip clip;

    void Start()
    {
        StartPlay();
    }
    public void StartPlay()
    {
        StartCoroutine(PlayBgm());
    }
    public void StopPlay()
    {
        StopCoroutine(PlayBgm());
        StartCoroutine(StopBgm());
    }
    IEnumerator PlayBgm()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = clip;
        while (true)
        {
            audio.Play();
            yield return new WaitForSeconds(audio.clip.length);
        }
    }
    IEnumerator StopBgm()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Stop();
        yield return null;
    }
}
