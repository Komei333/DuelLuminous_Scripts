using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


// BGMやSEのスライダーの設定

public class SliderManager : MonoBehaviour
{
    [SerializeField] MusicManager musicManager;

    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;

    public float bgmValue;
    public float seValue;

    void Start()
    {
        //BGMスライダーを動かした時の処理を登録
        bgmSlider.onValueChanged.AddListener(SetBGM);

        //SEスライダーを動かした時の処理を登録
        seSlider.onValueChanged.AddListener(SetSE);
    }

    public void SetBGM(float value)
    {
        //-20～0に変換
        bgmValue = -20f + (value * 20f);

        // 保存
        PlayerPrefs.SetFloat("BGM", bgmValue);
        PlayerPrefs.Save();

        musicManager.SetBGM();
    }

    public void SetSE(float value)
    {
        //-20～0に変換
        seValue = -20f + (value * 20f);

        // 保存
        PlayerPrefs.SetFloat("SE", seValue);
        PlayerPrefs.Save();

        musicManager.SetSE();
    }
}
