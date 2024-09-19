using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// ゲーム内のUI関連

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;

    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject rulePanel;
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject menuPanel;

    [SerializeField] Text playerHeroHpText;
    [SerializeField] Text enemyHeroHpText;

    [SerializeField] Text playerManaCostText;
    [SerializeField] Text enemyManaCostText;

    [SerializeField] Text timeCountText;

    public void HideResultPanel()
    {
        resultPanel.SetActive(false);
    }

    public void ShowManaCost(int playerManaCost, int enemyManaCost)
    {
        playerManaCostText.text = playerManaCost.ToString();
        enemyManaCostText.text = enemyManaCost.ToString();
    }

    public void UpdateTime(int timeCount)
    {
        timeCountText.text = timeCount.ToString();
    }

    public void ShowHeroHP(int playerHeroHp, int enemyHeroHp)
    {
        if (playerHeroHp < 0)
        {
            playerHeroHp = 0;
        }
        else if (enemyHeroHp < 0)
        {
            enemyHeroHp = 0;
        }

        playerHeroHpText.text = playerHeroHp.ToString();
        enemyHeroHpText.text = enemyHeroHp.ToString();
    }

    public void ShowResultPanel(int heroHp)
    {
        resultPanel.SetActive(true);
        if (heroHp <= 0)
        {
            resultText.text = "-  LOSE  -";
        }
        else
        {
            resultText.text = "-  WIN  -";
        }
    }

    public void ShowTitlePanel()
    {
        titlePanel.SetActive(true);
    }

    public void HideTitlePanel()
    {
        titlePanel.SetActive(false);
    }

    public void ShowRulePanel()
    {
        rulePanel.SetActive(true);
    }

    public void HideRulePanel()
    {
        rulePanel.SetActive(false);
    }

    public void ShowOptionPanel()
    {
        optionPanel.SetActive(true);
    }

    public void HideOptionPanel()
    {
        optionPanel.SetActive(false);
    }

    public void ShowMenuPanel()
    {
        // ヒエラルキーの一番下に移動(最前面に表示)
        menuPanel.transform.SetAsLastSibling();
        menuPanel.SetActive(true);
    }

    public void HideMenuPanel()
    {
        menuPanel.SetActive(false);
    }
}