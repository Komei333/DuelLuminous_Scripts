using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
using DG.Tweening;


// �Q�[�����̏����S�ʁi���C���j

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

    // ���ԊǗ�
    int timeCount;

    // �V���O���g����
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
        // �f�b�L�𐶐�
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

        // �f�b�L���V���b�t������
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

    public void Restart()
    {
        // hand��Filed�̃J�[�h���폜
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
        // �J�[�h�����ꂼ���6���z��
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

        // �f�b�L�̈�ԏォ��J�[�h������
        int cardID = deck[0];
        deck.RemoveAt(0);
        CreateCard(cardID, hand);
    }

    void CreateCard(int cardID, Transform hand)
    {
        // �J�[�h�̐����ƃf�[�^�̎󂯓n��
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
        Debug.Log("Player�̃^�[��");

        // �t�B�[���h�̃J�[�h���U���\�ɂ���
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

    public void ExitBattleButton()
    {
        DOTween.KillAll();
        StopAllCoroutines();
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

    public void ShowTitleButton()
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
            Restart();
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

    public void StartSE2()
    {
        musicManager.PlaySE2();
    }

    public void StartSE3()
    {
        musicManager.PlaySE3();
    }
}