using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CommonMovement : MonoBehaviour
{
    [SerializeField] SEManager seMana;
    public GameObject[] boxes;
    public GameObject parent;

    [Space(10)]
    [Header("回数制限がある場合に設定")]
    public bool hasSwapLimit;
    public int swapLimit1;
    public int swapLimit2;
    public int swapLimit3;
    [SerializeField] Sprite[] CounterSprites;
    [SerializeField] GameObject[] typeCounters;
    private SpriteRenderer[] counterSpriteRen = new SpriteRenderer[3];

    [Space(10)]
    [Header("SE")]
    [SerializeField] AudioClip swapSound;
    [SerializeField] AudioClip buzzerSound;

    //Delegateを道いることでCommonMovementがType1, 2, 3Movementを参照する必要がなくなる
    public delegate int Delegate1(GameObject clickedObj);
    public Delegate1 getTargetIndex;
    public delegate void Delegate2(int idx1, int idx2, GameObject clickedObj);
    public Delegate2 type2Change;

    [HideInInspector] public int puzzleSize = 4; //一辺にあるBoxの数
    [HideInInspector] public bool puzzleType1, puzzleType2, puzzleType3;
    [HideInInspector] public bool swaping = false;

    private void Start()
    {
        if (!hasSwapLimit)
        {
            return;
        }

        for (int i = 0; i < counterSpriteRen.Length; ++i)
        {
            counterSpriteRen[i] = typeCounters[i].GetComponent<SpriteRenderer>();
        }
    }

    public int GetBoxIndex(GameObject clickedObj)
    {
        int ret = -1;

        for (int i = 0; i < boxes.Length; ++i)
        {
            if (clickedObj == boxes[i])
            {
                ret = i;
            }
        }
        return ret;
    }

    public void ChangeBox(GameObject clickedObj)
    {
        swaping = true;

        int idx1 = GetBoxIndex(clickedObj);
        int idx2 = getTargetIndex(clickedObj);

        //エラーチェック
        if (clickedObj == null || idx1 < 0 || idx2 < 0)
        {
            Debug.Log("正しくBoxのIndexが取得できてない!");
            swaping = false;
            return;
        }

        if (puzzleType2)
        {
            type2Change(idx1, idx2, clickedObj);
        }
        else
        {
            Type1And3Change(idx1, idx2, clickedObj);
        }
        StartCoroutine(DetachChildren(parent));
    }

    //Type1とType3のスワップ処理は同一のため、このクラスでまとめて処理を記述する
    private void Type1And3Change(int idx1, int idx2, GameObject clickedObj)
    {
        if (puzzleType1 && hasSwapLimit)
        {
            if (swapLimit1 == 0)
            {
                seMana.PlayOneShot(buzzerSound);
                return;
            }
            --swapLimit1;
            StartCoroutine(ChangeCounterNum(1, swapLimit1));
        }
        else if (puzzleType3 && hasSwapLimit)
        {
            if (swapLimit3 == 0)
            {
                seMana.PlayOneShot(buzzerSound);
                Debug.Log("回数上限！");
                return;
            }
            --swapLimit3;
            StartCoroutine(ChangeCounterNum(3, swapLimit3));
        }

        seMana.PlayOneShot(swapSound);

        Vector3 tmpPos = clickedObj.transform.position;
        Vector3 parentPos = (tmpPos + boxes[idx2].transform.position) / 2;

        parent.transform.position = parentPos;
        clickedObj.transform.SetParent(parent.transform);
        boxes[idx2].transform.SetParent(parent.transform);

        int rotationAngle = -180;
        parent.transform.DORotate(new Vector3(0, 0, rotationAngle), 1f).SetRelative();
        foreach (Transform children in parent.transform)
        {
            children.transform.DOLocalRotate(new Vector3(0, 0, -rotationAngle), 1f).SetRelative();
        }

        GameObject currentBox = boxes[idx1];
        boxes[idx1] = boxes[idx2];
        boxes[idx2] = currentBox;


    }

    public IEnumerator DetachChildren(GameObject parent)
    {
        int movementTime = 1;
        yield return new WaitForSeconds(movementTime);
        parent.transform.DetachChildren();
        swaping = false;
        yield break;
    }

    public IEnumerator ChangeCounterNum(int puzzleTypeNum, int swapLimit)
    {
        float fadeTime = 0.5f;
        counterSpriteRen[puzzleTypeNum - 1].DOFade(0, fadeTime);
        yield return new WaitForSeconds(fadeTime);

        if (swapLimit > 0)
        {
            counterSpriteRen[puzzleTypeNum - 1].sprite = CounterSprites[swapLimit - 1];
            counterSpriteRen[puzzleTypeNum - 1].DOFade(1, fadeTime);
        }
        else
        {
            counterSpriteRen[puzzleTypeNum - 1].sprite = null;
        }
        yield break;
    }
}
