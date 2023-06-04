using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeImage : MonoBehaviour
{
    [Header("最初からフェードインが完了しているか")] public bool firstFadeInComp;

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
        //シーン読み込み時は処理が多く走るため、2フレーム待機してからフェードを行う
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
