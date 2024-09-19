using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// 見た目(view)に関することを操作

public class CardView : MonoBehaviour
{
    [SerializeField] Image panelImage;
    [SerializeField] Image iconImage;
    [SerializeField] Image backImage;
    [SerializeField] Image atBackImage;
    [SerializeField] Image hpBackImage;

    [SerializeField] Text costText;
    [SerializeField] Text atText;
    [SerializeField] Text hpText;
    [SerializeField] Text nameText;
    [SerializeField] Text damageAllText;
    [SerializeField] Text damagePlayerText;
    [SerializeField] Text healAllText;
    [SerializeField] Text healPlayerText;

    [SerializeField] GameObject selectablePanel;
    [SerializeField] GameObject atackablePanel;
    [SerializeField] GameObject shieldPanel;
    [SerializeField] GameObject maskPanel;

    public void SetCard(CardModel cardModel)
    {
        panelImage.sprite = cardModel.panel;
        iconImage.sprite = cardModel.icon;
        backImage.sprite = cardModel.back;
        costText.text = cardModel.cost.ToString();
        atText.text = cardModel.at.ToString();
        hpText.text = cardModel.hp.ToString();
        nameText.text = cardModel.name;
        maskPanel.SetActive(!cardModel.isPlayerCard);

        if (cardModel.ability == ABILITY.INIT_ATTACKABLE)
        {
            atackablePanel.SetActive(true);
        }
        else if (cardModel.ability == ABILITY.SHIELD)
        {
            shieldPanel.SetActive(true);
        }
        else
        {
            shieldPanel.SetActive(false);
        }


        // スペルカードの場合

        if (cardModel.spell == SPELL.DAMAGE_ENEMY_CARD || cardModel.spell == SPELL.DAMAGE_ENEMY_CARDS 
            || cardModel.spell == SPELL.DAMAGE_ENEMY_HERO)
        {
            backImage.gameObject.SetActive(true);
            iconImage.gameObject.SetActive(false);
            hpBackImage.gameObject.SetActive(false);
            hpText.gameObject.SetActive(false);
        }

        if (cardModel.spell == SPELL.HEAL_FRIEND_CARD || cardModel.spell == SPELL.HEAL_FRIEND_CARDS
            || cardModel.spell == SPELL.HEAL_FRIEND_HERO)
        {
            backImage.gameObject.SetActive(true);
            iconImage.gameObject.SetActive(false);
            atBackImage.gameObject.SetActive(false);
            atText.gameObject.SetActive(false);
        }

        if(cardModel.spell == SPELL.DAMAGE_ENEMY_CARDS)
        {
            damageAllText.gameObject.SetActive(true);
        }
        else if (cardModel.spell == SPELL.DAMAGE_ENEMY_HERO)
        {
            damagePlayerText.gameObject.SetActive(true);
        }
        else if (cardModel.spell == SPELL.HEAL_FRIEND_CARDS)
        {
            healAllText.gameObject.SetActive(true);
        }
        else if (cardModel.spell == SPELL.HEAL_FRIEND_HERO)
        {
            healPlayerText.gameObject.SetActive(true);
        }
    }

    public void Show()
    {
        maskPanel.SetActive(false);
    }
    public void Refresh(CardModel cardModel)
    {
        hpText.text = cardModel.hp.ToString();
        atText.text = cardModel.at.ToString();
    }

    public void SetActiveSelectablePanel(bool flag)
    {
        selectablePanel.SetActive(flag);
    }
}