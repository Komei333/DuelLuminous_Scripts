using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


// ÉãÅ[Éãê‡ñæâÊñ ÇÃê›íË

public class RuleManager : MonoBehaviour
{
    [SerializeField] MusicManager musicManager;

    [SerializeField] GameObject leftButton;
    [SerializeField] GameObject rightButton;

    [SerializeField] GameObject rule1Image;
    [SerializeField] GameObject rule2Image;
    [SerializeField] GameObject rule3Image;
    [SerializeField] GameObject rule4Image;
    [SerializeField] GameObject rule5Image;
    [SerializeField] GameObject rule6Image;
    [SerializeField] GameObject rule7Image;

    int ruleNum = 1;

    void Start()
    {
        leftButton.SetActive(false);
    }

    public void LeftButton()
    {
        if (ruleNum == 2)
        {
            rule1Image.SetActive(true);
            rule2Image.SetActive(false);
        }
        else if (ruleNum == 3)
        {
            rule2Image.SetActive(true);
            rule3Image.SetActive(false);
        }
        else if (ruleNum == 4)
        {
            rule3Image.SetActive(true);
            rule4Image.SetActive(false);
        }
        else if (ruleNum == 5)
        {
            rule4Image.SetActive(true);
            rule5Image.SetActive(false);
        }
        else if (ruleNum == 6)
        {
            rule5Image.SetActive(true);
            rule6Image.SetActive(false);
        }
        else if (ruleNum == 7)
        {
            rule6Image.SetActive(true);
            rule7Image.SetActive(false);
        }

        ruleNum--;
        CheckButtonActive();
        musicManager.PlaySE1();
    }

    public void RightButton()
    {
        if (ruleNum == 1)
        {
            rule2Image.SetActive(true);
            rule1Image.SetActive(false);
        }
        else if (ruleNum == 2)
        {
            rule3Image.SetActive(true);
            rule2Image.SetActive(false);
        }
        else if (ruleNum == 3)
        {
            rule4Image.SetActive(true);
            rule3Image.SetActive(false);
        }
        else if (ruleNum == 4)
        {
            rule5Image.SetActive(true);
            rule4Image.SetActive(false);
        }
        else if (ruleNum == 5)
        {
            rule6Image.SetActive(true);
            rule5Image.SetActive(false);
        }
        else if (ruleNum == 6)
        {
            rule7Image.SetActive(true);
            rule6Image.SetActive(false);
        }

        ruleNum++;
        CheckButtonActive();
        musicManager.PlaySE1();
    }

    void CheckButtonActive()
    {
        if (ruleNum == 1)
        {
            leftButton.SetActive(false);
        }
        else if (ruleNum == 7)
        {
            rightButton.SetActive(false);
        }
        else
        {
            leftButton.SetActive(true);
            rightButton.SetActive(true);
        }
    }
}