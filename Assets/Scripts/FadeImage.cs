using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeImage : MonoBehaviour
{
    private Image img;
    [Header("最初からフェードインが完了しているか")] public bool firstFadeInComp;
    private bool isFadeInCompleted = false;
    private bool isFadeInStarted = false;
    private int frameCount = 0;

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
        img.DOFade(0, 1);
        yield return new WaitForSeconds(1f);
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
        img.DOFade(1, 1);
    }
}
