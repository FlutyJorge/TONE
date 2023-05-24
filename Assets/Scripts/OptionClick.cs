using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class OptionClick : MonoBehaviour
{
    [SerializeField] GameObject optionScene;
    [SerializeField] GameObject blackBoard;
    private SpriteRenderer blackBoardRen;
    private BoxCollider2D blackBoardCol;

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
    [SerializeField] CircleCollider2D homeButton1Col;
    [SerializeField] CircleCollider2D retryButtonCol;

    [Space(10)]
    [Header("タイトルでアタッチが必要なもの")]
    [SerializeField] SpriteRenderer option0Ren;
    [SerializeField] SpriteRenderer option0BackgroundRen;

    [Space(10)]
    [Header("チュートリアルでアタッチが必要なもの")]
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] SpriteRenderer description;

    [Space(10)]
    [Header("シーンの種類")]
    public bool isTitle = false;
    public bool isTutorial = false;

    [HideInInspector] public bool isEnterOption = false;
    [HideInInspector] public bool isOptionAppeared = false;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        blackBoardRen = blackBoard.GetComponent<SpriteRenderer>();
        blackBoardCol = blackBoard.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickOptionButton()
    {
        if (!isOptionAppeared && !isMoving)
        {
            isMoving = true;
            blackBoardRen.DOFade(1, 1f);
            blackBoardCol.enabled = true;
            optionScene.transform.DOMove(new Vector3(0, -10, 0), 1f).SetRelative(true).SetEase(Ease.InOutBack);

            if (isTutorial)
            {
                description.DOFade(0, 1f);
                text.DOFade(0, 1f);
            }

            if (isTitle)
            {
                option0Ren.sortingOrder = -1;
                option0BackgroundRen.sortingOrder = -2;
            }
            else if (!isTitle)
            {
                backTotitle.transform.DOMove(new Vector3(0, -10, 0), 1f).SetRelative(true).SetEase(Ease.InOutBack);
            }

            if (clearMana != null && clearMana.isClear)
            {
                homeButtonCol.enabled = false;
                nextStageButtonCol.enabled = false;

                if (homeButton1Col != null)
                {
                    homeButton1Col.enabled = false;
                    retryButtonCol.enabled = false;
                }
            }

            //連打防止
            StartCoroutine(StopContinuousClick(true));
        }
        else if (isOptionAppeared && !isMoving)
        {
            isMoving = true;

            //タイトル画面の場合
            if (isTitle)
            {
                blackBoardRen.DOFade(0, 1f);
                blackBoardCol.enabled = false;
                optionScene.transform.DOMove(new Vector3(0, 10, 0), 1f).SetRelative(true).SetEase(Ease.InOutBack);
            }
            else if (!isTitle)
            {
                backTotitle.transform.DOMove(new Vector3(0, 10, 0), 1f).SetRelative(true).SetEase(Ease.InOutBack);
            }

            //ステージの場合
            if (!isTutorial && !isTitle && gameSta.isGameStarterEnd && !clearMana.isClear)
            {
                blackBoardRen.DOFade(0, 1f);
                blackBoardCol.enabled = false;
            }
            optionScene.transform.DOMove(new Vector3(0, 10, 0), 1f).SetRelative(true).SetEase(Ease.InOutBack);

            //タイトルの場合
            if (isTutorial && gameSta.isGameStarterEnd)
            {
                description.DOFade(1, 1f);
                text.DOFade(1, 1f);
            }

            if (clearMana != null && clearMana.isClear)
            {
                homeButtonCol.enabled = true;
                nextStageButtonCol.enabled = true;

                if (homeButton1Col != null)
                {
                    homeButton1Col.enabled = true;
                    retryButtonCol.enabled = true;
                }
            }

            //連打防止
            StartCoroutine(StopContinuousClick(false));

            if (isTitle)
            {
                StartCoroutine(ChangeOptionOrderInlayer());
            }
        }
    }

    private IEnumerator StopContinuousClick(bool OptionAppearChange)
    {
        yield return new WaitForSeconds(1f);
        isOptionAppeared = OptionAppearChange;
        isMoving = false;
    }

    public void CheckEnterOption(bool trueOrFalse)
    {
        isEnterOption = trueOrFalse;
    }

    //タイトル画面の場合、blackBoardが消えたらOptionのレイヤー順を変更する
    private IEnumerator ChangeOptionOrderInlayer()
    {
        yield return new WaitForSeconds(1f);
        option0Ren.sortingOrder = -6;
        option0BackgroundRen.sortingOrder = -7;

        yield break;
    }
}
