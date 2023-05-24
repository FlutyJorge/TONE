using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FailedManager : MonoBehaviour
{
    [SerializeField] CommonMovement comMov;
    [SerializeField] SEManager seMana;
    [SerializeField] ClearManager clearMana;
    [SerializeField] GameObject blackBoard;
    [SerializeField] AudioClip failedSound;

    private bool isStartedShowFailedBoard = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //スワップができなくなったら処理を行う
        if (comMov.swapLimit1 == 0 && comMov.swapLimit2 == 0 && comMov.swapLimit3 == 0 && !isStartedShowFailedBoard)
        {
            StartCoroutine(ShowFaliedBoard());
            isStartedShowFailedBoard = true;
        }
    }

    private IEnumerator ShowFaliedBoard()
    {
        //スワップアニメーションが終わるまで待機
        yield return new WaitForSeconds(1f);

        //Boxが正しく並べられているか判定
        clearMana.CheckBoxRow();

        //クリアなら何も処理を行わない
        if (clearMana.isClear)
        {
            yield break;
        }

        SpriteRenderer blackBoardRen = blackBoard.GetComponent<SpriteRenderer>();
        BoxCollider2D blackBoardCol = blackBoard.GetComponent<BoxCollider2D>();

        seMana.PlayOneShot(failedSound);
        blackBoardRen.DOFade(1, 1f);
        blackBoardCol.enabled = true;

        this.gameObject.transform.DOMove(new Vector3(0, -10, 0), 1f).SetRelative(true).SetEase(Ease.InOutBack);
    }
}
