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
        ChangeToType1();
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
        Vector3 tmpPos = clickedObj.transform.position;
        Vector3 parentPos = (tmpPos + comMov.boxes[idx2].transform.position) / 2;
        /*clickedObj.transform.DOMove(comMov.boxes[idx2].transform.position, 1f);
        comMov.boxes[idx2].transform.DOMove(tmpPos, 1);*/
        comMov.parent.transform.position = parentPos;
        clickedObj.transform.SetParent(comMov.parent.transform);
        comMov.boxes[idx2].transform.SetParent(comMov.parent.transform);
        comMov.parent.transform.DORotate(new Vector3(0, 0, -180), 1f).SetRelative();
        foreach (Transform children in comMov.parent.transform)
        {
            children.transform.DOLocalRotate(new Vector3(0, 0, 180), 1f).SetRelative();
        }

        //配列のデータを更新
        GameObject currentBox = comMov.boxes[idx1];
        comMov.boxes[idx1] = comMov.boxes[idx2];
        comMov.boxes[idx2] = currentBox;
    }

    public void ChangeToType1()
    {
        Debug.Log("Type1");
        comMov.getTargetIndex = GetTargetIndex;
        comMov.change = Type1Change;

        comMov.puzzleType1 = true;
        comMov.puzzleType2 = false;
        comMov.puzzleType3 = false;

        foreach (GameObject box in comMov.boxes)
        {
            box.transform.localScale = new Vector2(1.5f, 1.5f);
        }
    }
}
