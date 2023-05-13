using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CommonMovement : MonoBehaviour
{
    public GameObject[] boxes;
    public GameObject parent;
    public bool swaping = false;
    public int swapLimit1, swapLimit2, swapLimit3;
    [SerializeField] int[] clearJudgmentNum = new int[16];

    //このスクリプトからType系へのへの参照を避ける
    public delegate int Delegate1(GameObject clickedObj);
    public Delegate1 getTargetIndex;
    public delegate void Delegate2(int idx1, int idx2, GameObject clickedObj);
    public Delegate2 type2Change;

    public int puzzleSize; //Boxを並べたときの縦横の数
    [SerializeField] int suffleCount;
    public bool puzzleType1, puzzleType2, puzzleType3;

    [SerializeField] Sprite[] CounterSprites;
    [SerializeField] GameObject[] typeCounters;
    public SpriteRenderer[] counterSpriteRen = new SpriteRenderer[3];

    private void Start()
    {
        for (int i = 0; i < counterSpriteRen.Length; ++i)
        {
            counterSpriteRen[i] = typeCounters[i].GetComponent<SpriteRenderer>();
        }

        //Suffle(getTargetIndex);
    }

    //クリックしたBoxのインデックスを取得
    public int GetBoxIndex(GameObject clickedObj)
    {
        int ret = -1;

        for (int i = 0; i < boxes.Length; ++i)
        {
            //もしクリックされたBoxがboxes[i]と同じだったら
            if (clickedObj == boxes[i])
            {
                ret = i;
            }
        }
        return ret;
    }

    //Boxの入れ替え
    public void ChangeBox(GameObject clickedObj)
    {
        swaping = true;
        //交換するBoxのインデックス2つ
        int idx1 = GetBoxIndex(clickedObj);
        int idx2 = getTargetIndex(clickedObj);

        //エラーチェック
        if (clickedObj == null || idx1 < 0 || idx2 < 0)
        {
            Debug.Log("エラー");
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
        if (puzzleType1)
        {
            if (swapLimit1 == 0)
            {
                Debug.Log("回数上限！");
                return;
            }
            --swapLimit1;
            StartCoroutine(ChangeCounterNum(1, swapLimit1));
        }
        else if (puzzleType3)
        {
            if (swapLimit3 == 0)
            {
                Debug.Log("回数上限！");
                return;
            }
            --swapLimit3;
            StartCoroutine(ChangeCounterNum(3, swapLimit3));
        }
        else
        {
            Debug.Log("puzzletype3になってる");
        }

        //位置変更
        Vector3 tmpPos = clickedObj.transform.position;
        Vector3 parentPos = (tmpPos + boxes[idx2].transform.position) / 2;

        parent.transform.position = parentPos;
        clickedObj.transform.SetParent(parent.transform);
        boxes[idx2].transform.SetParent(parent.transform);
        parent.transform.DORotate(new Vector3(0, 0, -180), 1f).SetRelative();
        foreach (Transform children in parent.transform)
        {
            children.transform.DOLocalRotate(new Vector3(0, 0, 180), 1f).SetRelative();
        }

        //配列のデータを更新
        GameObject currentBox = boxes[idx1];
        boxes[idx1] = boxes[idx2];
        boxes[idx2] = currentBox;
    }

    //スワップ終了時に親子関係解除
    public IEnumerator DetachChildren(GameObject parent)
    {
        yield return new WaitForSeconds(1f);
        parent.transform.DetachChildren();
        swaping = false;
        yield break;
    }

    public IEnumerator ChangeCounterNum(int puzzleTypeNum, int swapLimit)
    {
        counterSpriteRen[puzzleTypeNum - 1].DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);

        if (swapLimit > 0)
        {
            counterSpriteRen[puzzleTypeNum - 1].sprite = CounterSprites[swapLimit - 1];
            counterSpriteRen[puzzleTypeNum - 1].DOFade(1, 0.5f);
        }
        else
        {
            counterSpriteRen[puzzleTypeNum - 1].sprite = null;
        }
        yield break;
    }

    //シャッフル(本番は不要)
    public void Suffle(Delegate1 getTargetIndex)
    {
        //シャッフル
        for (int i = 0; i < suffleCount; ++i)
        {
            List<GameObject> mboxes = new List<GameObject>();
            foreach (GameObject box in boxes)
            {
                //移動可能なブロック
                if (getTargetIndex(box) > -1)
                {
                    mboxes.Add(box);
                }
            }

            //動かせるBoxがない
            if (mboxes.Count < 1)
            {
                continue;
            }

            //ランダムで1つ動かす
            int rnd = Random.Range(0, mboxes.Count);
            ChangeBox(mboxes[rnd]);
        }
    }
}
