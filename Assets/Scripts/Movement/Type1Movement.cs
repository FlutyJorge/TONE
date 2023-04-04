using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Type1Movement : MonoBehaviour
{
    public CommonMovement comMov;
    [SerializeField] GameObject parent;

    private int[] dirs;

    //初期の移動タイプを設定
    private void Awake()
    {
        comMov.getTargetIndex = GetTargetIndex;
    }

    //移動可能Boxのインデックスを取得
    public int GetTargetIndex(GameObject clickedObj)
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
}
