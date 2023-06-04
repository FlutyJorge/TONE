using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ClearManager : MonoBehaviour
{
    [SerializeField] SoundManager soundMana;
    [SerializeField] CommonMovement comMov;
    [SerializeField] SEManager seMana;
    [SerializeField] OptionClick optinClick;
    public GameObject clearBoard;
    public GameObject clearBackground;
    public ParticleSystem[] particles = new ParticleSystem[8];
    [SerializeField] GameObject[] correctBoxRow = new GameObject[16]; //Ç±ÇÃîzóÒÇ…ÉNÉäÉAéûÇÃBoxÇÃï¿Ç—èáÇäiî[Ç∑ÇÈ

    [Space(10)]
    [Header("SE")]
    [SerializeField] AudioClip clearJudgeSound;
    [SerializeField] AudioClip buzzerSound;
    [SerializeField] AudioClip clearSound;

    private float judgeTime;
    [HideInInspector] public bool isClear;
    [HideInInspector] public bool isJudging;

    void Start()
    {
        judgeTime = clearJudgeSound.length;
    }

    public void ClickTry()
    {
        StartCoroutine(ShowClearScene());
        isJudging = true;
    }

    private IEnumerator ShowClearScene()
    {
        float fadeTime = 0.5f;
        soundMana.songAudioS.DOFade(0, fadeTime);
        seMana.PlayOneShot(clearJudgeSound);
        BoxCollider2D clearBackgroundCol =  clearBackground.GetComponent<BoxCollider2D>();
        clearBackgroundCol.enabled = true;

        SpriteRenderer clearBackGroundRen = clearBackground.GetComponent<SpriteRenderer>();
        clearBackGroundRen.DOFade(1, judgeTime);
        yield return new WaitForSeconds(judgeTime);

        CheckBoxRow();

        if (isClear)
        {
            seMana.PlayOneShot(clearSound);
            clearBoard.transform.DOMove(new Vector3(0, 0, 0), 1f).SetEase(Ease.InOutBack);
            yield return new WaitForSeconds(1f);

            foreach (ParticleSystem particle in particles)
            {
                particle.Play();
            }
        }
        else
        {
            seMana.PlayOneShot(buzzerSound);
            soundMana.songAudioS.DOFade(VolumeHolder.instance.musicVolume, fadeTime);

            if (!optinClick.isOptionAppeared)
            {
                clearBackGroundRen.DOFade(0, fadeTime);
                clearBackgroundCol.enabled = false;
            }
            isJudging = false;
        }
        yield break;
    }

    public void CheckBoxRow()
    {
        for (int i = 0; i < correctBoxRow.Length; ++i)
        {
            if (comMov.boxes[i] != correctBoxRow[i])
            {
                isClear = false;
                break;
            }
            isClear = true;
        }
    }
}
