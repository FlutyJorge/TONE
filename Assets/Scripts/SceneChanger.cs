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

    [HideInInspector] public static bool firstPush = false; //OptionManagerでのシーン移動ボタン連打対策用
    const int fadeTime = 1;

    void Start()
    {
        firstPush = false;
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
                noteMana.songAudioS[i].DOFade(0, fadeTime);
            }

            StartCoroutine(GoStageScene(stageNum));
            firstPush = true;
        }
    }

    private IEnumerator GoStageScene(int stageNum)
    {
        //FadeOutが完了するまで待つ
        yield return new WaitForSeconds(fadeTime);

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
            for (int i = 0; i < 3; ++i)
            {
                noteMana.songAudioS[i].DOFade(0, fadeTime);
            }
            StartCoroutine(GoTitleOrTutorial("Tutorial"));
            firstPush = true;
        }
    }

    //タイトルに遷移する処理
    public void ChangeToTitleScene()
    {
        if (!firstPush)
        {
            ChangeSceneFromStage();
            gameSta.auSource.DOFade(0, fadeTime);
            StartCoroutine(GoTitleOrTutorial("Title"));
        }
    }

    //タイトル、チュートリアル移動時に使用
    private IEnumerator GoTitleOrTutorial(string scene)
    {
        //FadeOut完了まで待機
        yield return new WaitForSeconds(fadeTime);

        DOTween.Clear(true);
        SceneManager.LoadScene(scene);
    }

    //リトライボタンに使用
    public void Retry()
    {
        if (!firstPush)
        {
            ChangeSceneFromStage();
            StartCoroutine(GoRetry());
        }
    }

    private IEnumerator GoRetry()
    {
        //FadeOutが完了するまで待つ
        yield return new WaitForSeconds(fadeTime);

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
            ChangeSceneFromStage();
            StartCoroutine(GonextScene());
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

    private void ChangeSceneFromStage()
    {
        firstPush = true;
        fadeImg.StartFadeOut();
        SoundManager.seAudioS.PlayOneShot(sceneChangeSound);
        soundMana.songAudioS.DOFade(0, fadeTime);
        SoundManager.seAudioS.DOFade(0, fadeTime);
    }

    //ゲーム終了
    public void QiutGame()
    {
        if (!firstPush)
        {
            firstPush = true;
            fadeImg.StartFadeOut();
            audioSForTitle.PlayOneShot(sceneChangeSound);
            for (int i = 0; i < 3; ++i)
            {
                noteMana.songAudioS[i].DOFade(0, fadeTime);
            }
            StartCoroutine(QuitGameCoro());
        }
    }

    private IEnumerator QuitGameCoro()
    {
        yield return new WaitForSeconds(fadeTime);
        Application.Quit();
    }
}
