using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class OptionClick : MonoBehaviour
{
    [SerializeField] GameObject optionScene;
    [SerializeField] GameObject blackBoard;

    [Space(10)]
    [Header("�^�C�g���ȊO�ŃA�^�b�`���K�v�Ȃ���")]
    [SerializeField] GameStarter gameSta;
    [SerializeField] GameObject backTotitle;

    [Space(10)]
    [Header("�X�e�[�W�ŃA�^�b�`���K�v�Ȃ���")]
    [SerializeField] ClearManager clearMana;
    [SerializeField] CircleCollider2D homeButtonCol;
    [SerializeField] CircleCollider2D nextStageButtonCol;

    [Space(10)]
    [Header("�񐔐���������ꍇ�ɃA�^�b�`���K�v�Ȃ���")]
    [SerializeField] FailedManager failedMana;
    [SerializeField] CircleCollider2D homeButton1Col;
    [SerializeField] CircleCollider2D retryButtonCol;

    [Space(10)]
    [Header("�^�C�g���ŃA�^�b�`���K�v�Ȃ���")]
    [SerializeField] SpriteRenderer option0Ren;
    [SerializeField] SpriteRenderer option0BackgroundRen;
    [SerializeField] GameObject quitGame;

    [Space(10)]
    [Header("�`���[�g���A���ŃA�^�b�`���K�v�Ȃ���")]
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] SpriteRenderer description;

    [Space(10)]
    [Header("�V�[���̎��")]
    public bool isTitle = false;
    public bool isTutorial = false;
    public bool isStage = false;

    [HideInInspector] public bool isEnterOption = false;
    [HideInInspector] public bool isOptionAppeared = false;
    private SpriteRenderer blackBoardRen;
    private BoxCollider2D blackBoardCol;
    const int fadeTime = 1;
    const int moveTime = 1;
    const int center = -10;
    const int outOfCamera = 10;
    private bool isMoving = false;

    void Start()
    {
        blackBoardRen = blackBoard.GetComponent<SpriteRenderer>();
        blackBoardCol = blackBoard.GetComponent<BoxCollider2D>();
    }

    public void ClickOptionButton()
    {
        if (!isOptionAppeared && !isMoving)
        {
            isMoving = true;
            blackBoardRen.DOFade(1, fadeTime);
            blackBoardCol.enabled = true;

            optionScene.transform.DOMove(new Vector3(0, center, 0), moveTime).SetRelative(true).SetEase(Ease.InOutBack);

            if (isTutorial)
            {
                description.DOFade(0, fadeTime);
                text.DOFade(0, fadeTime);
            }

            if (isTitle)
            {
                int aboveNote = -1;
                option0Ren.sortingOrder = aboveNote;
                option0BackgroundRen.sortingOrder = aboveNote - 1;
                quitGame.transform.DOMove(new Vector3(0, center, 0), moveTime).SetRelative(true).SetEase(Ease.InOutBack);
            }
            else
            {
                backTotitle.transform.DOMove(new Vector3(0, center, 0), moveTime).SetRelative(true).SetEase(Ease.InOutBack);
            }

            //�N���A��ʂ��\������Ă���ꍇ�A�I�v�V������ʂ̏ォ��ł�RayCast���������邽�߁A�R���C�_�[���I�t�ɂ��邱�ƂŖh��
            SetClearSceneCollider(false);

            //�A�Ŗh�~
            StartCoroutine(StopContinuousClick(true));
        }
        else if (isOptionAppeared && !isMoving)
        {
            isMoving = true;
            optionScene.transform.DOMove(new Vector3(0, outOfCamera, 0), moveTime).SetRelative(true).SetEase(Ease.InOutBack);

            if (isTitle)
            {
                blackBoardRen.DOFade(0, fadeTime);
                blackBoardCol.enabled = false;
                quitGame.transform.DOMove(new Vector3(0, outOfCamera, 0), moveTime).SetRelative(true).SetEase(Ease.InOutBack);
            }
            else if (!isTitle)
            {
                backTotitle.transform.DOMove(new Vector3(0, outOfCamera, 0), moveTime).SetRelative(true).SetEase(Ease.InOutBack);
            }

            if (isStage && gameSta.isGameStarterEnd && !isClearOrFailed())
            {
                blackBoardRen.DOFade(0, fadeTime);
                blackBoardCol.enabled = false;
            }

            if (isTutorial && gameSta.isGameStarterEnd)
            {
                description.DOFade(1, fadeTime);
                text.DOFade(1, fadeTime);
            }

            SetClearSceneCollider(true);

            //�A�Ŗh�~
            StartCoroutine(StopContinuousClick(false));

            if (isTitle)
            {
                StartCoroutine(ChangeOptionOrderInlayer());
            }
        }
    }

    private bool isClearOrFailed()
    {
        if (clearMana.isClear)
        {
            return true;
        }

        if (failedMana != null && failedMana.isStartedShowFailedBoard)
        {
            return true;
        }

        return false;
    }

    private void SetClearSceneCollider(bool hasCollider)
    {
        if (clearMana != null && clearMana.isClear)
        {
            homeButtonCol.enabled = hasCollider;

            if (nextStageButtonCol != null)
            {
                nextStageButtonCol.enabled = hasCollider;
            }

            if (homeButton1Col != null)
            {
                homeButton1Col.enabled = hasCollider;
                retryButtonCol.enabled = hasCollider;
            }
        }
    }

    private IEnumerator StopContinuousClick(bool OptionAppearChange)
    {
        yield return new WaitForSeconds(moveTime);
        isOptionAppeared = OptionAppearChange;
        isMoving = false;
    }

    public void CheckEnterOption(bool isEnter)
    {
        isEnterOption = isEnter;
    }

    private IEnumerator ChangeOptionOrderInlayer()
    {
        yield return new WaitForSeconds(1f);

        //�^�C�g����ʂł�Note����ԑO�ɕ`�悳���悤�ɂ���
        int behindNote = -6;
        option0Ren.sortingOrder = behindNote;
        option0BackgroundRen.sortingOrder = behindNote - 1;

        yield break;
    }
}
