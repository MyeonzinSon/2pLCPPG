using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip BGM0;
    public AudioClip BGM1;
    public AudioClip BGM2;
    public AudioClip BGM3;

    public AudioClip PLAYERONEJUMP;
    public AudioClip PLAYERTWOJUMP;
    public AudioClip PLAYERONEABILLITY;
    public AudioClip ITEMNOTESEVEN;
    public AudioClip ITEMBALLOTPAPER;
    public AudioClip ITEMBLUEPILL;
    public AudioClip NOTESEVENEXPLODE;
    public AudioClip SWITCHCLICK;
    public AudioClip COLLAPSE;

    IEnumerator PlayRepeat(AudioClip clip)
    {
        AudioSource audio = GetComponent<AudioSource>();

        audio.clip = clip;
        while (true)
        {
            audio.Play();
            yield return new WaitForSeconds(audio.clip.length);
        }
    }
    IEnumerator PlayOnce(AudioClip clip)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = clip;
        audio.Play();
        yield return null;
    }
}
