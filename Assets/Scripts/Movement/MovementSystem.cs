using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MovementSystem : MonoBehaviour
{
    public GameObject[] boxes;

    //Box����ׂ��Ƃ��̏c���̐�
    public int puzzleSize;
    [SerializeField] int suffleCount;
    [SerializeField] bool isClear = false;
    [SerializeField] bool puzzleType1, puzzleType2; //�ǂꂩ1��True�ɂ���

    private int[] dirs;

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

        //�p�Y���̎�ޕʂɏ�����ύX
        if (puzzleType1)
        {
            Type1Directions();

            for (int i = 0; i < dirs.Length; ++i)
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
        else if (puzzleType2)
        {
            Type2Directions();
            int nidx = idx + dirs[0];

            //nidx����]�ł���z�񂩊m�F����
            for (int r = 1; r < puzzleSize; ++r)
            {
                for (int c = 0; c < puzzleSize - 1; ++c)
                {
                    if (nidx == r * puzzleSize + c)
                    {
                        ret = nidx;
                        Debug.Log(ret);
                        return ret;
                    }
                }
            }
            Debug.Log("��]�ł���I");
            return ret;
        }
        Debug.Log("�p�Y���̎�ތ��߂�I");
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

        //�p�Y���̎�ޕʂɏ�����ύX
        if (puzzleType1)
        {
            Type1Change(idx1, idx2, clickedObj);
        }
        else if (puzzleType2)
        {
            Type2Change(idx1, idx2, clickedObj);
        }
    }

    //�N���A�`�F�b�N
    private void CheckClear()
    {
        if (isClear)
        {
            return;
        }

        //1�ł��C���f�b�N�X��Box�̖��O����v���Ȃ���΃t���O���I�t��
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
            Debug.Log("�N���A");
        }
    }

    //�p�Y���̎�ޕʂɈړ�����������
    private void Type1Directions()
    {
        dirs = new int[4]
        {
            -puzzleSize, //��
            +puzzleSize, //��
            -1, //��
            +1 //�E
        };
    }

    private void Type2Directions()
    {
        dirs = new int[1]
        {
            -1 //���v���
        };
    }

    //�p�Y���̎�ޕʂɈړ�������ύs��
    private void Type1Change(int idx1, int idx2, GameObject clickedObj)
    {
        //�ʒu�ύX
        Vector2 tmpPos = clickedObj.transform.position;
        clickedObj.transform.position = boxes[idx2].transform.position;
        boxes[idx2].transform.position = tmpPos;

        //�z��̃f�[�^���X�V
        GameObject currentBox = boxes[idx1];
        boxes[idx1] = boxes[idx2];
        boxes[idx2] = currentBox;
    }

    private void Type2Change(int idx1, int idx2, GameObject clickedObj)
    {
        //�ʒu�ύX
        Vector2 tmpPos = clickedObj.transform.position;
        clickedObj.transform.position = boxes[idx2].transform.position;
        boxes[idx2].transform.position = boxes[idx2 - puzzleSize].transform.position;
        boxes[idx2 - puzzleSize].transform.position = boxes[idx1 - puzzleSize].transform.position;
        boxes[idx1 - puzzleSize].transform.position = tmpPos;

        //�z��̃f�[�^���X�V
        GameObject tmpBox1 = boxes[idx1];
        GameObject tmpBox2 = boxes[idx2];
        GameObject tmpBox3 = boxes[idx2 - puzzleSize];

        boxes[idx1] = boxes[idx1 - puzzleSize];
        boxes[idx2] = tmpBox1;
        boxes[idx2 - puzzleSize] = tmpBox2;
        boxes[idx1 - puzzleSize] = tmpBox3;
    }
}
