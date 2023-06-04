using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameStarter : MonoBehaviour
{
    [Header("チュートリアルならtrueに")] public bool isTutorial = false;

    [Space(10)]
    [Header("アタッチが必要なプレハブ、オブジェクト")]
    [SerializeField] GameObject stagePrefab;
    [SerializeField] GameObject listenPrefab;
    [SerializeField] GameObject startPrefab;
    [SerializeField] GameObject[] countdownPrefab = new GameObject[3];
    [SerializeField] GameObject[] dotPrefab = new GameObject[3];
    [SerializeField] GameObject blackBoard;

    [HideInInspector] public AudioSource auSource;
    [HideInInspector] public bool isGameStarterEnd = false;
    private Vector3 firstPos = new Vector3(0, 7, 0); //7はカメラ範囲にオブジェクトが映らない時のY座標
    private int behindOption = -2; //オプション画面の上にオブジェクトが描画されないよう、インスタンス生成時にOrderInlayerを指定する変数
    const int center = 0;
    const int outOfCamera = 7;
    private bool isJumping = true;
    private bool isSongEnd = false;
    private bool isStarted = false;

    //インスタンス化されたオブジェクトやレンダラー
    private GameObject[] countdown = new GameObject[3];
    private SpriteRenderer[] countdownRen = new SpriteRenderer[3];
    private GameObject[] dot = new GameObject[3];
    private SpriteRenderer[] dotRen = new SpriteRenderer[3];
    private GameObject listen;
    private SpriteRenderer listenRen;

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(StartGame());
        auSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSongEnd && !auSource.isPlaying && !isStarted)
        {
            StartCoroutine(ShowStart());
            isStarted = true;
        }
    }

    private IEnumerator StartGame()
    {
        //FadeInが完了するまでの待機時間
        yield return new WaitForSeconds(1f);

        GameObject stageChara = Instantiate(stagePrefab, firstPos, Quaternion.identity);
        SpriteRenderer stageCharaRen = stageChara.GetComponent<SpriteRenderer>();
        stageCharaRen.sortingOrder = behindOption;

        int fadeTime = 1;
        stageChara.transform.DOMove(new Vector3(0, center, 0), fadeTime).SetEase(Ease.InOutBack);

        yield return new WaitForSeconds(2f); //stageCharが中央で待機している時間
        stageChara.transform.DOMove(new Vector3(0, outOfCamera, 0), fadeTime).SetEase(Ease.InOutBack);

        yield return new WaitForSeconds(fadeTime);
        Destroy(stageChara);

        GenerateObject(countdown, countdownPrefab, countdownRen);

        for (int i = 0; i < countdownPrefab.Length; ++i)
        {
            float countdownFadetime = 0.5f;
            float countdownShowTime = countdownFadetime + 0.1f;

            countdownRen[i].DOFade(1, countdownFadetime);
            yield return new WaitForSeconds(countdownShowTime);
            countdownRen[i].DOFade(0, countdownFadetime);
            yield return new WaitForSeconds(countdownShowTime);
            Destroy(countdown[i]);
        }

        GenerateObject(dot, dotPrefab, dotRen);

        listen = Instantiate(listenPrefab, new Vector3(-1, 0, 0), Quaternion.identity);
        listenRen = listen.GetComponent<SpriteRenderer>();
        listenRen.sortingOrder = behindOption;

        listenRen.DOFade(1, fadeTime);
        for (int i = 0; i < dotRen.Length; ++i)
        {
            dotRen[i].DOFade(1, fadeTime);
        }

        StartCoroutine(StartJumpDot(dot));
        yield return new WaitForSeconds(1f); //dotが跳ねるまで少しの間待機時間を設ける
        auSource.Play();
        yield return new WaitForSeconds(auSource.clip.length);
        isSongEnd = true;

        yield break;
    }

    private void GenerateObject(GameObject[] obj, GameObject[] objPrefab, SpriteRenderer[] objRen)
    {
        for (int i = 0; i < obj.Length; ++i)
        {
            obj[i] = Instantiate(objPrefab[i]);
            objRen[i] = obj[i].GetComponent<SpriteRenderer>();
            objRen[i].sortingOrder = behindOption;
        }
    }

    private IEnumerator JumpDot(GameObject[] dot, int dotIdx)
    {
        float dotMoveTime = 0.5f;
        float dotHigestPoint = 0.5f;
        float dotReferencePoint = -0.35f;
        while(isJumping)
        {
            dot[dotIdx].transform.DOMove(new Vector3(dot[dotIdx].transform.position.x, dotHigestPoint, 0), dotMoveTime).SetEase(Ease.InOutCirc);
            yield return new WaitForSeconds(dotMoveTime);
            dot[dotIdx].transform.DOMove(new Vector3(dot[dotIdx].transform.position.x, dotReferencePoint, 0), dotMoveTime).SetEase(Ease.InOutCirc);
            yield return new WaitForSeconds(1f); //dotが基準点に到達してからしばらくの間を設ける
        }
        yield break;
    }

    private IEnumerator StartJumpDot(GameObject[] dot)
    {
        const int dotLength = 3;
        for (int i = 0; i < dotLength; ++i)
        {
            StartCoroutine(JumpDot(dot, i));

            //dotが跳ねるタイミングをずらすために、一定の待機時間を設ける
            yield return new WaitForSeconds(0.2f);
        }

        yield break;
    }

    private IEnumerator ShowStart()
    {
        int fadeTime = 1;
        listenRen.DOFade(0, fadeTime);
        for (int i = 0; i < dotRen.Length; ++i)
        {
            dotRen[i].DOFade(0, fadeTime);
        }

        yield return new WaitForSeconds(fadeTime);
        Destroy(listen);
        isJumping = false;
        Invoke("DestroyDot", 1.1f); //dotがfadeoutされるまでの時間を設ける

        GameObject start = Instantiate(startPrefab);
        SpriteRenderer startRen = start.GetComponent<SpriteRenderer>();
        startRen.sortingOrder = behindOption;

        int moveTime = 1;
        start.transform.DOMove(new Vector3(start.transform.position.x, center, 0), moveTime).SetEase(Ease.InOutBack);
        yield return new WaitForSeconds(1.2f);
        start.transform.DOMove(new Vector3(start.transform.position.x, outOfCamera, 0), moveTime).SetEase(Ease.InOutBack);

        SpriteRenderer blackBoardRen = blackBoard.GetComponent<SpriteRenderer>();

        blackBoardRen.DOFade(0, 0.5f); //fade時間はmoveTime以下であれば良い
        yield return new WaitForSeconds(moveTime);
        Destroy(start);

        if (!isTutorial)
        {
            blackBoard.GetComponent<BoxCollider2D>().enabled = false;
        }
        isGameStarterEnd = true;

        yield break;
    }

    //ドット削除用
    private void DestroyDot()
    {
        for (int i = 0; i < dot.Length; ++i)
        {
            Destroy(dot[i]);
        }
    }
}
