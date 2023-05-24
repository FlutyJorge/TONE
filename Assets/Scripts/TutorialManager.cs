using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameStarter gameSta;
    [SerializeField] TypeChanger typeChanger;
    [SerializeField] Type3Movement type3Mov;
    [SerializeField] OptionClick optionClick;
    [SerializeField] SceneChanger sceneChan;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] SpriteRenderer blackBoard;

    const int frontLayer = -1;
    [HideInInspector] public int paintToolUsed = 0;
    private bool isTutorialStarted = false;
    [HideInInspector] public bool isBoxClickedR = false;
    [HideInInspector] public bool isBoxClickedL = false;
    [HideInInspector] public bool readyToSwap = false;
    [HideInInspector] public bool isDropedPaintTool = false;

    [Space(10)]
    [Header("ハイライトされるオブジェクト")]
    [SerializeField] GameObject[] boxes = new GameObject[16];

    [Space(10)]
    [Header("ハイライトされるオブジェクトのレンダラー")]
    [SerializeField] SpriteRenderer[] playersRen = new SpriteRenderer[2];
    [SerializeField] Canvas sliderRen;
    [SerializeField] SpriteRenderer[] playPointsRen = new SpriteRenderer[16];
    [SerializeField] SpriteRenderer[] typeChangersRen = new SpriteRenderer[6];
    [SerializeField] SpriteRenderer[] countersRen = new SpriteRenderer[3];
    [SerializeField] SpriteRenderer[] paintToolsRen = new SpriteRenderer[6];
    [SerializeField] SpriteRenderer tryButtonRen;
    [SerializeField] SpriteRenderer pathRen;
    private SpriteRenderer[] boxesRen = new SpriteRenderer[16];

    [Space(10)]
    [Header("ハイライト用のレンダラー")]
    [SerializeField] SpriteRenderer description;
    [SerializeField] SpriteRenderer boxesHighlight;
    [SerializeField] SpriteRenderer boxes6Highlight;
    [SerializeField] SpriteRenderer boxes9Highlight;
    [SerializeField] SpriteRenderer boxHighlight;
    [SerializeField] SpriteRenderer playersHighlight;
    [SerializeField] SpriteRenderer typeChangersHighlight;
    [SerializeField] SpriteRenderer typeChangerHighlight;
    [SerializeField] SpriteRenderer paintToolsHighlight;
    [SerializeField] SpriteRenderer sliderHighlight;
    [SerializeField] SpriteRenderer tryButtonHighlight;

    [Space(10)]
    [Header("オブジェクトのコライダー")]
    [SerializeField] BoxCollider2D[] typeChangersCol = new BoxCollider2D[3];
    [SerializeField] CircleCollider2D[] paintToolsCol = new CircleCollider2D[6];
    [SerializeField] CircleCollider2D[] paintToolCheckersCol = new CircleCollider2D[16];
    private BoxCollider2D[] boxesCol = new BoxCollider2D[16];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < boxes.Length; ++i)
        {
            boxesRen[i] = boxes[i].GetComponent<SpriteRenderer>();
            boxesCol[i] = boxes[i].GetComponent<BoxCollider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSta.isGameStarterEnd && !isTutorialStarted)
        {
            StartCoroutine(PlayTutorial());
            isTutorialStarted = true;
        }
    }

    private IEnumerator PlayTutorial()
    {
        blackBoard.DOFade(1, 0.5f);
        description.DOFade(1, 0.5f);
        text.DOFade(1, 0.5f);

        //テキストが完全に表示され、次に左クリックされるまで処理待機
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("今から、ゲームの遊び方について一通り説明させていただきますね。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("まず、画面中央にある16個の青い音符マークに注目してください。"));

        //Boxesとpathのハイライトをオンに
        for (int i = 0; i < boxes.Length; ++i)
        {
            boxesRen[i].sortingOrder = -1;
        }
        pathRen.sortingOrder = -2;
        boxesHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("このピース達を下に敷かれている青い矢印に沿って、曲の流れ通りに並べる\nことがプレイヤーさんの目標です。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("いわば、音のパズルゲームといったところでしょうか？"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("各パズルピースには始めに聴いていただいた曲が16のフレーズに分割されて\n割り当てられています。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("どのピースにどのフレーズが割り当てられているのかは、ピースを右クリックして確かめることができますよ。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("Challenge\nいづれかのピースを右クリックする。 0/3"));

        yield return new WaitForSeconds(0.3f);

        //Boxesをクリックできるようにする
        foreach (BoxCollider2D boxCol in boxesCol)
        {
            boxCol.enabled = true;
        }

        const int threeChallenges = 3;
        for (int i = 1; i <= threeChallenges; ++i)
        {
            yield return new WaitUntil(() => isBoxClickedR);
            text.SetText("Challenge\nいづれかのピースを右クリックする。 " + i + " / 3");

            //WaitUntil後にフラグを再びオフにする時間を設ける
            isBoxClickedR = false;
            yield return new WaitForSeconds(0.1f);
        }

        //Boxesをクリックできないようにする
        foreach (BoxCollider2D boxCol in boxesCol)
        {
            boxCol.enabled = false;
        }

        StartCoroutine(SetText("確かめていただけましたか？"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("パズルを解いていくにあたって、何回も曲とピースに割り当てられたフレーズを聴き比べることになるでしょう。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("そんな時には、左側の再生、停止ボタンを使ってください。"));


        //Boxesのハイライトをオフに、Playersのハイライトをオンに
        boxesHighlight.color = new Color(1, 1, 1, 0);
        foreach (SpriteRenderer boxRen in boxesRen)
        {
            boxRen.sortingOrder = -5;
        }
        pathRen.sortingOrder = -6;

        foreach (SpriteRenderer playerRen in playersRen)
        {
            playerRen.sortingOrder = -2;
        }
        playersHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        //スライダーを見せるため、テキストの位置を上にずらす
        StartCoroutine(MoveAndSetText(-2.85f, -2.7f, "特定の位置から再生したい場合は、下の再生バーを使うと便利ですから覚えておいてくださいね。"));

        yield return new WaitForSeconds(0.3f);

        //Playersのハイライトをオフに、Sliderのハイライトをオンに
        playersHighlight.color = new Color(1, 1, 1, 0);
        foreach (SpriteRenderer playerRen in playersRen)
        {
            playerRen.sortingOrder = -5;
        }

        sliderRen.sortingOrder = -2;
        foreach (SpriteRenderer playPointRen in playPointsRen)
        {
            playPointRen.sortingOrder = -1;
        }
        sliderHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        //Sliderのハイライトをオフに
        sliderHighlight.color = new Color(1, 1, 1, 0);
        foreach (SpriteRenderer playPointRen in playPointsRen)
        {
            playPointRen.sortingOrder = -5;
        }
        sliderRen.sortingOrder = -6;

        StartCoroutine(MoveAndSetText(-4.35f, -4.2f, "では、次にピースの並べ替え方法についてご説明します。"));

        //TypeChangersのハイライトをオンに
        foreach (SpriteRenderer typeChangerRen in typeChangersRen)
        {
            typeChangerRen.sortingOrder = -2;
        }
        foreach (SpriteRenderer counterRen in countersRen)
        {
            counterRen.sortingOrder = -2;
        }
        typeChangersHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("並べ替えには、スワップモード、ローテーションモード、シンメトリーモードの3種類を使います。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("まず、スワップモードでは、丸で囲まれているピースと4方向に隣り合った\nピースを左クリックすることで、互いの位置を入れ替えることができます。"));

        //TypeChangersのハイライトをオフに、スワップモードのハイライトは維持
        typeChangersHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[1].sortingOrder = -5;
        typeChangersRen[2].sortingOrder = -5;
        countersRen[1].sortingOrder = -5;
        countersRen[2].sortingOrder = -5;
        typeChangerHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("Challenge\nピースを左クリックしてスワップさせる。 0/2"));

        //スワップモードのハイライトをオフに、Boxのハイライトをオンに
        typeChangerHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[0].sortingOrder = -5;
        typeChangersRen[3].sortingOrder = -5;
        countersRen[0].sortingOrder = -5;

        //Box9をクリック可能に
        boxesRen[9].sortingOrder = -2;
        boxesRen[15].sortingOrder = -2;
        boxesCol[9].enabled = true;
        boxHighlight.color = new Color(1, 1, 1, 1);
        readyToSwap = true;

        const int twoChallenges = 2;
        const float swapingTime = 1f;
        for (int counter = 1; counter <= twoChallenges; ++counter)
        {
            yield return new WaitUntil(() => isBoxClickedL);

            switch (counter)
            {
                case 1:
                    //Box9をクリックしたので、Box6をクリック可能にする
                    isBoxClickedL = false;
                    text.SetText("Challenge\nピースを左クリックしてスワップさせる。 " + counter + " / 2");
                    yield return new WaitForSeconds(swapingTime);
                    boxesRen[9].sortingOrder = -5;
                    boxesRen[6].sortingOrder = -2;
                    boxHighlight.transform.position = new Vector3(-2.25f, -2.25f, 0);
                    boxesCol[9].enabled = false;
                    boxesCol[6].enabled = true;
                    break;

                case 2:
                    //Box13をクリックしたので、ループを抜ける
                    isBoxClickedL = false;
                    text.SetText("Challenge\nピースを左クリックしてスワップさせる。 " + counter + " / 2");
                    yield return new WaitForSeconds(swapingTime);
                    boxesRen[6].sortingOrder = -5;
                    boxesRen[15].sortingOrder = -5;
                    boxesCol[6].enabled = false;
                    boxHighlight.color = new Color(1, 1, 1, 0);
                    break;
            }
        }

        StartCoroutine(SetText("次に、ローテーションモードについてご紹介しますね。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("その前に、スワップモードからローテーションモードに切り替えてみましょう。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("各モードへの切り替えはいつでも左側のボタンからできますよ。"));

        //TypeChangersをハイライトする
        foreach (SpriteRenderer typeChangerRen in typeChangersRen)
        {
            typeChangerRen.sortingOrder = -2;
        }
        foreach (SpriteRenderer counterRen in countersRen)
        {
            counterRen.sortingOrder = -2;
        }
        typeChangersHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("Challenge\nローテーションモードに切り替える"));

        //TypeChangerのハイライトをオフに、ローテーションモードのハイライトは維持
        typeChangersHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[0].sortingOrder = -5;
        typeChangersRen[2].sortingOrder = -5;
        typeChangersRen[3].sortingOrder = -5;
        typeChangersRen[5].sortingOrder = -5;
        typeChangersCol[1].enabled = true;
        countersRen[0].sortingOrder = -5;
        countersRen[2].sortingOrder = -5;
        typeChangerHighlight.transform.position = new Vector3(7.5f, 1.84f, 0);
        typeChangerHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => typeChanger.isTypeChanging);

        StartCoroutine(SetText("ローテーションモードでは、左クリックで選択したピースを基準に4つのピースを時計回りに回転させることができます。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("Challenge\nピースを左クリックしてローテーションさせる。 0/2"));

        //ローテーションモードのハイライトをオフに、Box9, 13, 6, 14のハイライトをオンに
        typeChangerHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[1].sortingOrder = -5;
        typeChangersRen[4].sortingOrder = -5;
        boxesRen[9].sortingOrder = -2;
        boxesRen[13].sortingOrder = -2;
        boxesRen[6].sortingOrder = -2;
        boxesRen[14].sortingOrder = -2;
        boxHighlight.transform.position = new Vector3(0.75f, -2.25f, 0);
        boxHighlight.color = new Color(1, 1, 1, 1);

        //Box14をクリック可能に
        boxesCol[14].enabled = true;

        for (int counter = 1; counter <= twoChallenges; ++counter)
        {
            yield return new WaitUntil(() => isBoxClickedL);

            switch (counter)
            {
                case 1:
                    //Box14をクリックしたので、ハイライトを0, 5, 6, 9に変更
                    isBoxClickedL = false;
                    text.SetText("Challenge\nピースを左クリックしてスワップさせる。 " + counter + " / 2");
                    yield return new WaitForSeconds(1f);
                    boxesRen[13].sortingOrder = -5;
                    boxesRen[14].sortingOrder = -5;
                    boxesRen[0].sortingOrder = -2;
                    boxesRen[5].sortingOrder = -2;
                    boxHighlight.transform.position = new Vector3(0.75f, -0.75f, 0);

                    //Box14をクリック不可に、Box9をクリック可能に
                    boxesCol[14].enabled = false;
                    boxesCol[9].enabled = true;
                    break;

                case 2:
                    //Box9をクリックしたので、ループを抜ける
                    isBoxClickedL = false;
                    text.SetText("Challenge\nピースを左クリックしてスワップさせる。 " + counter + " / 2");
                    yield return new WaitForSeconds(1f);
                    boxesRen[0].sortingOrder = -5;
                    boxesRen[5].sortingOrder = -5;
                    boxesRen[6].sortingOrder = -5;
                    boxesRen[9].sortingOrder = -5;
                    countersRen[1].sortingOrder = -5;
                    boxHighlight.color = new Color(1, 1, 1, 0);

                    //Box9をクリック不可に
                    boxesCol[9].enabled = false;
                    break;
            }
        }

        StartCoroutine(SetText("いい感じですね。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("最後に、シンメトリーモードについて紹介させていただきますが、少し特殊なのでがんばってくださいね。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("このモードではピースを動かすために、複数個のピースを範囲選択する必要があります。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("Challenge\nシンメトリーモードに切り替える。"));

        //シンメトリーモードのハイライトをオンに
        typeChangersRen[2].sortingOrder = -2;
        typeChangersRen[5].sortingOrder = -2;
        typeChangersCol[2].enabled = true;
        countersRen[2].sortingOrder = -2;
        typeChangerHighlight.transform.position = new Vector3(6.35f, -0.86f, 0);
        typeChangerHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => typeChanger.isTypeChanging);

        //シンメトリーモードのハイライトをオフに
        typeChangerHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[2].sortingOrder = -5;
        typeChangersRen[5].sortingOrder = -5;
        typeChangersCol[2].enabled = false;
        countersRen[2].sortingOrder = -5;

        StartCoroutine(SetText("Challenge\nピースをカーソルドラッグで範囲選択する。"));

        //Box10, 1, 2, 7, 6, 0, 8, 9, 5をハイライト&選択できるように
        boxesRen[10].sortingOrder = -2;
        boxesRen[1].sortingOrder = -2;
        boxesRen[2].sortingOrder = -2;
        boxesRen[7].sortingOrder = -2;
        boxesRen[6].sortingOrder = -2;
        boxesRen[0].sortingOrder = -2;
        boxesRen[8].sortingOrder = -2;
        boxesRen[9].sortingOrder = -2;
        boxesRen[5].sortingOrder = -2;
        boxes9Highlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => CheckSelectedBox(9, 10, 5));

        StartCoroutine(SetText("選択されたピース達が少し大きくなっているのがわかりますか？"));

        //左クリックをしてもBox選択状態が解除されないように
        type3Mov.canResetBoxSize = false;

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("この状態で範囲選択されたピースの一つを左クリックすると、そのピースと\n選択範囲の中で対象の位置にあるピースとを入れ替えることができます。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("Challenge\n左上のピースをクリックして右下のピースと入れ替える。"));

        //ハイライトをBoxes9からBoxへ
        boxes9Highlight.color = new Color(1, 1, 1, 0);
        boxHighlight.transform.position = new Vector3(-2.25f, 2.25f, 0);
        boxHighlight.size = new Vector3(1.1f, 1.1f, 1);
        boxHighlight.color = new Color(1, 1, 1, 1);

        //Box10をクリック可能に
        boxesCol[10].enabled = true;

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => isBoxClickedL);

        isBoxClickedL = false;

        //Boxのサイズを元に戻す
        type3Mov.canType3Swap = false;
        type3Mov.DeactivateAllBoxes();
        type3Mov.canResetBoxSize = true;

        yield return new WaitForSeconds(1f);

        StartCoroutine(SetText("無事に移動できましたね。"));

        //boxのハイライトをオフに
        boxHighlight.color = new Color(1, 1, 1, 0);
        boxesRen[10].sortingOrder = -5;
        boxesRen[1].sortingOrder = -5;
        boxesRen[2].sortingOrder = -5;
        boxesRen[7].sortingOrder = -5;
        boxesRen[6].sortingOrder = -5;
        boxesRen[0].sortingOrder = -5;
        boxesRen[8].sortingOrder = -5;
        boxesRen[9].sortingOrder = -5;
        boxesRen[5].sortingOrder = -5;
        boxesCol[10].enabled = false;

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("もう一度試してみましょう。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("Challenge\nピースをカーソルドラッグで範囲選択する。"));

        //Box5, 1, 2, 7, 6, 0をハイライト&選択可能に
        boxesRen[5].sortingOrder = -2;
        boxesRen[1].sortingOrder = -2;
        boxesRen[2].sortingOrder = -2;
        boxesRen[7].sortingOrder = -2;
        boxesRen[6].sortingOrder = -2;
        boxesRen[0].sortingOrder = -2;
        boxes6Highlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => CheckSelectedBox(6, 5, 0));

        StartCoroutine(SetText("Challenge\n左上のピースをクリックして右下のピースと入れ替える。"));

        type3Mov.canResetBoxSize = false;
        boxes6Highlight.color = new Color(1, 1, 1, 0);
        boxHighlight.color = new Color(1, 1, 1, 1);
        boxesCol[5].enabled = true;

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => isBoxClickedL);

        isBoxClickedL = false;

        //Boxのサイズを元に戻す
        type3Mov.canType3Swap = true;
        type3Mov.DeactivateAllBoxes();
        type3Mov.canResetBoxSize = true;

        yield return new WaitForSeconds(1f);

        StartCoroutine(SetText("3つの入れ替え方法は使いこなせそうでしょうか？"));

        boxHighlight.color = new Color(1, 1, 1, 0);
        boxesRen[5].sortingOrder = -5;
        boxesRen[1].sortingOrder = -5;
        boxesRen[2].sortingOrder = -5;
        boxesRen[7].sortingOrder = -5;
        boxesRen[6].sortingOrder = -5;
        boxesRen[0].sortingOrder = -5;
        boxesCol[5].enabled = false;

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("各モードに使用回数制限が設けられているステージもあるので注意してくださいね。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("次に、ピースを並び替えるにあたって便利な機能をご紹介しましょう。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("これらはペイントツールと呼ばれるもので、なんとピースの色を変えることができます。"));

        paintToolsHighlight.color = new Color(1, 1, 1, 1);
        foreach (SpriteRenderer paintToolRen in paintToolsRen)
        {
            paintToolRen.sortingOrder = 0;
        }

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("ペイントツールをクリックしたまま、変えたいピースの位置までドラッグして実際に色を変えてみましょう。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("Challenge\nペイントツールをピースにドラッグ&ドロップする。 0/3"));

        foreach (CircleCollider2D paintToolCol in paintToolsCol)
        {
            paintToolCol.enabled = true;
        }
        foreach (CircleCollider2D paintToolCheckerCol in paintToolCheckersCol)
        {
            paintToolCheckerCol.enabled = true;
        }

        //Boxesのハイライトをオンに
        for (int i = 0; i < boxes.Length; ++i)
        {
            boxesRen[i].sortingOrder = -2;
        }
        boxesHighlight.color = new Color(1, 1, 1, 1);

        for (int i = 1; i <= threeChallenges; ++i)
        {
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => isDropedPaintTool);
            isDropedPaintTool = false;
            text.SetText("Challenge\nペイントツールをピースにドラッグ & ドロップする。 " + i + "/3");
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(SetText("使用回数に制限はありませんので、ぜひ目印として有効活用してくださいね。"));

        foreach (CircleCollider2D paintToolCol in paintToolsCol)
        {
            paintToolCol.enabled = false;
        }
        foreach (CircleCollider2D paintToolCheckerCol in paintToolCheckersCol)
        {
            paintToolCheckerCol.enabled = false;
        }

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("ここまで、ゲームの進め方についてご説明しましたが、最後にクリア方法に\nついてご説明します。"));

        //BoxesとPaintToolsのハイライトをオフに
        boxesHighlight.color = new Color(1, 1, 1, 0);
        for (int i = 0; i < boxes.Length; ++i)
        {
            boxesRen[i].sortingOrder = -5;
        }

        paintToolsHighlight.color = new Color(1, 1, 1, 0);
        foreach (SpriteRenderer paintToolRen in paintToolsRen)
        {
            paintToolRen.sortingOrder = -5;
        }

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("クリア方法はいたってシンプル。ピースを曲の順番に並べ変え終えたら、このTRYボタンを押すだけです。"));

        //Tryのハイライトをオンに
        tryButtonRen.sortingOrder = -2;
        tryButtonHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("ただ、正しい順番のピースが並べ替えられていないとボタンを押してもクリアできませんので注意してくださいね。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("以上でゲーム説明は終了です。頑張ってくださいね！"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        sceneChan.ChangeToTitleScene();

        yield break;
    }

    private IEnumerator SetText(string textContent)
    {
        text.DOFade(0, 0.3f);
        yield return new WaitForSeconds(0.3f);
        text.SetText(textContent);
        text.DOFade(1, 0.3f);
    }

    private IEnumerator MoveAndSetText(float textPosY, float descriptionPosY, string textContent)
    {
        text.DOFade(0, 0.3f);
        description.DOFade(0, 0.3f);
        yield return new WaitForSeconds(0.3f);
        text.rectTransform.position = new Vector3(0, textPosY, 0);
        description.transform.position = new Vector3(0, descriptionPosY, 0);
        text.SetText(textContent);
        text.DOFade(1, 0.3f);
        description.DOFade(1, 0.3f);
    }

    //シンメトリーモードで範囲選択した際、意図された範囲選択が行われているか確認
    private bool CheckSelectedBox(int boxCount, int topLeftBox, int bottomRightBox)
    {
        if (type3Mov.selectedBoxes.Count != boxCount)
        {
            return false;
        }

        int counter = 0;

        for (int i = 0; i < type3Mov.selectedBoxes.Count; ++i)
        {
            if (type3Mov.selectedBoxes[i] == boxes[topLeftBox] || type3Mov.selectedBoxes[i] == boxes[bottomRightBox])
            {
                ++counter;
            }

            if (counter == 2)
            {
                return true;
            }
        }
        return false;
    }
}
