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

    //�^�C�g������e�X�e�[�W�ɑJ�ڂ��鏈��
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
        //FadeOut����������܂ő҂�
        yield return new WaitForSeconds(1f);

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
            StartCoroutine(GoTitleOrTutorial("Tutorial"));
            firstPush = true;
        }
    }

    //�^�C�g���ɑJ�ڂ��鏈��
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

    //�^�C�g���A�`���[�g���A���ԑJ�ڂŎg�p
    private IEnumerator GoTitleOrTutorial(string scene)
    {
        //FadeOut����������܂ő҂�
        yield return new WaitForSeconds(1f);

        DOTween.Clear(true);
        SceneManager.LoadScene(scene);
    }

    //���g���C�{�^���Ɏg�p
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
        //FadeOut����������܂ő҂�
        yield return new WaitForSeconds(1f);

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
        //FadeOut����������܂ő҂�
        yield return new WaitForSeconds(1f);

        DOTween.Clear(true);
        int currentIdx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIdx + 1);
    }
}
