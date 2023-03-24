using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Type2Movment : MonoBehaviour
{
    public CommonMovement comMov;
    //[SerializeField] GameObject parent;


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

        int nidx = idx -1;

        //nidxが回転できる配列か確認する
        int r = idx / comMov.puzzleSize;
        int c = idx % comMov.puzzleSize;

        if (r == 0 || c == 0)
        {
            Debug.Log("端は回転できん！");
            return ret;
        }

        for (int i = 0; i < comMov.boxes.Length; ++i)
        {
            if (nidx == i)
            {
                ret = nidx;
                return ret;
            }
        }
        Debug.Log("回転できん！");
        return ret;
    }

    private void Type2Change(int idx1, int idx2, GameObject clickedObj)
    {
        //位置変更
        Vector3 tmpPos = clickedObj.transform.position;
        Vector3 parentPos = (tmpPos + comMov.boxes[idx2 - comMov.puzzleSize].transform.position) / 2;

        comMov.parent.transform.position = parentPos;
        clickedObj.transform.SetParent(comMov.parent.transform);
        comMov.boxes[idx2].transform.SetParent(comMov.parent.transform);
        comMov.boxes[idx2 - comMov.puzzleSize].transform.SetParent(comMov.parent.transform);
        comMov.boxes[idx1 - comMov.puzzleSize].transform.SetParent(comMov.parent.transform);
        comMov.parent.transform.DORotate(new Vector3(0, 0, -90), 1f).SetRelative();
        foreach (Transform children in comMov.parent.transform)
        {
            children.transform.DOLocalRotate(new Vector3(0, 0, 90), 1f).SetRelative();
        }
        //StartCoroutine(DetachChildren(parent));

        //配列のデータを更新
        GameObject tmpBox1 = comMov.boxes[idx1];
        GameObject tmpBox2 = comMov.boxes[idx2];
        GameObject tmpBox3 = comMov.boxes[idx2 - comMov.puzzleSize];

        comMov.boxes[idx1] = comMov.boxes[idx1 - comMov.puzzleSize];
        comMov.boxes[idx2] = tmpBox1;
        comMov.boxes[idx2 - comMov.puzzleSize] = tmpBox2;
        comMov.boxes[idx1 - comMov.puzzleSize] = tmpBox3;
    }

    /*private IEnumerator DetachChildren(GameObject parent)
    {
        yield return new WaitForSeconds(1f);
        parent.transform.DetachChildren();
        yield break;
    }*/

    public void ChangeToType2()
    {
        Debug.Log("Type2");
        comMov.getTargetIndex = GetTargetIndex;
        comMov.change = Type2Change;

        comMov.puzzleType1 = false;
        comMov.puzzleType2 = true;
        comMov.puzzleType3 = false;

        foreach (GameObject box in comMov.boxes)
        {
            box.transform.localScale = new Vector2(1.5f, 1.5f);
        }
    }
}
