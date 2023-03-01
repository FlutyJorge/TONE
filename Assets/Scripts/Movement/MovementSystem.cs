using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MovementSystem : MonoBehaviour
{
    public GameObject[] boxes;

    //Boxを並べたときの縦横の数
    public int puzzleSize;
    [SerializeField] int suffleCount;
    [SerializeField] bool isClear = false;
    [SerializeField] bool puzzleType1, puzzleType2; //どれか1つをTrueにする

    private int[] dirs;

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

        //パズルの種類別に処理を変更
        if (puzzleType1)
        {
            Type1Directions();

            for (int i = 0; i < dirs.Length; ++i)
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
        else if (puzzleType2)
        {
            Type2Directions();
            int nidx = idx + dirs[0];

            //nidxが回転できる配列か確認する
            for (int r = 1; r < puzzleSize; ++r)
            {
                for (int c = 0; c < puzzleSize - 1; ++c)
                {
                    if (nidx == r * puzzleSize + c)
                    {
                        ret = nidx;
                        Debug.Log(ret);
                        return ret;
                    }
                }
            }
            Debug.Log("回転できん！");
            return ret;
        }
        Debug.Log("パズルの種類決めろ！");
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

        //パズルの種類別に処理を変更
        if (puzzleType1)
        {
            Type1Change(idx1, idx2, clickedObj);
        }
        else if (puzzleType2)
        {
            Type2Change(idx1, idx2, clickedObj);
        }
    }

    //クリアチェック
    private void CheckClear()
    {
        if (isClear)
        {
            return;
        }

        //1つでもインデックスとBoxの名前が一致しなければフラグをオフに
        bool canclear = true;
        for (int i = 0; i < boxes.Length; ++i)
        {
            if (boxes[i].name != i.ToString())
            {
                canclear = false;
            }
        }

        if (canclear)
        {
            isClear = true;
            Debug.Log("クリア");
        }
    }

    //パズルの種類別に移動方向を検索
    private void Type1Directions()
    {
        dirs = new int[4]
        {
            -puzzleSize, //上
            +puzzleSize, //下
            -1, //左
            +1 //右
        };
    }

    private void Type2Directions()
    {
        dirs = new int[1]
        {
            -1 //時計回り
        };
    }

    //パズルの種類別に移動処理を変行う
    private void Type1Change(int idx1, int idx2, GameObject clickedObj)
    {
        //位置変更
        Vector2 tmpPos = clickedObj.transform.position;
        clickedObj.transform.position = boxes[idx2].transform.position;
        boxes[idx2].transform.position = tmpPos;

        //配列のデータを更新
        GameObject currentBox = boxes[idx1];
        boxes[idx1] = boxes[idx2];
        boxes[idx2] = currentBox;
    }

    private void Type2Change(int idx1, int idx2, GameObject clickedObj)
    {
        //位置変更
        Vector2 tmpPos = clickedObj.transform.position;
        clickedObj.transform.position = boxes[idx2].transform.position;
        boxes[idx2].transform.position = boxes[idx2 - puzzleSize].transform.position;
        boxes[idx2 - puzzleSize].transform.position = boxes[idx1 - puzzleSize].transform.position;
        boxes[idx1 - puzzleSize].transform.position = tmpPos;

        //配列のデータを更新
        GameObject tmpBox1 = boxes[idx1];
        GameObject tmpBox2 = boxes[idx2];
        GameObject tmpBox3 = boxes[idx2 - puzzleSize];

        boxes[idx1] = boxes[idx1 - puzzleSize];
        boxes[idx2] = tmpBox1;
        boxes[idx2 - puzzleSize] = tmpBox2;
        boxes[idx1 - puzzleSize] = tmpBox3;
    }
}
