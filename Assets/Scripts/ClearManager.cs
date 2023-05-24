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
    public GameObject clearBoard;
    public GameObject clearBackground;
    public ParticleSystem[] particles = new ParticleSystem[8];
    [SerializeField] AudioClip clearJudgeSound;
    [SerializeField] AudioClip buzzerSound;
    [SerializeField] AudioClip clearSound;
    [SerializeField] GameObject[] correctBoxRow = new GameObject[16];

    private float judgeTime;
    [HideInInspector] public bool isClear;


    // Start is called before the first frame update
    void Start()
    {
        judgeTime = clearJudgeSound.length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickTry()
    {
        StartCoroutine(ShowClearScene());
    }

    private IEnumerator ShowClearScene()
    {
        soundMana.songAudioS.DOFade(0, 0.5f);
        seMana.PlayOneShot(clearJudgeSound);
        BoxCollider2D clearBackgroundCol =  clearBackground.GetComponent<BoxCollider2D>();
        clearBackgroundCol.enabled = true;

        SpriteRenderer clearBackGroundRen = clearBackground.GetComponent<SpriteRenderer>();
        clearBackGroundRen.DOFade(1, judgeTime);
        yield return new WaitForSeconds(judgeTime);

        //Boxが正しく並べられているか判定
        CheckBoxRow();

        //クリアの場合
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
        //クリアでない場合
        else
        {
            seMana.PlayOneShot(buzzerSound);
            clearBackGroundRen.DOFade(0, 0.5f);
            clearBackgroundCol.enabled = false;
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
