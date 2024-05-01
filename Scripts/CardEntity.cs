using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.Experimental.GraphView;

//[CreateAssetMenu(fileName = "CardEntity", menuName = "Create CardEntity")]


// カードデータそのもの

public class CardEntity : ScriptableObject
{
    public Sprite panel;
    public Sprite icon;
    public Sprite back;

    public int cost;
    public int at;
    public int hp;
    public new string name;
    public ABILITY ability;
    public SPELL spell;
}

public enum ABILITY
{
    NONE,
    INIT_ATTACKABLE,
    SHIELD,
}

public enum SPELL
{
    NONE,
    DAMAGE_ENEMY_CARD,
    DAMAGE_ENEMY_CARDS,
    DAMAGE_ENEMY_HERO,
    HEAL_FRIEND_CARD,
    HEAL_FRIEND_CARDS,
    HEAL_FRIEND_HERO,
}