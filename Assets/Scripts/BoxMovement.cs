using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BoxMovement : MonoBehaviour
{
    public GameObject[] boxes;

    //Boxを並べたときの縦横の数
    public int boxWidth;
    public int boxHeight;
    [SerializeField] int suffleCount;
    [SerializeField] bool isClear = false;

    private void Start()
    {
        //シャッフル
        for (int i = 0; i < suffleCount; ++i)
        {
            List<GameObject> mboxes = new List<GameObject>();
            foreach (GameObject box in boxes)
            {
                //移動可能なブロック
                if (GetTargetIndex(box) > -1)
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CheckClear();
        }
    }

    //クリックしたBoxのインデックスを取得
    private int GetBoxIndex(GameObject clickedObj)
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

    //移動可能Boxのインデックスを取得
    private int GetTargetIndex(GameObject clickedObj)
    {
        int ret = -1;

        //押されたBoxのインデックスを取得
        int idx = GetBoxIndex(clickedObj);

        //エラーチェック
        if (idx < 0)
        {
            Debug.Log("クリックされたBoxのインデックスが取得できてない");
            return ret;
        }

        //移動可能場所を4方向で検索
        List<int> dirs = new List<int>()
        {
            -boxHeight, //上
            +boxHeight, //下
            -1, //左
            +1, //右
        };

        for (int i = 0; i < dirs.Count; ++i)
        {
            //4方向Boxのインデックス
            int nidx = idx + dirs[i];

            //配列オーバーチェック
            if (nidx < 0 || nidx > boxes.Length - 1)
            {
                continue;
            }

            //名前による判別で移動先のインデックスを確定
            if (boxes[nidx].name == "15")
            {
                ret = nidx;
            }
        }

        return ret;
    }

    //Boxの入れ替え
    public void ChangeBox(GameObject clickedObj)
    {
        //交換するBoxのインデックス2つ
        int idx1 = GetBoxIndex(clickedObj);
        int idx2 = GetTargetIndex(clickedObj);

        //エラーチェック
        if (clickedObj == null || idx1 < 0 || idx2 < 0)
        {
            return;
        }

        //位置変更
        Vector2 currentPos = clickedObj.transform.position;
        clickedObj.transform.position = boxes[idx2].transform.position;
        boxes[idx2].transform.position = currentPos;

        //配列のデータを更新
        GameObject currentBox = boxes[idx1];
        boxes[idx1] = boxes[idx2];
        boxes[idx2] = currentBox;
    }

    //クリアチェック
    private void CheckClear()
    {
        if (isClear)
        {
            return;
        }

        //1つでもインデックスとBoxの名前が一致しなければフラグをオフに
        bool readyToclear = true;
        for (int i = 0; i < boxes.Length; ++i)
        {
            if (boxes[i].name != i.ToString())
            {
                readyToclear = false;
            }
        }

        if (readyToclear)
        {
            isClear = true;
            Debug.Log("クリア");
        }
    }
}
