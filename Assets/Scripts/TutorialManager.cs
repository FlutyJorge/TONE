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
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] SpriteRenderer blackBoard;
    const int frontLayer = -1;
    [HideInInspector] public int paintToolUsed = 0;
    [HideInInspector] public bool isDropedPaintTool = false;

    private bool isTutorialStart = false;
    [HideInInspector] public bool isBoxClickedR = false;
    [HideInInspector] public bool isBoxClickedL = false;
    [HideInInspector] public bool readyToSwap = false;

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
    private BoxCollider2D[] boxesCol = new BoxCollider2D[16];


    //オブジェクトのEventTriggerManager
    private EventTriggerManager[] boxesEveTrigger = new EventTriggerManager[16];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < boxes.Length; ++i)
        {
            boxesRen[i] = boxes[i].GetComponent<SpriteRenderer>();
            boxesCol[i] = boxes[i].GetComponent<BoxCollider2D>();
            boxesEveTrigger[i] = boxes[i].GetComponent<EventTriggerManager>();
        }
        StartCoroutine(PlayTutorial());
    }

    // Update is called once per frame
    void Update()
    {
        /*if (gameSta.isGameStarterEnd && !isTutorialStart)
        {

        }*/
    }

    private IEnumerator PlayTutorial()
    {
        blackBoard.DOFade(1, 0.5f);
        description.DOFade(1, 0.5f);
        text.DOFade(1, 0.5f);

        //テキストが完全に表示され、次に左クリックされるまで処理待機
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("今から、ゲームの遊び方について一通り説明させていただきます。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("まず、画面中央に16個の青い音符マークに注目してください。"));

        //Boxesをハイライトする
        for (int i = 0; i < boxes.Length; ++i)
        {
            boxesRen[i].sortingOrder = -1;
        }
        boxesHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("各パズルピースには先ほど聴いていただいた曲が16のフレーズに分割されて\n収納されています。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("どのピースにどのフレーズが収納されているのかは、ピースを右クリックして確かめることができますよ。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("Challenge\nいづれかのピースを右クリックする。 0/3"));

        yield return new WaitForSeconds(0.3f);

        //Boxesをクリックできるようにする
        foreach (BoxCollider2D boxCol in boxesCol)
        {
            boxCol.enabled = true;
        }

        const int challengeNum = 3;
        for (int i = 1; i <= challengeNum; ++i)
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
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("このピース達を下に敷かれている青い矢印に沿って、曲の流れ通りに並べることがプレイヤーさんの目標です。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("曲を確認するには左側の再生、停止ボタンを使ってください。"));


        //ハイライトをBoxesからPlayersに変更
        boxesHighlight.color = new Color(1, 1, 1, 0);
        foreach (SpriteRenderer boxRen in boxesRen)
        {
            boxRen.sortingOrder = -5;
        }

        foreach (SpriteRenderer playerRen in playersRen)
        {
            playerRen.sortingOrder = -1;
        }
        playersHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(MoveAndSetText(-2.85f, -2.7f, "特定の位置から再生したい場合は、下の再生バーを使うと便利ですから覚えておいてくださいね。"));

        yield return new WaitForSeconds(0.3f);

        //ハイライトをPlayersからSliderに変更
        playersHighlight.color = new Color(1, 1, 1, 0);
        foreach (SpriteRenderer playerRen in playersRen)
        {
            playerRen.sortingOrder = -6;
        }

        sliderRen.sortingOrder = -1;
        foreach (SpriteRenderer playPointRen in playPointsRen)
        {
            playPointRen.sortingOrder = 0;
        }
        sliderHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        //Sliderのハイライトを消す
        sliderHighlight.color = new Color(1, 1, 1, 0);
        foreach (SpriteRenderer playPointRen in playPointsRen)
        {
            playPointRen.sortingOrder = -5;
        }
        sliderRen.sortingOrder = -5;

        StartCoroutine(MoveAndSetText(-4.35f, -4.2f, "では、次にピースの並べ替え方法についてご説明します。"));

        //TypeChangersをハイライトする
        foreach (SpriteRenderer typeChangerRen in typeChangersRen)
        {
            typeChangerRen.sortingOrder = -1;
        }
        foreach (SpriteRenderer counterRen in countersRen)
        {
            counterRen.sortingOrder = -1;
        }
        typeChangersHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("並べ替えには、スワップモード、ローテーションモード、シンメトリーモードの3種類を使います。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("まず、スワップモードでは、丸に囲まれているピースと4方向に隣り合ったピースを左クリックすることで、互いの位置を入れ替えることができますよ。"));

        //スワップモードをハイライト
        typeChangersHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[1].sortingOrder = -6;
        typeChangersRen[2].sortingOrder = -6;
        countersRen[1].sortingOrder = -4;
        countersRen[2].sortingOrder = -4;
        typeChangerHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("Challenge\nピースを左クリックしてスワップさせる 0/3"));

        //ハイライトをスワップモードからBoxに変更しBox14をスワップできるように
        typeChangerHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[0].sortingOrder = -6;
        typeChangersRen[3].sortingOrder = -5;
        countersRen[0].sortingOrder = -6;

        boxesRen[14].sortingOrder = -1;
        boxesRen[15].sortingOrder = -1;
        boxesCol[14].enabled = true;
        boxHighlight.color = new Color(1, 1, 1, 1);
        readyToSwap = true;

        const float swapingTime = 1f;
        for (int counter = 1; counter <= challengeNum; ++counter)
        {
            yield return new WaitUntil(() => isBoxClickedL);

            switch (counter)
            {
                case 1:
                    //Box14をクリックし、Box9をクリック可能にする
                    isBoxClickedL = false;
                    text.SetText("Challenge\nピースを左クリックしてスワップさせる " + counter + " / 3");
                    yield return new WaitForSeconds(swapingTime);
                    boxesRen[14].sortingOrder = -5;
                    boxesRen[9].sortingOrder = -1;
                    boxHighlight.transform.position = new Vector3(-0.75f, -0.75f, 0);
                    boxesCol[14].enabled = false;
                    boxesCol[9].enabled = true;
                    break;

                case 2:
                    //Box9をクリックし、Box10をクリック可能にする
                    isBoxClickedL = false;
                    text.SetText("Challenge\nピースを左クリックしてスワップさせる " + counter + " / 3");
                    yield return new WaitForSeconds(swapingTime);
                    boxesRen[9].sortingOrder = -5;
                    boxesRen[10].sortingOrder = -1;
                    boxHighlight.transform.position = new Vector3(0.75f, -0.75f, 0);
                    boxesCol[9].enabled = false;
                    boxesCol[10].enabled = true;
                    break;

                case 3:
                    //Box10をクリックし、ループを抜ける
                    isBoxClickedL = false;
                    text.SetText("Challenge\nピースを左クリックしてスワップさせる " + counter + " / 3");
                    yield return new WaitForSeconds(swapingTime);
                    boxesRen[10].sortingOrder = -5;
                    boxesRen[15].sortingOrder = -5;
                    boxesCol[10].enabled = false;
                    boxHighlight.color = new Color(1, 1, 1, 0);
                    break;
            }
        }

        StartCoroutine(SetText("次に、ローテーションモードについてご紹介しますね。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("その前に、スワップモードからローテーションモードに切り替えてみましょう。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("各モードへの切り替えはいつでも左側のボタンからできますよ。"));

        //TypeChangersをハイライトする
        foreach (SpriteRenderer typeChangerRen in typeChangersRen)
        {
            typeChangerRen.sortingOrder = -1;
        }
        foreach (SpriteRenderer counterRen in countersRen)
        {
            counterRen.sortingOrder = -1;
        }
        typeChangersHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("Challenge\nローテーションモードに切り替える"));

        //ハイライトをTypeChangersからローテーションモードへ
        typeChangersHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[0].sortingOrder = -6;
        typeChangersRen[2].sortingOrder = -6;
        typeChangersRen[3].sortingOrder = -5;
        typeChangersRen[5].sortingOrder = -5;
        typeChangersCol[1].enabled = true;
        countersRen[0].sortingOrder = -4;
        countersRen[2].sortingOrder = -4;
        typeChangerHighlight.transform.position = new Vector3(7.5f, 1.84f, 0);
        typeChangerHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => typeChanger.isTypeChanging);

        StartCoroutine(SetText("ローテーションモードでは、左クリックで選択したピースを基準に4つのピースを時計回りに回転させることができるんです。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("Challenge\nピースを左クリックしてローテーションさせる。"));

        //ハイライトをローテーションモードからBoxに変更しBox11をスワップできるように
        typeChangerHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[1].sortingOrder = -6;
        typeChangersRen[4].sortingOrder = -5;
        boxesRen[15].sortingOrder = -1;
        boxesRen[5].sortingOrder = -1;
        boxesRen[4].sortingOrder = -1;
        boxesRen[11].sortingOrder = -1;
        boxesCol[11].enabled = true;
        boxHighlight.transform.position = new Vector3(2.25f, -0.75f, 0);
        boxHighlight.color = new Color(1, 1, 1, 1);

        for (int counter = 1; counter < challengeNum; ++counter)
        {
            yield return new WaitUntil(() => isBoxClickedL);

            switch (counter)
            {
                //Box11をクリックし、Box15をクリックできるように。1, 2, 6, 15
                case 1:
                    isBoxClickedL = false;
                    text.SetText("Challenge\nピースを左クリックしてスワップさせる " + counter + " / 2");
                    yield return new WaitForSeconds(1f);
                    boxesRen[5].sortingOrder = -5;
                    boxesRen[4].sortingOrder = -5;
                    boxesRen[11].sortingOrder = -5;
                    boxesCol[11].enabled = false;
                    boxesRen[1].sortingOrder = -1;
                    boxesRen[2].sortingOrder = -1;
                    boxesRen[6].sortingOrder = -1;
                    boxesCol[15].enabled = true;
                    boxHighlight.transform.position = new Vector3(0.75f, 0.75f, 0);
                    break;

                //Box15をクリックし、ループを抜ける

                case 2:
                    isBoxClickedL = false;
                    text.SetText("Challenge\nピースを左クリックしてスワップさせる " + counter + " / 2");
                    yield return new WaitForSeconds(1f);
                    boxesRen[1].sortingOrder = -5;
                    boxesRen[2].sortingOrder = -5;
                    boxesRen[6].sortingOrder = -5;
                    boxesRen[15].sortingOrder = -5;
                    boxesCol[15].enabled = false;
                    countersRen[1].sortingOrder = -4;
                    boxHighlight.color = new Color(1, 1, 1, 0);
                    break;
            }
        }

        StartCoroutine(SetText("いい感じですね。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("最後に、シンメトリーモードについて紹介させていただきますが、少し特殊なのでがんばってくださいね。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("このモードではピースを動かすために、複数個のピースを範囲選択する必要があります。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("Challenge\nシンメトリーモードに切り替える"));

        //シンメトリーモードをハイライト
        typeChangersRen[2].sortingOrder = -1;
        typeChangersRen[5].sortingOrder = -1;
        typeChangersCol[2].enabled = true;
        countersRen[2].sortingOrder = -1;
        typeChangerHighlight.transform.position = new Vector3(6.35f, -0.86f, 0);
        typeChangerHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => typeChanger.isTypeChanging);

        //シンメトリーモードのハイライトをオフに
        typeChangerHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[2].sortingOrder = -6;
        typeChangersRen[5].sortingOrder = -5;
        typeChangersCol[2].enabled = false;
        countersRen[2].sortingOrder = -4;

        StartCoroutine(SetText("Challenge\nピースをカーソルドラッグで範囲選択する。"));

        //Boxes9をハイライト&Box0, 6, 1, 7, 15, 2, 8, 10, 11を選択できるように
        boxesRen[0].sortingOrder = -1;
        boxesRen[6].sortingOrder = -1;
        boxesRen[1].sortingOrder = -1;
        boxesRen[7].sortingOrder = -1;
        boxesRen[15].sortingOrder = -1;
        boxesRen[2].sortingOrder = -1;
        boxesRen[8].sortingOrder = -1;
        boxesRen[10].sortingOrder = -1;
        boxesRen[11].sortingOrder = -1;
        boxes9Highlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => CheckSelectedBox(9, 0, 11));

        StartCoroutine(SetText("選択されたピース達が少し大きくなっているのがわかりますか？"));
        //左クリックをしてもBox選択状態が解除されないように
        type3Mov.canResetBoxSize = false;

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("この状態で範囲選択されたピースの一つを左クリックすると、そのピースと選択範囲の中で対象の位置にあるピースとを入れ替えることができます。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("Challenge\n左上のピースをクリックして右下のピースと入れ替える"));

        //ハイライトをBoxes9からBoxへ
        boxes9Highlight.color = new Color(1, 1, 1, 0);

        boxHighlight.transform.position = new Vector3(-2.25f, 2.25f, 0);
        boxHighlight.size = new Vector3(1.1f, 1.1f, 1);
        boxHighlight.color = new Color(1, 1, 1, 1);
        boxesCol[0].enabled = true;

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => isBoxClickedL);

        isBoxClickedL = false;

        //Boxのサイズを元に戻す
        type3Mov.canType3Swap = false;
        type3Mov.DeactivateAllBoxes();
        type3Mov.canResetBoxSize = true;

        yield return new WaitForSeconds(1f);

        StartCoroutine(SetText("無事に移動できましたね。"));

        boxHighlight.color = new Color(1, 1, 1, 0);
        boxesRen[0].sortingOrder = -5;
        boxesRen[6].sortingOrder = -5;
        boxesRen[1].sortingOrder = -5;
        boxesRen[7].sortingOrder = -5;
        boxesRen[15].sortingOrder = -5;
        boxesRen[2].sortingOrder = -5;
        boxesRen[8].sortingOrder = -5;
        boxesRen[10].sortingOrder = -5;
        boxesRen[11].sortingOrder = -5;
        boxesCol[0].enabled = false;

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("もう一度試してみましょう"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("Challenge\nピースをカーソルドラッグで範囲選択する"));

        //Box0, 6, 1, 7, 15, 2, 8, 10, 11をハイライトする
        boxesRen[6].sortingOrder = -1;
        boxesRen[1].sortingOrder = -1;
        boxesRen[7].sortingOrder = -1;
        boxesRen[15].sortingOrder = -1;
        boxesRen[2].sortingOrder = -1;
        boxesRen[11].sortingOrder = -1;
        boxes6Highlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => CheckSelectedBox(6, 11, 2));

        StartCoroutine(SetText("Challenge\n左上のピースをクリックして右下のピースと入れ替える"));

        type3Mov.canResetBoxSize = false;
        boxes6Highlight.color = new Color(1, 1, 1, 0);
        boxHighlight.color = new Color(1, 1, 1, 1);
        boxesCol[11].enabled = true;

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
        boxesRen[6].sortingOrder = -5;
        boxesRen[1].sortingOrder = -5;
        boxesRen[7].sortingOrder = -5;
        boxesRen[15].sortingOrder = -5;
        boxesRen[2].sortingOrder = -5;
        boxesRen[11].sortingOrder = -5;
        boxesCol[11].enabled = false;

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("ここで、ピースを並び替えるにあたって便利な機能をご紹介しましょう。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("これらはペイントツールと呼ばれるもので、ピースの色を変えることができます。"));

        paintToolsHighlight.color = new Color(1, 1, 1, 1);
        foreach (SpriteRenderer paintToolRen in paintToolsRen)
        {
            paintToolRen.sortingOrder = -1;
        }

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("ペイントツールをクリックしたまま、変えたいピースの位置までドラッグして実際に色を変えてみましょう。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("Challenge\nペイントツールをピースにドラッグ&ドロップする。 0/3"));

        foreach (CircleCollider2D paintToolCol in paintToolsCol)
        {
            paintToolCol.enabled = true;
        }

        //Boxesのハイライトをオンにする
        for (int i = 0; i < boxes.Length; ++i)
        {
            boxesRen[i].sortingOrder = -1;
        }
        boxesHighlight.color = new Color(1, 1, 1, 1);

        for (int i = 1; i <= 3; ++i)
        {
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => isDropedPaintTool);
            isDropedPaintTool = false;
            text.SetText("Challenge\nペイントツールをピースにドラッグ & ドロップする。 " + i + "/3");
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(SetText("使用回数に制限はありませんので、ぜひ目印として有効活用してください。"));

        foreach (CircleCollider2D paintToolCol in paintToolsCol)
        {
            paintToolCol.enabled = false;
        }

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("ここまで、ゲームの進め方についてご説明しましたが、最後にクリア方法についてご説明します。"));

        //BoxesとPaintToolsのハイライトをオフにする
        boxesHighlight.color = new Color(1, 1, 1, 0);
        for (int i = 0; i < boxes.Length; ++i)
        {
            boxesRen[i].sortingOrder = -5;
        }

        paintToolsHighlight.color = new Color(1, 1, 1, 0);
        foreach (SpriteRenderer paintToolRen in paintToolsRen)
        {
            paintToolRen.sortingOrder = -4;
        }

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("クリア方法はいたってシンプルで、ピースを曲の順番に並べ変え終わったらこのTRYボタンを押すだけです。"));

        //Tryのハイライトをオンに
        tryButtonRen.sortingOrder = -1;
        tryButtonHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("ただ、正しい順番のピースが並べ替えられていないとボタンを押してもクリアできませんので注意してくださいね。"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("以上でゲーム説明は終了です。お疲れさまでした！"));

        //Box11をクリックできるようにする

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
