using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// カードデータそのものとその処理

public class CardModel
{
    public Sprite panel;
    public Sprite icon;
    public Sprite back;

    public int cost;
    public int at;
    public int hp;
    public string name;
    public ABILITY ability;
    public SPELL spell;

    public bool isAlive;
    public bool canAttack;
    public bool isFieldCard;
    public bool isPlayerCard;

    public CardModel(int cardID, bool isPlayer)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);
        panel = cardEntity.panel;
        icon = cardEntity.icon;
        back = cardEntity.back;

        cost = cardEntity.cost;
        at = cardEntity.at;
        hp = cardEntity.hp;
        name = cardEntity.name;

        ability = cardEntity.ability;
        spell = cardEntity.spell;
        isAlive = true;
        isPlayerCard = isPlayer;
    }

    void Damage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            hp = 0;
            isAlive = false;
        }
    }

    // 自分を回復する
    void RecoveryHP(int point)
    {
        hp += point;
    }

    public void Attack(CardController card)
    {
        card.model.Damage(at);
    }

    // cardを回復させる
    public void Heal(CardController card)
    {
        card.model.RecoveryHP(hp);
    }
}