using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type3Movement : MonoBehaviour
{
    public RectTransform selectorImage; //範囲選択時の画像
    public CommonMovement comMov;
    public List<GameObject> selectedBoxes = new List<GameObject>(); //選択中のBoxを入れる
    public bool canType3Swap = false;

    private Camera cam;
    private Rect selectionRect;
    private Vector2 startPos, endPos;

    private void Start()
    {
        DrawRectangle();
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !canType3Swap　&& comMov.puzzleType3)
        {
            startPos = Input.mousePosition;
            DeactivateAllBoxes();

            selectionRect = new Rect();
        }

        if (Input.GetMouseButton(0) && !canType3Swap && comMov.puzzleType3)
        {
            endPos = Input.mousePosition;
            DrawRectangle();

            //計算X
            if (Input.mousePosition.x < startPos.x)
            {
                selectionRect.xMin = Input.mousePosition.x;
                selectionRect.xMax = startPos.x;
            }
            else
            {
                selectionRect.xMin = startPos.x;
                selectionRect.xMax = Input.mousePosition.x;
            }

            //計算Y
            if (Input.mousePosition.y < startPos.y)
            {
                selectionRect.yMin = Input.mousePosition.y;
                selectionRect.yMax = startPos.y;
            }
            else
            {
                selectionRect.yMin = startPos.y;
                selectionRect.yMax = Input.mousePosition.y;
            }
        }

        if (Input.GetMouseButtonUp(0) && comMov.puzzleType3)
        {
            if (!canType3Swap)
            {
                CheckSelectedBox();
                startPos = endPos = Vector2.zero;
                DrawRectangle();

                //リストのソート
                SortSelectedBoxes();
            }
            else
            {
                canType3Swap = false;
            }
        }
    }

    //Boxの範囲選択画像の大きさをマウス位置に合わせて変更
    private void DrawRectangle()
    {
        //画像の中心を計算
        Vector2 boxStart = startPos;
        Vector2 center = (boxStart + endPos) / 2;

        selectorImage.position = center;

        //画像のサイズを計算
        float sizeX = Mathf.Abs(boxStart.x - endPos.x);
        float sizeY = Mathf.Abs(boxStart.y - endPos.y);

        selectorImage.sizeDelta = new Vector2(sizeX, sizeY);
    }

    //選択されたBoxのスケールを大きくする
    private void CheckSelectedBox()
    {
        foreach (GameObject box in comMov.boxes)
        {
            if (selectionRect.Contains(cam.WorldToScreenPoint(box.transform.position)))
            {
                selectedBoxes.Add(box);
            }
        }

        //Boxの個数が4以上9以下かつ8以外でないと選択は不可
        if (selectedBoxes.Count < 4 || selectedBoxes.Count > 9 || selectedBoxes.Count == 8 || IsOneRowOneColum())
        {
            selectedBoxes.Clear();
            Debug.Log("選択不可!");
        }
        else
        {
            foreach (GameObject box in selectedBoxes)
            {
                box.transform.localScale = new Vector2(1.7f, 1.7f);
            }

            //スワップを可能に
            canType3Swap = true;
        }
    }

    //選択されBox群が縦横一列でないかチェック
    private bool IsOneRowOneColum()
    {
        int idx = -1;
        bool ret = true;
        bool rowRet = true;
        bool columnRet = true;

        for (int i = 0; i < comMov.boxes.Length; ++i)
        {
            if (selectedBoxes[0] == comMov.boxes[i])
            {
                idx = i;
                break;
            }
        }

        //縦横チェック
        for (int i = 0; i < selectedBoxes.Count; ++i)
        {
            if (selectedBoxes[i] != comMov.boxes[i + idx])
            {
                rowRet = false;
            }
            else if (selectedBoxes[i] != comMov.boxes[idx + i * comMov.puzzleSize])
            {
                columnRet = false;
            }

            if (!rowRet && !columnRet)
            {
                ret = false;
            }
        }
        return ret;
    }

    //選択されたBoxの初期化
    private void DeactivateAllBoxes()
    {
        foreach (GameObject box in selectedBoxes)
        {
            box.transform.localScale = new Vector2(1.5f, 1.5f);
        }
        selectedBoxes.Clear();
    }

    //選択されたBoxをインデックスでソートする
    private void SortSelectedBoxes()
    {
        List<int> selectedIdx = new List<int>();
        foreach (GameObject box in selectedBoxes)
        {
            for (int i = 0; i < comMov.boxes.Length; ++i)
            {
                if (box == comMov.boxes[i])
                {
                    selectedIdx.Add(i);
                }
            }

        }

        selectedIdx.Sort();
        selectedBoxes.Clear();

        foreach (int idx in selectedIdx)
        {
            selectedBoxes.Add(comMov.boxes[idx]);
        }
    }



    private int GetTargetIndex(GameObject clickedObj)
    {
        int ret = -1;
        int sidx = -1;

        switch (selectedBoxes.Count)
        {
            case 4:
                sidx = GetSIdx(4, clickedObj);
                break;

            case 6:
                sidx = GetSIdx(6, clickedObj);
                break;

            case 9:
                sidx = GetSIdx(9, clickedObj);
                break;
        }

        //エラーチェック
        if (sidx == -1)
        {
            Debug.Log("スワップ不可!");
            return ret;
        }

        for (int i = 0; i < comMov.boxes.Length; ++i)
        {
            if (selectedBoxes[sidx] == comMov.boxes[i])
            {
                ret = i;
                break;
            }
        }
        return ret;
    }

    //selectedBoxesでの移動先インデックスを求める
    private int GetSIdx(int count, GameObject clickedObj)
    {
        int ret = -1;
        int idx = -1;
        for (int i = 0; i < count; ++i)
        {
            if (clickedObj == selectedBoxes[i])
            {
                idx = i;
                break;
            }
        }

        ret = count - 1 - idx;

        if (ret == idx)
        {
            ret = -1;
        }
        return ret;
    }

    private void Type3Change(int idx1, int idx2, GameObject clickedObj)
    {
        //位置変更
        Vector2 tmpPos = clickedObj.transform.position;
        clickedObj.transform.position = comMov.boxes[idx2].transform.position;
        comMov.boxes[idx2].transform.position = tmpPos;

        //配列のデータを更新
        GameObject currentBox = comMov.boxes[idx1];
        comMov.boxes[idx1] = comMov.boxes[idx2];
        comMov.boxes[idx2] = currentBox;

        //selectedBoxesのサイズと配列を初期化
        DeactivateAllBoxes();
    }

    public void ChangeType3()
    {
        Debug.Log("Type3");
        comMov.getTargetIndex = GetTargetIndex;
        comMov.change = Type3Change;

        comMov.puzzleType1 = false;
        comMov.puzzleType2 = false;
        comMov.puzzleType3 = true;
    }
}
