using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FailedManager : MonoBehaviour
{
    [SerializeField] CommonMovement comMov;
    [SerializeField] SoundManager soundMana;
    [SerializeField] SEManager seMana;
    [SerializeField] ClearManager clearMana;
    [SerializeField] GameObject blackBoard;

    [Space(10)]
    [Header("SE")]
    [SerializeField] AudioClip failedSound;

    [HideInInspector] public bool isStartedShowFailedBoard = false;

    void Update()
    {
        if (comMov.swapLimit1 != 0 || comMov.swapLimit2 != 0 || comMov.swapLimit3 != 0)
        {
            return;
        }

        if (isStartedShowFailedBoard)
        {
            return;
        }

        StartCoroutine(ShowFaliedBoard());
        isStartedShowFailedBoard = true;
    }

    private IEnumerator ShowFaliedBoard()
    {
        float fadeTime = 1f;

        soundMana.songAudioS.DOFade(0, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        clearMana.CheckBoxRow();

        if (clearMana.isClear)
        {
            yield break;
        }

        SpriteRenderer blackBoardRen = blackBoard.GetComponent<SpriteRenderer>();
        BoxCollider2D blackBoardCol = blackBoard.GetComponent<BoxCollider2D>();

        soundMana.songAudioS.Stop();
        seMana.PlayOneShot(failedSound);
        blackBoardRen.DOFade(1, fadeTime);
        blackBoardCol.enabled = true;

        int setPosition = -10;
        this.gameObject.transform.DOMove(new Vector3(0, setPosition, 0), fadeTime).SetRelative(true).SetEase(Ease.InOutBack);
    }
}
