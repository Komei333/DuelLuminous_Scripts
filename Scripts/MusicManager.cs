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

    bool seInterval = false;

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
        if (!seInterval)
        {
            seAudioSource.PlayOneShot(se1);
            seInterval = true;
            Invoke("EndInterval", 0.2f);
        }
    }

    public void PlaySE2()
    {
        if (!seInterval)
        {
            seAudioSource.PlayOneShot(se2);
            seInterval = true;
            Invoke("EndInterval", 0.2f);
        }
    }

    public void PlaySE3()
    {
        if (!seInterval)
        {
            seAudioSource.PlayOneShot(se3);
            seInterval = true;
            Invoke("EndInterval", 0.2f);
        }
    }

    void EndInterval()
    {
        seInterval = false;
    }
}