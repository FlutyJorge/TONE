using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameStarter : MonoBehaviour
{
    [SerializeField] GameObject stagePrefab;
    [SerializeField] GameObject listenPrefab;
    [SerializeField] GameObject startPrefab;
    [SerializeField] GameObject[] countdownPrefab = new GameObject[3];
    [SerializeField] GameObject[] dotPrefab = new GameObject[3];
    [SerializeField] GameObject blackBoard;

    [HideInInspector] public bool isGameStarterEnd = false;

    private GameObject listen;
    private SpriteRenderer listenRen;
    private GameObject[] dot = new GameObject[3];
    private SpriteRenderer[] dotRen = new SpriteRenderer[3];
    private Vector3 firstPos = new Vector3(0, 7, 0);
    private AudioSource auSource;
    private bool isJumping = true;
    private bool isAudioEnd = false;
    private bool isStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartGame());
        auSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAudioEnd && !auSource.isPlaying && !isStarted)
        {
            StartCoroutine(ShowStart());
            isStarted = true;
        }
    }

    private IEnumerator StartGame()
    {
        GameObject stageChara = Instantiate(stagePrefab, firstPos, Quaternion.identity);
        GameObject[] countdown = new GameObject[3];
        SpriteRenderer[] countdownRen = new SpriteRenderer[3];
        stageChara.transform.DOMove(new Vector3(0, 0, 0), 1f).SetEase(Ease.InOutBack);

        yield return new WaitForSeconds(2f);

        stageChara.transform.DOMove(new Vector3(0, 7, 0), 1f).SetEase(Ease.InOutBack);

        yield return new WaitForSeconds(1f);
        Destroy(stageChara);

        for (int i = 0; i < countdownPrefab.Length; ++i)
        {
            countdown[i] = Instantiate(countdownPrefab[i]);
            countdownRen[i] = countdown[i].GetComponent<SpriteRenderer>();
        }

        for (int i = 0; i < countdownPrefab.Length; ++i)
        {
            countdownRen[i].DOFade(1, 0.5f);
            yield return new WaitForSeconds(0.6f);
            countdownRen[i].DOFade(0, 0.5f);
            yield return new WaitForSeconds(0.6f);
            Destroy(countdown[i]);
        }

        for (int i = 0; i < dot.Length; ++i)
        {
            dot[i] = Instantiate(dotPrefab[i]);
            dotRen[i] = dot[i].GetComponent<SpriteRenderer>();
        }

        listen = Instantiate(listenPrefab, new Vector3(-1, 0, 0), Quaternion.identity);
        listenRen = listen.GetComponent<SpriteRenderer>();

        //Listen...を1秒かけて表示
        listenRen.DOFade(1, 1f);
        for (int i = 0; i < dotRen.Length; ++i)
        {
            dotRen[i].DOFade(1, 1f);
        }

        StartCoroutine(StartJumpDot(dot));
        yield return new WaitForSeconds(1f);
        auSource.Play();
        isAudioEnd = true;

        yield break;
    }

    private IEnumerator JumpDot(GameObject[] dot, int dotIdx)
    {
        while(isJumping)
        {
            dot[dotIdx].transform.DOMove(new Vector3(dot[dotIdx].transform.position.x, 0.5f, 0), 0.5f).SetEase(Ease.InOutCirc);
            yield return new WaitForSeconds(0.5f);
            dot[dotIdx].transform.DOMove(new Vector3(dot[dotIdx].transform.position.x, -0.35f, 0), 0.5f).SetEase(Ease.InOutCirc);
            yield return new WaitForSeconds(1f);
        }
        yield break;
    }

    private IEnumerator StartJumpDot(GameObject[] dot)
    {
        const int dotLength = 3;
        for (int i = 0; i < dotLength; ++i)
        {
            StartCoroutine(JumpDot(dot, i));
            yield return new WaitForSeconds(0.2f);
        }

        yield break;
    }

    //曲が終わった際にSTARTを出現させる
    private IEnumerator ShowStart()
    {
        //LISTEN...を1秒かけて非表示に
        listenRen.DOFade(0, 1);
        for (int i = 0; i < dotRen.Length; ++i)
        {
            dotRen[i].DOFade(0, 1);
        }

        yield return new WaitForSeconds(1f);
        Destroy(listen);
        isJumping = false;
        Invoke("DestroyDot", 1.6f);

        GameObject start = Instantiate(startPrefab);
        start.transform.DOMove(new Vector3(start.transform.position.x, 0, 0), 1f).SetEase(Ease.InOutBack);
        yield return new WaitForSeconds(1.2f);
        start.transform.DOMove(new Vector3(start.transform.position.x, 7, 0), 1f).SetEase(Ease.InOutBack);

        //whiteBackgroundを非表示に
        SpriteRenderer blackBoardRen = blackBoard.GetComponent<SpriteRenderer>();

        blackBoardRen.DOFade(0, 0.5f);
        yield return new WaitForSeconds(1f);
        Destroy(start);
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
