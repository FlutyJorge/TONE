using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] FadeImage fadeImg;
    [SerializeField] AudioClip sceneChangeSound;

    [Space(10)]
    [Header("タイトル以外の場合アタッチ")]
    [SerializeField] SoundManager soundMana;
    [SerializeField] GameStarter gameSta;

    [Space(10)]
    [Header("タイトルの場合アタッチ")]
    [SerializeField] NotesManager noteMana;
    [SerializeField] AudioSource audioSForTitle;

    [HideInInspector] public static bool firstPush = false;

    // Start is called before the first frame update
    void Start()
    {
        firstPush = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //タイトルから各ステージに遷移する処理
    public void ChangeToStageScene(int stageNum)
    {
        if (!firstPush)
        {
            fadeImg.StartFadeOut();
            audioSForTitle.PlayOneShot(sceneChangeSound);
            for (int i = 0; i < 3; ++i)
            {
                noteMana.songAudioS[i].DOFade(0, 1f);
            }

            StartCoroutine(GoStageScene(stageNum));
            firstPush = true;
        }
    }

    private IEnumerator GoStageScene(int stageNum)
    {
        //FadeOutが完了するまで待つ
        yield return new WaitForSeconds(1f);

        DOTween.Clear(true);
        SceneManager.LoadScene("Stage" + stageNum.ToString());
    }

    //タイトルからチュートリアルに遷移する処理
    public void ChangeToTutorialScene()
    {
        if (!firstPush)
        {
            fadeImg.StartFadeOut();
            audioSForTitle.PlayOneShot(sceneChangeSound);
            StartCoroutine(GoTitleOrTutorial("Tutorial"));
            firstPush = true;
        }
    }

    //タイトルに遷移する処理
    public void ChangeToTitleScene()
    {
        if (!firstPush)
        {
            fadeImg.StartFadeOut();
            SoundManager.seAudioS.PlayOneShot(sceneChangeSound);
            soundMana.songAudioS.DOFade(0, 1f);
            SoundManager.seAudioS.DOFade(0, 1f);
            gameSta.auSource.DOFade(0, 1f);
            StartCoroutine(GoTitleOrTutorial("Title"));
            firstPush = true;
        }
    }

    //タイトル、チュートリアル間遷移で使用
    private IEnumerator GoTitleOrTutorial(string scene)
    {
        //FadeOutが完了するまで待つ
        yield return new WaitForSeconds(1f);

        DOTween.Clear(true);
        SceneManager.LoadScene(scene);
    }

    //リトライボタンに使用
    public void Retry()
    {
        if (!firstPush)
        {
            fadeImg.StartFadeOut();
            SoundManager.seAudioS.PlayOneShot(sceneChangeSound);
            soundMana.songAudioS.DOFade(0, 1f);
            SoundManager.seAudioS.DOFade(0, 1f);
            gameSta.auSource.DOFade(0, 1f);
            StartCoroutine(GoRetry());
            firstPush = true;
        }
    }

    private IEnumerator GoRetry()
    {
        //FadeOutが完了するまで待つ
        yield return new WaitForSeconds(1f);

        DOTween.Clear(true);

        //現在のシーンを取得
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene.name);
        SceneManager.LoadScene(scene.name);
    }

    //クリア後の次シーン遷移に使用
    public void ChangeToNextScene()
    {
        if (!firstPush)
        {
            fadeImg.StartFadeOut();
            SoundManager.seAudioS.PlayOneShot(sceneChangeSound);
            soundMana.songAudioS.DOFade(0, 1f);
            SoundManager.seAudioS.DOFade(0, 1f);
            StartCoroutine(GonextScene());
            firstPush = true;
        }
    }

    private IEnumerator GonextScene()
    {
        //FadeOutが完了するまで待つ
        yield return new WaitForSeconds(1f);

        DOTween.Clear(true);
        int currentIdx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIdx + 1);
    }
}
