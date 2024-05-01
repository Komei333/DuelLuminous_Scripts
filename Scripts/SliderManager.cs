using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


// BGM��SE�̃X���C�_�[�̐ݒ�

public class SliderManager : MonoBehaviour
{
    [SerializeField] MusicManager musicManager;

    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;

    public float bgmValue;
    public float seValue;

    void Start()
    {
        //BGM�X���C�_�[�𓮂��������̏�����o�^
        bgmSlider.onValueChanged.AddListener(SetBGM);

        //SE�X���C�_�[�𓮂��������̏�����o�^
        seSlider.onValueChanged.AddListener(SetSE);
    }

    public void SetBGM(float value)
    {
        //-20�`0�ɕϊ�
        bgmValue = -20f + (value * 20f);

        // �ۑ�
        PlayerPrefs.SetFloat("BGM", bgmValue);
        PlayerPrefs.Save();

        musicManager.SetBGM();
    }

    public void SetSE(float value)
    {
        //-20�`0�ɕϊ�
        seValue = -20f + (value * 20f);

        // �ۑ�
        PlayerPrefs.SetFloat("SE", seValue);
        PlayerPrefs.Save();

        musicManager.SetSE();
    }
}
