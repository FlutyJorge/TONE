using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type2Movment : MonoBehaviour
{
    public CommonMovement comMov;

    private int dir;

    private void Update()
    {
        if (Input.GetKeyDown("2"))
        {
            ChangeType2();
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

        dir = -1; //時計回り
        int nidx = idx + dir;

        //nidxが回転できる配列か確認する
        for (int r = 1; r < comMov.puzzleSize; ++r)
        {
            for (int c = 0; c < comMov.puzzleSize - 1; ++c)
            {
                if (nidx == r * comMov.puzzleSize + c)
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

    private void Type2Change(int idx1, int idx2, GameObject clickedObj)
    {
        //位置変更
        Vector2 tmpPos = clickedObj.transform.position;
        clickedObj.transform.position = comMov.boxes[idx2].transform.position;
        comMov.boxes[idx2].transform.position = comMov.boxes[idx2 - comMov.puzzleSize].transform.position;
        comMov.boxes[idx2 - comMov.puzzleSize].transform.position = comMov.boxes[idx1 - comMov.puzzleSize].transform.position;
        comMov.boxes[idx1 - comMov.puzzleSize].transform.position = tmpPos;

        //配列のデータを更新
        GameObject tmpBox1 = comMov.boxes[idx1];
        GameObject tmpBox2 = comMov.boxes[idx2];
        GameObject tmpBox3 = comMov.boxes[idx2 - comMov.puzzleSize];

        comMov.boxes[idx1] = comMov.boxes[idx1 - comMov.puzzleSize];
        comMov.boxes[idx2] = tmpBox1;
        comMov.boxes[idx2 - comMov.puzzleSize] = tmpBox2;
        comMov.boxes[idx1 - comMov.puzzleSize] = tmpBox3;
    }

    private void ChangeType2()
    {
        comMov.getTargetIndex = GetTargetIndex;
        comMov.change = Type2Change;
    }
}
