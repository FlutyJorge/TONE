using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Type1Movement : MonoBehaviour
{
    public CommonMovement comMov;

    private int[] dirs;

    private void Awake()
    {
        comMov.getTargetIndex = GetTargetIndex;
    }

    public int GetTargetIndex(GameObject clickedObj)
    {
        int ret = -1;
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
            int nidx = idx + dirs[i];

            //配列オーバーチェック
            if (nidx < 0 || nidx > comMov.boxes.Length - 1)
            {
                continue;
            }

            GameObject childObj = comMov.boxes[nidx].transform.GetChild(0).gameObject;

            if (childObj.tag == "PaintToolCheckerForCircle")
            {
                ret = nidx;
            }
        }
        return ret;
    }
}
