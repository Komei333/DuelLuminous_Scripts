using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// プレイヤー関連

public class GamePlayerManager : MonoBehaviour
{
    public List<int> deck = new List<int>();

    public int heroHp;
    public int manaCost;
    public int defaultManaCost;

    public void Init(List<int> cardDeck)
    {
        deck = cardDeck;
        heroHp = 10;
        manaCost = 1;
        defaultManaCost = 1;
    }

    public void IncreaseManaCost()
    {
        defaultManaCost++;
        manaCost = defaultManaCost;
    }
}