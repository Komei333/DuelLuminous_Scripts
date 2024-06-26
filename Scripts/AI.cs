using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 敵の行動関連

public class AI : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    public IEnumerator EnemyTurn()
    {
        Debug.Log("Enemyのターン");

        // フィールドのカードを攻撃可能にする
        CardController[] enemyFieldCardList = gameManager.enemyFieldTransform.GetComponentsInChildren<CardController>();
        gameManager.SettingCanAttackView(enemyFieldCardList, true);

        yield return new WaitForSeconds(1);


        /* フィールドにカードをだす */

        // 手札のカードを取得
        CardController[] handCardList = gameManager.enemyHandTransform.GetComponentsInChildren<CardController>();

        // コスト以下のカードがあれば、カードをフィールドに出し続ける
        // 条件：モンスターカードならコストのみ
        // 条件：スペルならコストと、使用可能かどうか（CanUseSpell）
        while (Array.Exists(handCardList, card => (card.model.cost <= gameManager.enemy.manaCost) 
        && (!card.IsSpell || (card.IsSpell && card.CanUseSpell()))))
        {
            CardController[] selectableHandCardList;

            // モンスターカードはフィールドに5枚まで
            if (enemyFieldCardList.Length >= 5)
            {
                // スペルカードのみを取得
                selectableHandCardList = Array.FindAll(handCardList, card => (card.model.cost <= gameManager.enemy.manaCost) 
                && (card.IsSpell && card.CanUseSpell()));

                if (selectableHandCardList.Length == 0 || selectableHandCardList[0].model.cost > gameManager.enemy.manaCost)
                {
                    break;
                }
            }
            else
            {
                // コスト以下のカードリストを取得
                selectableHandCardList = Array.FindAll(handCardList, card => (card.model.cost <= gameManager.enemy.manaCost) 
                && (!card.IsSpell || (card.IsSpell && card.CanUseSpell())));            
            }

            // フィールドに出すカードを選択
            CardController selectCard = selectableHandCardList[0];

            // カードを表にする
            selectCard.Show();

            // スペルカードなら使用する
            if (selectCard.IsSpell)
            {
                StartCoroutine(CastSpellOf(selectCard));
            }
            else
            {
                // カードを移動
                StartCoroutine(selectCard.movement.MoveToField(gameManager.enemyFieldTransform));
                selectCard.OnFiled();
            }

            yield return new WaitForSeconds(1);
            handCardList = gameManager.enemyHandTransform.GetComponentsInChildren<CardController>();

            // フィールドのカードを取得
            enemyFieldCardList = gameManager.enemyFieldTransform.GetComponentsInChildren<CardController>();
        }

        yield return new WaitForSeconds(1);


        /* 攻撃 */

        // フィールドのカードリストを取得
        CardController[] fieldCardList = gameManager.enemyFieldTransform.GetComponentsInChildren<CardController>();

        // 攻撃可能カードがあれば攻撃を繰り返す
        while (Array.Exists(fieldCardList, card => card.model.canAttack))
        {
            // 攻撃可能カードを取得
            CardController[] enemyCanAttackCardList = Array.FindAll(fieldCardList, card => card.model.canAttack); // 検索：Array.FindAll
            CardController[] playerFieldCardList = gameManager.playerFieldTransform.GetComponentsInChildren<CardController>();

            // attackerカードを選択
            CardController attacker = enemyCanAttackCardList[0];

            // defenderカードを選択
            // シールドカードのみ攻撃対象にする
            if (Array.Exists(playerFieldCardList, card => card.model.ability == ABILITY.SHIELD))
            {
                playerFieldCardList = Array.FindAll(playerFieldCardList, card => card.model.ability == ABILITY.SHIELD);
                CardController defender = playerFieldCardList[0];

                // attackerとdefenderを戦わせる
                StartCoroutine(attacker.movement.MoveToTarget(defender.transform));
                yield return new WaitForSeconds(0.51f);
                gameManager.CardsBattle(attacker, defender);
            }
            else if (playerFieldCardList.Length > 0)
            {
                // シールドカードがない場合は攻撃対象をランダムに選ぶ
                var attackRandom = new System.Random();

                if (attackRandom.Next(0, 2) % 2 == 0)
                {
                    CardController defender = playerFieldCardList[0];

                    // attackerとdefenderを戦わせる
                    StartCoroutine(attacker.movement.MoveToTarget(defender.transform));
                    yield return new WaitForSeconds(0.51f);
                    gameManager.CardsBattle(attacker, defender);
                }
                else
                {
                    StartCoroutine(attacker.movement.MoveToTarget(gameManager.playerHero));
                    yield return new WaitForSeconds(0.25f);
                    gameManager.AttackToHero(attacker);
                    yield return new WaitForSeconds(0.25f);
                    gameManager.CheckHeroHP();
                }
            }
            else
            {
                // カードがない場合は相手プレイヤーを攻撃
                StartCoroutine(attacker.movement.MoveToTarget(gameManager.playerHero));
                yield return new WaitForSeconds(0.25f);
                gameManager.AttackToHero(attacker);
                yield return new WaitForSeconds(0.25f);
                gameManager.CheckHeroHP();
            }

            fieldCardList = gameManager.enemyFieldTransform.GetComponentsInChildren<CardController>();
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);
        gameManager.ChangeTurn();
    }

    IEnumerator CastSpellOf(CardController card)
    {
        CardController target = null;
        Transform movePosition = null;

        switch (card.model.spell)
        {
            case SPELL.DAMAGE_ENEMY_CARD:
                target = gameManager.GetEnemyFieldCards(card.model.isPlayerCard)[0];
                movePosition = target.transform;
                break;
            case SPELL.HEAL_FRIEND_CARD:
                target = gameManager.GetFriendFieldCards(card.model.isPlayerCard)[0];
                movePosition = target.transform;
                break;
            case SPELL.DAMAGE_ENEMY_CARDS:
                movePosition = gameManager.playerFieldTransform;
                break;
            case SPELL.HEAL_FRIEND_CARDS:
                movePosition = gameManager.enemyFieldTransform;
                break;
            case SPELL.DAMAGE_ENEMY_HERO:
                movePosition = gameManager.playerHero;
                break;
            case SPELL.HEAL_FRIEND_HERO:
                movePosition = gameManager.enemyHero;
                break;
        }

        // 移動先としてターゲット、それぞれのフィールド、それぞれのHeroのTransformが必要
        StartCoroutine(card.movement.MoveToField(movePosition));
        yield return new WaitForSeconds(0.25f);
        card.UseSpellTo(target); // カードを使用したら破壊する
    }
}