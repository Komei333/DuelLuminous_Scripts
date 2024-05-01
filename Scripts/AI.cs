using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �G�̍s���֘A

public class AI : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    public IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy�̃^�[��");

        // �t�B�[���h�̃J�[�h���U���\�ɂ���
        CardController[] enemyFieldCardList = gameManager.enemyFieldTransform.GetComponentsInChildren<CardController>();
        gameManager.SettingCanAttackView(enemyFieldCardList, true);

        yield return new WaitForSeconds(1);


        /* �t�B�[���h�ɃJ�[�h������ */

        // ��D�̃J�[�h���擾
        CardController[] handCardList = gameManager.enemyHandTransform.GetComponentsInChildren<CardController>();

        // �R�X�g�ȉ��̃J�[�h������΁A�J�[�h���t�B�[���h�ɏo��������
        // �����F�����X�^�[�J�[�h�Ȃ�R�X�g�̂�
        // �����F�X�y���Ȃ�R�X�g�ƁA�g�p�\���ǂ����iCanUseSpell�j
        while (Array.Exists(handCardList, card => (card.model.cost <= gameManager.enemy.manaCost) 
        && (!card.IsSpell || (card.IsSpell && card.CanUseSpell()))))
        {
            CardController[] selectableHandCardList;

            // �����X�^�[�J�[�h�̓t�B�[���h��5���܂�
            if (enemyFieldCardList.Length >= 5)
            {
                // �X�y���J�[�h�݂̂��擾
                selectableHandCardList = Array.FindAll(handCardList, card => (card.model.cost <= gameManager.enemy.manaCost) 
                && (card.IsSpell && card.CanUseSpell()));

                if (selectableHandCardList.Length == 0 || selectableHandCardList[0].model.cost > gameManager.enemy.manaCost)
                {
                    break;
                }
            }
            else
            {
                // �R�X�g�ȉ��̃J�[�h���X�g���擾
                selectableHandCardList = Array.FindAll(handCardList, card => (card.model.cost <= gameManager.enemy.manaCost) 
                && (!card.IsSpell || (card.IsSpell && card.CanUseSpell())));            
            }

            // �t�B�[���h�ɏo���J�[�h��I��
            CardController selectCard = selectableHandCardList[0];

            // �J�[�h��\�ɂ���
            selectCard.Show();

            // �X�y���J�[�h�Ȃ�g�p����
            if (selectCard.IsSpell)
            {
                StartCoroutine(CastSpellOf(selectCard));
            }
            else
            {
                // �J�[�h���ړ�
                StartCoroutine(selectCard.movement.MoveToField(gameManager.enemyFieldTransform));
                selectCard.OnFiled();
            }

            yield return new WaitForSeconds(1);
            handCardList = gameManager.enemyHandTransform.GetComponentsInChildren<CardController>();

            // �t�B�[���h�̃J�[�h���擾
            enemyFieldCardList = gameManager.enemyFieldTransform.GetComponentsInChildren<CardController>();
        }

        yield return new WaitForSeconds(1);


        /* �U�� */

        // �t�B�[���h�̃J�[�h���X�g���擾
        CardController[] fieldCardList = gameManager.enemyFieldTransform.GetComponentsInChildren<CardController>();

        // �U���\�J�[�h������΍U�����J��Ԃ�
        while (Array.Exists(fieldCardList, card => card.model.canAttack))
        {
            // �U���\�J�[�h���擾
            CardController[] enemyCanAttackCardList = Array.FindAll(fieldCardList, card => card.model.canAttack); // �����FArray.FindAll
            CardController[] playerFieldCardList = gameManager.playerFieldTransform.GetComponentsInChildren<CardController>();

            // attacker�J�[�h��I��
            CardController attacker = enemyCanAttackCardList[0];

            // defender�J�[�h��I��
            // �V�[���h�J�[�h�̂ݍU���Ώۂɂ���
            if (Array.Exists(playerFieldCardList, card => card.model.ability == ABILITY.SHIELD))
            {
                playerFieldCardList = Array.FindAll(playerFieldCardList, card => card.model.ability == ABILITY.SHIELD);
                CardController defender = playerFieldCardList[0];

                // attacker��defender���킹��
                StartCoroutine(attacker.movement.MoveToTarget(defender.transform));
                yield return new WaitForSeconds(0.51f);
                gameManager.CardsBattle(attacker, defender);
            }
            else if (playerFieldCardList.Length > 0)
            {
                // �V�[���h�J�[�h���Ȃ��ꍇ�͍U���Ώۂ������_���ɑI��
                var attackRandom = new System.Random();

                if (attackRandom.Next(0, 2) % 2 == 0)
                {
                    CardController defender = playerFieldCardList[0];

                    // attacker��defender���킹��
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
                // �J�[�h���Ȃ��ꍇ�͑���v���C���[���U��
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

        // �ړ���Ƃ��ă^�[�Q�b�g�A���ꂼ��̃t�B�[���h�A���ꂼ���Hero��Transform���K�v
        StartCoroutine(card.movement.MoveToField(movePosition));
        yield return new WaitForSeconds(0.25f);
        card.UseSpellTo(target); // �J�[�h���g�p������j�󂷂�
    }
}