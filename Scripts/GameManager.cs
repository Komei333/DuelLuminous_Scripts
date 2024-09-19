using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
using DG.Tweening;


// ゲーム内の処理全般（メイン）

public class GameManager : MonoBehaviour
{
    public GamePlayerManager player;
    public GamePlayerManager enemy;

    [SerializeField] AI enemyAI;
    [SerializeField] UIManager uiManager;
    [SerializeField] MusicManager musicManager;

    public Transform playerHandTransform, playerFieldTransform, enemyHandTransform, enemyFieldTransform;
    [SerializeField] CardController cardPrefab;

    public bool isPlayerTurn;
    public Transform playerHero;
    public Transform enemyHero;

    bool firstGame = true;

    // 時間管理
    int timeCount;

    // シングルトン化
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        musicManager.PlayBGM1();
    }

    void StartGame()
    {
        uiManager.HideTitlePanel();
        uiManager.HideResultPanel();
        player.Init(new List<int>() {});
        enemy.Init(new List<int>() {});
        CreateDeck();

        uiManager.ShowHeroHP(player.heroHp, enemy.heroHp);
        uiManager.ShowManaCost(player.manaCost, enemy.manaCost);
        SettingInitHand();
        isPlayerTurn = true;
        TurnCalc();

        musicManager.StopBGM();
        musicManager.PlayBGM2();
        musicManager.PlaySE1();
    }

    void CreateDeck()
    {
        // デッキを生成
        player.deck = new List<int>() { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 
                                        3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 
                                        6, 7, 7, 8, 8, 9, 10, 11, 11,
                                        11, 12, 12, 12, 13, 13,
                                        14, 14, 15, 15, 16, 16};

        enemy.deck = new List<int>() { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3,
                                        3, 3, 4, 4, 4, 5, 5, 5, 6, 6,
                                        6, 7, 7, 8, 8, 9, 10, 11, 11,
                                        11, 12, 12, 12, 13, 13,
                                        14, 14, 15, 15, 16, 16};

        // デッキをシャッフルする
        player.deck = player.deck.OrderBy(a => Guid.NewGuid()).ToList();
        enemy.deck = enemy.deck.OrderBy(a => Guid.NewGuid()).ToList();
    }

    public void ReduceManaCost(int cost, bool isPlayerCard)
    {
        if (isPlayerCard)
        {
            player.manaCost -= cost;
        }
        else
        {
            enemy.manaCost -= cost;
        }

        uiManager.ShowManaCost(player.manaCost, enemy.manaCost);
    }

    public void RetryButton()
    {
        Time.timeScale = 1;
        uiManager.HideMenuPanel();

        // handとFiledのカードを削除
        foreach (Transform card in playerHandTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in playerFieldTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in enemyHandTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in enemyFieldTransform)
        {
            Destroy(card.gameObject);
        }

        StartGame();
    }

    void SettingInitHand()
    {
        // カードをそれぞれに6枚配る
        for (int i = 0; i < 6; i++)
        {
            GiveCardToHand(player.deck, playerHandTransform);
            GiveCardToHand(enemy.deck, enemyHandTransform);
        }
    }

    void GiveCardToHand(List<int> deck, Transform hand)
    {
        if (deck.Count == 0)
        {
            return;
        }

        // デッキの一番上からカードを引く
        int cardID = deck[0];
        deck.RemoveAt(0);
        CreateCard(cardID, hand);
    }

    void CreateCard(int cardID, Transform hand)
    {
        // カードの生成とデータの受け渡し
        CardController card = Instantiate(cardPrefab, hand, false);

        if (hand.name == "PlayerHand")
        {
            card.Init(cardID, true);
        }
        else
        {
            card.Init(cardID, false);
        }
    }

    void TurnCalc()
    {
        StopAllCoroutines();
        StartCoroutine(CountDown());

        if (isPlayerTurn)
        {
            PlayerTurn();
        }
        else
        {
            StartCoroutine(enemyAI.EnemyTurn());
        }
    }

    IEnumerator CountDown()
    {
        timeCount = 30;
        uiManager.UpdateTime(timeCount);

        while (timeCount > 0)
        {
            yield return new WaitForSeconds(1);
            timeCount--;
            uiManager.UpdateTime(timeCount);
        }

        ChangeTurn();
    }

    public CardController[] GetEnemyFieldCards(bool isPlayer)
    {
        if (isPlayer)
        {
            return enemyFieldTransform.GetComponentsInChildren<CardController>();
        }
        else
        {
            return playerFieldTransform.GetComponentsInChildren<CardController>();
        }
    }
    public CardController[] GetFriendFieldCards(bool isPlayer)
    {
        if (isPlayer)
        {
            return playerFieldTransform.GetComponentsInChildren<CardController>();
        }
        else
        {
            return enemyFieldTransform.GetComponentsInChildren<CardController>();
        }
    }

    public void OnClickTurnEndButton()
    {
        if (isPlayerTurn)
        {
            ChangeTurn();
            musicManager.PlaySE1();
        }
    }

    public void ChangeTurn()
    {
        isPlayerTurn = !isPlayerTurn;

        CardController[] playerHandCardList = playerHandTransform.GetComponentsInChildren<CardController>();
        CardController[] playerFieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        SettingCanAttackView(playerFieldCardList, false);

        CardController[] enemyHandCardList = enemyHandTransform.GetComponentsInChildren<CardController>();
        CardController[] enemyFieldCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();
        SettingCanAttackView(enemyFieldCardList, false);


        if (isPlayerTurn)
        {
            player.IncreaseManaCost();

            if (playerHandCardList.Length < 9)
            {
                GiveCardToHand(player.deck, playerHandTransform);
            }
        }
        else
        {
            enemy.IncreaseManaCost();

            if (enemyHandCardList.Length < 9)
            {
                GiveCardToHand(enemy.deck, enemyHandTransform);
            }
        }

        uiManager.ShowManaCost(player.manaCost, enemy.manaCost);
        TurnCalc();
    }

    public void SettingCanAttackView(CardController[] fieldCardList, bool canAttack)
    {
        foreach (CardController card in fieldCardList)
        {
            card.SetCanAttack(canAttack);
        }
    }

    void PlayerTurn()
    {
        Debug.Log("Playerのターン");

        // フィールドのカードを攻撃可能にする
        CardController[] playerFieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        SettingCanAttackView(playerFieldCardList, true);
    }

    public void CardsBattle(CardController attacker, CardController defender)
    {
        //Debug.Log("CardsBattle");
        //Debug.Log("attacker HP:" + attacker.model.hp);
        //Debug.Log("defender HP:" + defender.model.hp);

        attacker.Attack(defender);
        defender.Attack(attacker);

        //Debug.Log("attacker HP:" + attacker.model.hp);
        //Debug.Log("defender HP:" + defender.model.hp);

        attacker.CheckAlive();
        defender.CheckAlive();
    }

    public void AttackToHero(CardController attacker)
    {
        if (attacker.model.isPlayerCard)
        {
            enemy.heroHp -= attacker.model.at;
        }
        else
        {
            player.heroHp -= attacker.model.at;
        }

        attacker.SetCanAttack(false);
        uiManager.ShowHeroHP(player.heroHp, enemy.heroHp);
        musicManager.PlaySE2();
    }

    public void HealToHero(CardController healer)
    {
        if (healer.model.isPlayerCard)
        {
            player.heroHp += healer.model.hp;
        }
        else
        {
            enemy.heroHp += healer.model.hp;
        }

        uiManager.ShowHeroHP(player.heroHp, enemy.heroHp);
        musicManager.PlaySE3();
    }

    public void CheckHeroHP()
    {
        if (player.heroHp <= 0 || enemy.heroHp <= 0)
        {
            ShowResultPanel(player.heroHp);
        }
    }

    public void ReturnTitleButton()
    {
        Time.timeScale = 1;
        DOTween.KillAll();
        StopAllCoroutines(); 

        uiManager.HideMenuPanel();
        uiManager.ShowTitlePanel();

        musicManager.StopBGM();
        musicManager.StopSE();
        musicManager.PlayBGM1();
        musicManager.PlaySE1();
    }

    void ShowResultPanel(int heroHp)
    {
        StopAllCoroutines();
        uiManager.ShowResultPanel(heroHp);
    }

    public void ShowTitlePanelButton()
    {
        uiManager.ShowTitlePanel();
        uiManager.HideResultPanel();
        musicManager.StopBGM();
        musicManager.PlayBGM1();
        musicManager.PlaySE1();
    }

    public void StartGameButton()
    {
        if (firstGame)
        {
            StartGame();
            firstGame = false;
        }
        else
        {
            RetryButton();
        }
    }

    public void OpenRuleButton()
    {
        uiManager.ShowRulePanel();
        uiManager.HideTitlePanel();
        musicManager.PlaySE1();
    }

    public void EscapeRuleButton()
    {
        uiManager.ShowTitlePanel();
        uiManager.HideRulePanel();
        musicManager.PlaySE1();
    }

    public void OpenOptionButton()
    {
        uiManager.ShowOptionPanel();
        uiManager.HideTitlePanel();
        musicManager.PlaySE1();
    }

    public void EscapeOptionButton()
    {
        uiManager.ShowTitlePanel();
        uiManager.HideOptionPanel();
        musicManager.PlaySE1();
    }

    public void ExitGameButton()
    {
        musicManager.PlaySE1();
        Application.Quit();
    }

    public void OpenMenuButton()
    {
        Time.timeScale = 0;
        musicManager.PlaySE1();
        uiManager.ShowMenuPanel();
    }

    public void EscapeMenuButton()
    {
        Time.timeScale = 1;
        musicManager.EndSEInterval();
        musicManager.PlaySE1();
        uiManager.HideMenuPanel();
    }

    public void PlayAttackSE()
    {
        musicManager.PlaySE2();
    }

    public void PlayHealSE()
    {
        musicManager.PlaySE3();
    }
}