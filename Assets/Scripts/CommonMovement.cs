using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommonMovement : MonoBehaviour
{
    public GameObject[] boxes;

    //このスクリプトからType系へのへの参照を避ける
    public delegate int Delegate1(GameObject clickedObj);
    public Delegate1 getTargetIndex;
    public delegate void Delegate2(int idx1, int idx2, GameObject clickedObj);
    public Delegate2 change;

    public int puzzleSize;     //Boxを並べたときの縦横の数
    [SerializeField] int suffleCount;
    [SerializeField] bool isClear = false;
    [SerializeField] bool puzzleType1, puzzleType2;

    private void Start()
    {
        Suffle(getTargetIndex);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CheckClear();
        }
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
        //交換するBoxのインデックス2つ
        int idx1 = GetBoxIndex(clickedObj);
        int idx2 = getTargetIndex(clickedObj);

        //エラーチェック
        if (clickedObj == null || idx1 < 0 || idx2 < 0)
        {
            return;
        }

        change(idx1, idx2, clickedObj);
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
            //ChangeBox(mboxes[rnd]);
            ChangeBox(mboxes[rnd]);
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
}
