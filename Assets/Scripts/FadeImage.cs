using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeImage : MonoBehaviour
{
    [Header("�ŏ�����t�F�[�h�C�����������Ă��邩")] public bool firstFadeInComp;

    private Image img;
    private bool isFadeInCompleted = false;
    private bool isFadeInStarted = false;
    private int frameCount = 0;
    private float fadeTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();

        if (firstFadeInComp)
        {
            FadeInComplete();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //�V�[���ǂݍ��ݎ��͏������������邽�߁A2�t���[���ҋ@���Ă���t�F�[�h���s��
        if (frameCount > 2)
        {
            if (isFadeInCompleted)
            {
                return;
            }

            if (!isFadeInStarted)
            {
                isFadeInStarted = true;
                StartCoroutine(UpdateFadeIn());
            }
        }
        ++frameCount;
    }

    private IEnumerator UpdateFadeIn()
    {
        img.DOFade(0, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        img.raycastTarget = false;
        isFadeInCompleted = false;
        yield break;
    }

    private void FadeInComplete()
    {
        isFadeInCompleted = true;
        img.color = new Color(1, 1, 1, 0);
        img.raycastTarget = false;
    }

    public void StartFadeOut()
    {
        img.raycastTarget = true;
        img.DOFade(1, fadeTime);
    }
}
