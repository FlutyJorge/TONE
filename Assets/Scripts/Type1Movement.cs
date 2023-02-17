using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type1Movement : MonoBehaviour
{
    public CommonMovement comMov;

    private int[] dirs;

    //初期の移動タイプを設定
    private void Start()
    {
        ChangeType1();
    }

    private void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            ChangeType1();
        }
    }

    //移動可能Boxのインデックスを取得
    private int GetTargetIndex(GameObject clickedObj)
    {
        int ret = -1;

        //押されたBoxのインデックスを取得
        int idx = comMov.GetBoxIndex(clickedObj);

        //エラーチェック
        if (idx < 0)
        {
            Debug.Log("クリックされたBoxのインデックスが取得できてない");
            return ret;
        }

        dirs = new int[4]
        {
            -comMov.puzzleSize, //上
            +comMov.puzzleSize, //下
            -1, //左
            +1 //右
        };

        for (int i = 0; i < dirs.Length; ++i)
        {
            //4方向Boxのインデックス
            int nidx = idx + dirs[i];

            //配列オーバーチェック
            if (nidx < 0 || nidx > comMov.boxes.Length - 1)
            {
                continue;
            }

            //名前による判別で移動先のインデックスを確定
            if (comMov.boxes[nidx].name == "15")
            {
                ret = nidx;
            }
        }

        return ret;
    }

    private void Type1Change(int idx1, int idx2, GameObject clickedObj)
    {
        //位置変更
        Vector2 tmpPos = clickedObj.transform.position;
        clickedObj.transform.position = comMov.boxes[idx2].transform.position;
        comMov.boxes[idx2].transform.position = tmpPos;

        //配列のデータを更新
        GameObject currentBox = comMov.boxes[idx1];
        comMov.boxes[idx1] = comMov.boxes[idx2];
        comMov.boxes[idx2] = currentBox;
    }

    private void ChangeType1()
    {
        comMov.getTargetIndex = GetTargetIndex;
        comMov.change = Type1Change;
    }
}
