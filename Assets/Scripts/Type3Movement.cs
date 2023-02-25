using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type3Movement : MonoBehaviour
{
    public RectTransform selectorImage; //�͈͑I�����̉摜
    public CommonMovement comMov;
    public List<GameObject> selectedBoxes = new List<GameObject>(); //�I�𒆂�Box������
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
        if (Input.GetMouseButtonDown(0) && !canType3Swap�@&& comMov.puzzleType3)
        {
            startPos = Input.mousePosition;
            DeactivateAllBoxes();

            selectionRect = new Rect();
        }

        if (Input.GetMouseButton(0) && !canType3Swap && comMov.puzzleType3)
        {
            endPos = Input.mousePosition;
            DrawRectangle();

            //�v�ZX
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

            //�v�ZY
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

                //���X�g�̃\�[�g
                SortSelectedBoxes();
            }
            else
            {
                canType3Swap = false;
            }
        }
    }

    //Box�͈̔͑I���摜�̑傫�����}�E�X�ʒu�ɍ��킹�ĕύX
    private void DrawRectangle()
    {
        //�摜�̒��S���v�Z
        Vector2 boxStart = startPos;
        Vector2 center = (boxStart + endPos) / 2;

        selectorImage.position = center;

        //�摜�̃T�C�Y���v�Z
        float sizeX = Mathf.Abs(boxStart.x - endPos.x);
        float sizeY = Mathf.Abs(boxStart.y - endPos.y);

        selectorImage.sizeDelta = new Vector2(sizeX, sizeY);
    }

    //�I�����ꂽBox�̃X�P�[����傫������
    private void CheckSelectedBox()
    {
        foreach (GameObject box in comMov.boxes)
        {
            if (selectionRect.Contains(cam.WorldToScreenPoint(box.transform.position)))
            {
                selectedBoxes.Add(box);
            }
        }

        //Box�̌���4�ȏ�9�ȉ�����8�ȊO�łȂ��ƑI���͕s��
        if (selectedBoxes.Count < 4 || selectedBoxes.Count > 9 || selectedBoxes.Count == 8 || IsOneRowOneColum())
        {
            selectedBoxes.Clear();
            Debug.Log("�I��s��!");
        }
        else
        {
            foreach (GameObject box in selectedBoxes)
            {
                box.transform.localScale = new Vector2(1.7f, 1.7f);
            }

            //�X���b�v���\��
            canType3Swap = true;
        }
    }

    //�I������Box�Q���c�����łȂ����`�F�b�N
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

        //�c���`�F�b�N
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

    //�I�����ꂽBox�̏�����
    private void DeactivateAllBoxes()
    {
        foreach (GameObject box in selectedBoxes)
        {
            box.transform.localScale = new Vector2(1.5f, 1.5f);
        }
        selectedBoxes.Clear();
    }

    //�I�����ꂽBox���C���f�b�N�X�Ń\�[�g����
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

        //�G���[�`�F�b�N
        if (sidx == -1)
        {
            Debug.Log("�X���b�v�s��!");
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

    //selectedBoxes�ł̈ړ���C���f�b�N�X�����߂�
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
        //�ʒu�ύX
        Vector2 tmpPos = clickedObj.transform.position;
        clickedObj.transform.position = comMov.boxes[idx2].transform.position;
        comMov.boxes[idx2].transform.position = tmpPos;

        //�z��̃f�[�^���X�V
        GameObject currentBox = comMov.boxes[idx1];
        comMov.boxes[idx1] = comMov.boxes[idx2];
        comMov.boxes[idx2] = currentBox;

        //selectedBoxes�̃T�C�Y�Ɣz���������
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
