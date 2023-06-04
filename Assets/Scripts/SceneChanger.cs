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
    [Header("�^�C�g���ȊO�̏ꍇ�A�^�b�`")]
    [SerializeField] SoundManager soundMana;
    [SerializeField] GameStarter gameSta;

    [Space(10)]
    [Header("�^�C�g���̏ꍇ�A�^�b�`")]
    [SerializeField] NotesManager noteMana;
    [SerializeField] AudioSource audioSForTitle;

    [HideInInspector] public static bool firstPush = false; //OptionManager�ł̃V�[���ړ��{�^���A�ő΍��p
    const int fadeTime = 1;

    void Start()
    {
        firstPush = false;
    }

    //�^�C�g������e�X�e�[�W�ɑJ�ڂ��鏈��
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
        //FadeOut����������܂ő҂�
        yield return new WaitForSeconds(fadeTime);

        DOTween.Clear(true);
        SceneManager.LoadScene("Stage" + stageNum.ToString());
    }

    //�^�C�g������`���[�g���A���ɑJ�ڂ��鏈��
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

    //�^�C�g���ɑJ�ڂ��鏈��
    public void ChangeToTitleScene()
    {
        if (!firstPush)
        {
            ChangeSceneFromStage();
            gameSta.auSource.DOFade(0, fadeTime);
            StartCoroutine(GoTitleOrTutorial("Title"));
        }
    }

    //�^�C�g���A�`���[�g���A���ړ����Ɏg�p
    private IEnumerator GoTitleOrTutorial(string scene)
    {
        //FadeOut�����܂őҋ@
        yield return new WaitForSeconds(fadeTime);

        DOTween.Clear(true);
        SceneManager.LoadScene(scene);
    }

    //���g���C�{�^���Ɏg�p
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
        //FadeOut����������܂ő҂�
        yield return new WaitForSeconds(fadeTime);

        DOTween.Clear(true);

        //���݂̃V�[�����擾
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene.name);
        SceneManager.LoadScene(scene.name);
    }

    //�N���A��̎��V�[���J�ڂɎg�p
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
        //FadeOut����������܂ő҂�
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

    //�Q�[���I��
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
