using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


// BGMやSEのスライダーの設定

public class OptionManager : MonoBehaviour
{
    [SerializeField] MusicManager musicManager;

    [SerializeField] GameObject windowPanel;

    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;

    [SerializeField] Button windowScreenButton;
    [SerializeField] Button fullScreenButton;

    public float bgmValue;
    public float seValue;

    Image windowScreenImage;
    Image fullScreenImage;

    Color windowScreenColor;
    Color fullScreenColor;

    void Start()
    {
        // 初回プレイ時に設定を初期化
        if (PlayerPrefs.GetString("FirstTime") == "")
        {
            // 保存
            PlayerPrefs.SetFloat("BGM", 1.0f);
            PlayerPrefs.SetFloat("SE", 1.0f);
            PlayerPrefs.SetString("Screen", "Full");
            PlayerPrefs.SetString("FirstTime", "false");
            PlayerPrefs.Save();

            // 1920×1080、フルスクリーン有効、リフレッシュレート60Hz
            Screen.SetResolution(1920, 1080, true, 60);
        }

        //BGMスライダーを動かした時の処理を登録
        bgmSlider.onValueChanged.AddListener(SetBGM);

        //SEスライダーを動かした時の処理を登録
        seSlider.onValueChanged.AddListener(SetSE);

        // スライダーに値を反映
        bgmSlider.value = (PlayerPrefs.GetFloat("BGM") + 30f) / 30f;
        seSlider.value = (PlayerPrefs.GetFloat("SE") + 30f) / 30f;

        // 音量調整
        musicManager.SetBGM();
        musicManager.SetSE();

        windowScreenImage = windowScreenButton.GetComponent<Image>();
        fullScreenImage = fullScreenButton.GetComponent<Image>();
        windowScreenColor = windowScreenImage.color;
        fullScreenColor = fullScreenImage.color;

        ChangeButtonAlpha();
    }

    public void SetBGM(float value)
    {
        if (value == 0)
        {
            bgmValue = -999f;
        }
        else
        {
            //-30～0に変換（相対量をdBに変換）
            bgmValue = -30f + (value * 30f);
        }

        // 保存
        PlayerPrefs.SetFloat("BGM", bgmValue);
        PlayerPrefs.Save();

        musicManager.SetBGM();
    }

    public void SetSE(float value)
    {
        if (value == 0)
        {
            seValue = -999f;
        }
        else
        {
            //-30～0に変換（相対量をdBに変換）
            seValue = -30f + (value * 30f);
        }

        // 保存
        PlayerPrefs.SetFloat("SE", seValue);
        PlayerPrefs.Save();

        musicManager.SetSE();
    }

    public void WindowScreenButton()
    {
        musicManager.PlaySE1();

        //保存
        PlayerPrefs.SetString("Screen", "Window");
        PlayerPrefs.Save();

        ChangeButtonAlpha();

        windowPanel.SetActive(true);
    }

    public void FullScreenButton()
    {
        musicManager.PlaySE1();

        //保存
        PlayerPrefs.SetString("Screen", "Full");
        PlayerPrefs.Save();

        ChangeButtonAlpha();

        // 1920×1080、フルスクリーン有効、リフレッシュレート60Hz
        Screen.SetResolution(1920, 1080, true, 60);
    }

    void ChangeButtonAlpha()
    {
        if (PlayerPrefs.GetString("Screen") == "Window")
        {
            // aは透明度を表す
            windowScreenColor.a = 1.0f;
            fullScreenColor.a = 0.5f;
        }
        else if (PlayerPrefs.GetString("Screen") == "Full")
        {
            // aは透明度を表す
            windowScreenColor.a = 0.5f;
            fullScreenColor.a = 1.0f;
        }

        windowScreenImage.color = windowScreenColor;
        fullScreenImage.color = fullScreenColor;
    }

    public void ScreenSizeButton1()
    {
        // 640×360、フルスクリーン無効、リフレッシュレート60Hz
        Screen.SetResolution(640, 360, false, 60);

        musicManager.PlaySE1();
        windowPanel.SetActive(false);
    }

    public void ScreenSizeButton2()
    {
        // 1280×720、フルスクリーン無効、リフレッシュレート60Hz
        Screen.SetResolution(1280, 720, false, 60);

        musicManager.PlaySE1();
        windowPanel.SetActive(false);
    }

    public void ScreenSizeButton3()
    {
        // 1920×1080、フルスクリーン無効、リフレッシュレート60Hz
        Screen.SetResolution(1920, 1080, false, 60);

        musicManager.PlaySE1();
        windowPanel.SetActive(false);
    }
}
