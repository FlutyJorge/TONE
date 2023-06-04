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
    [Header("タイトル以外でアタッチが必要なもの")]
    [SerializeField] GameStarter gameSta;
    [SerializeField] GameObject backTotitle;

    [Space(10)]
    [Header("ステージでアタッチが必要なもの")]
    [SerializeField] ClearManager clearMana;
    [SerializeField] CircleCollider2D homeButtonCol;
    [SerializeField] CircleCollider2D nextStageButtonCol;

    [Space(10)]
    [Header("回数制限がある場合にアタッチが必要なもの")]
    [SerializeField] FailedManager failedMana;
    [SerializeField] CircleCollider2D homeButton1Col;
    [SerializeField] CircleCollider2D retryButtonCol;

    [Space(10)]
    [Header("タイトルでアタッチが必要なもの")]
    [SerializeField] SpriteRenderer option0Ren;
    [SerializeField] SpriteRenderer option0BackgroundRen;
    [SerializeField] GameObject quitGame;

    [Space(10)]
    [Header("チュートリアルでアタッチが必要なもの")]
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] SpriteRenderer description;

    [Space(10)]
    [Header("シーンの種類")]
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

            //クリア画面が表示されている場合、オプション画面の上からでもRayCastが反応するため、コライダーをオフにすることで防ぐ
            SetClearSceneCollider(false);

            //連打防止
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

            //連打防止
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

        //タイトル画面ではNoteが一番前に描画されるようにする
        int behindNote = -6;
        option0Ren.sortingOrder = behindNote;
        option0BackgroundRen.sortingOrder = behindNote - 1;

        yield break;
    }
}
