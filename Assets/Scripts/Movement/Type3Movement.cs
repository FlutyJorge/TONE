using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Type3Movement : MonoBehaviour
{
    [SerializeField] CommonMovement comMov;
    [SerializeField] SEManager seMana;
    [SerializeField] OptionClick optionClick;
    [SerializeField] ClearManager clearMana;
    [SerializeField] FailedManager faildMana;
    public RectTransform selectorImage; //�͈͑I�����̉摜

    [Space(10)]
    [Header("SE")]
    [SerializeField] AudioClip selectSound;
    [HideInInspector] public List<GameObject> selectedBoxes = new List<GameObject>(); //�I�𒆂�Box������
    [HideInInspector] public bool canType3Swap = false;
    [HideInInspector] public bool canResetBoxSize = true;
    [HideInInspector] public Vector2 startPos, endPos;
    private float scaleChangeTime = 0.2f;
    private Camera cam;
    private Rect selectionRect;

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
            //�I�v�V������ʁA�N���A��ʁAFailed��ʁAPaintTool�h���b�O���̓Z���N�^�[���o�Ă͂����Ȃ�
            if (optionClick.isOptionAppeared || PaintToolMovement.isDraging)
            {
                return;
            }
            else if (clearMana != null && clearMana.isJudging)
            {
                return;
            }
            else if (faildMana != null && faildMana.isStartedShowFailedBoard)
            {
                return;
            }

            endPos = Input.mousePosition;

            DrawRectangle();

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
                SortSelectedBoxes();
            }
            else if (canResetBoxSize)
            {
                canType3Swap = false;
                DeactivateAllBoxes();
            }
        }
    }

    public void DrawRectangle()
    {
        Vector2 boxStart = startPos;
        Vector2 center = (boxStart + endPos) / 2;

        selectorImage.position = center;

        float sizeX = Mathf.Abs(boxStart.x - endPos.x);
        float sizeY = Mathf.Abs(boxStart.y - endPos.y);

        selectorImage.sizeDelta = new Vector2(sizeX, sizeY);
    }

    private void CheckSelectedBox()
    {
        foreach (GameObject box in comMov.boxes)
        {
            if (selectionRect.Contains(cam.WorldToScreenPoint(box.transform.position)))
            {
                selectedBoxes.Add(box);
            }
        }

        //Box�̌���4�ȏ�9�ȉ�����8�ȊO�łȂ��ƑI���͕s�A�c����񂾂����s��
        if (selectedBoxes.Count < 4 || selectedBoxes.Count > 9 || selectedBoxes.Count == 8 || IsOneRowOneColum())
        {
            selectedBoxes.Clear();
        }
        else
        {
            float boxScale = 1.2f;
            foreach (GameObject box in selectedBoxes)
            {
                box.transform.DOScale(new Vector2(boxScale, boxScale), scaleChangeTime);
            }

            canType3Swap = true;
            seMana.PlayOneShot(selectSound);
        }
    }

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

        if (selectedBoxes[3] != comMov.boxes[idx + 3])
        {
            rowRet = false;
        }

        if (selectedBoxes[0] != comMov.boxes[12] && selectedBoxes[1] != comMov.boxes[idx + comMov.puzzleSize])
        {
            columnRet = false;
        }

        if (!rowRet && !columnRet)
        {
            ret = false;
        }
        return ret;
    }

    public void DeactivateAllBoxes()
    {
        int boxNormalScale = 1;
        foreach (GameObject box in selectedBoxes)
        {
            box.transform.DOScale(new Vector2(boxNormalScale, boxNormalScale), scaleChangeTime);
        }
        selectedBoxes.Clear();
    }

    //�I�u�W�F�N�g�̖��O���r���ă\�[�g���邽�߁A���g�Ŋ֐��������Sort���s��
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

        //Sort���邽�߂ɂ͈�x���g����ɂ��āA�Ăѓ���Ȃ���
        selectedBoxes.Clear();
        foreach (int idx in selectedIdx)
        {
            selectedBoxes.Add(comMov.boxes[idx]);
        }
    }



    public int GetTargetIndex(GameObject clickedObj)
    {
        int ret = -1;
        int sidx = -1;

        switch (selectedBoxes.Count)
        {
            //�I�������Box�̐���4, 6, 9�Ɍ����邽�߁A������ԗ��ł���
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
}
