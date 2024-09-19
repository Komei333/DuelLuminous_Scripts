using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;


// BGMÇ‚SEÇÃê›íË 

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioSource seAudioSource;

    [SerializeField] AudioClip bgm1;
    [SerializeField] AudioClip bgm2;

    [SerializeField] AudioClip se1;
    [SerializeField] AudioClip se2;
    [SerializeField] AudioClip se3;

    bool isSEInterval = false;
    float seIntervalValue = 0.2f;

    public void SetBGM()
    {
        audioMixer.SetFloat("BGM", PlayerPrefs.GetFloat("BGM"));
    }

    public void SetSE()
    {
        audioMixer.SetFloat("SE", PlayerPrefs.GetFloat("SE"));
    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }

    public void StopSE()
    {
        seAudioSource.Stop();
    }

    public void PlayBGM1()
    {
        bgmAudioSource.PlayOneShot(bgm1);
    }

    public void PlayBGM2()
    {
        bgmAudioSource.PlayOneShot(bgm2);
    }

    public void PlaySE1()
    {
        if (isSEInterval) return;

        seAudioSource.PlayOneShot(se1);
        isSEInterval = true;
        Invoke("EndSEInterval", seIntervalValue);
    }

    public void PlaySE2()
    {
        if (isSEInterval) return;

        seAudioSource.PlayOneShot(se2);
        isSEInterval = true;
        Invoke("EndSEInterval", seIntervalValue);
    }

    public void PlaySE3()
    {
        if (isSEInterval) return;

        seAudioSource.PlayOneShot(se3);
        isSEInterval = true;
        Invoke("EndSEInterval", seIntervalValue);
    }

    public void EndSEInterval()
    {
        isSEInterval = false;
    }
}