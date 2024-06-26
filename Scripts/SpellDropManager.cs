using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// スペルで攻撃される側の処理

public class SpellDropManager : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        CardController spellCard = eventData.pointerDrag.GetComponent<CardController>();
        CardController target = GetComponent<CardController>();

        if (spellCard == null)
        {
            return;
        }
        if (spellCard.CanUseSpell())
        {
            spellCard.UseSpellTo(target);
        }
    }
}