using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BoxMovement : MonoBehaviour
{
    public GameObject[] boxes;

    //Box����ׂ��Ƃ��̏c���̐�
    public int boxWidth;
    public int boxHeight;
    [SerializeField] int suffleCount;
    [SerializeField] bool isClear = false;

    private void Start()
    {
        //�V���b�t��
        for (int i = 0; i < suffleCount; ++i)
        {
            List<GameObject> mboxes = new List<GameObject>();
            foreach (GameObject box in boxes)
            {
                //�ړ��\�ȃu���b�N
                if (GetTargetIndex(box) > -1)
                {
                    mboxes.Add(box);
                }
            }

            //��������Box���Ȃ�
            if (mboxes.Count < 1)
            {
                continue;
            }

            //�����_����1������
            int rnd = Random.Range(0, mboxes.Count);
            ChangeBox(mboxes[rnd]);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CheckClear();
        }
    }

    //�N���b�N����Box�̃C���f�b�N�X���擾
    private int GetBoxIndex(GameObject clickedObj)
    {
        int ret = -1;

        for (int i = 0; i < boxes.Length; ++i)
        {
            //�����N���b�N���ꂽBox��boxes[i]�Ɠ�����������
            if (clickedObj == boxes[i])
            {
                ret = i;
            }
        }
        return ret;
    }

    //�ړ��\Box�̃C���f�b�N�X���擾
    private int GetTargetIndex(GameObject clickedObj)
    {
        int ret = -1;

        //�����ꂽBox�̃C���f�b�N�X���擾
        int idx = GetBoxIndex(clickedObj);

        //�G���[�`�F�b�N
        if (idx < 0)
        {
            Debug.Log("�N���b�N���ꂽBox�̃C���f�b�N�X���擾�ł��ĂȂ�");
            return ret;
        }

        //�ړ��\�ꏊ��4�����Ō���
        List<int> dirs = new List<int>()
        {
            -boxHeight, //��
            +boxHeight, //��
            -1, //��
            +1, //�E
        };

        for (int i = 0; i < dirs.Count; ++i)
        {
            //4����Box�̃C���f�b�N�X
            int nidx = idx + dirs[i];

            //�z��I�[�o�[�`�F�b�N
            if (nidx < 0 || nidx > boxes.Length - 1)
            {
                continue;
            }

            //���O�ɂ�锻�ʂňړ���̃C���f�b�N�X���m��
            if (boxes[nidx].name == "15")
            {
                ret = nidx;
            }
        }

        return ret;
    }

    //Box�̓���ւ�
    public void ChangeBox(GameObject clickedObj)
    {
        //��������Box�̃C���f�b�N�X2��
        int idx1 = GetBoxIndex(clickedObj);
        int idx2 = GetTargetIndex(clickedObj);

        //�G���[�`�F�b�N
        if (clickedObj == null || idx1 < 0 || idx2 < 0)
        {
            return;
        }

        //�ʒu�ύX
        Vector2 currentPos = clickedObj.transform.position;
        clickedObj.transform.position = boxes[idx2].transform.position;
        boxes[idx2].transform.position = currentPos;

        //�z��̃f�[�^���X�V
        GameObject currentBox = boxes[idx1];
        boxes[idx1] = boxes[idx2];
        boxes[idx2] = currentBox;
    }

    //�N���A�`�F�b�N
    private void CheckClear()
    {
        if (isClear)
        {
            return;
        }

        //1�ł��C���f�b�N�X��Box�̖��O����v���Ȃ���΃t���O���I�t��
        bool readyToclear = true;
        for (int i = 0; i < boxes.Length; ++i)
        {
            if (boxes[i].name != i.ToString())
            {
                readyToclear = false;
            }
        }

        if (readyToclear)
        {
            isClear = true;
            Debug.Log("�N���A");
        }
    }
}
