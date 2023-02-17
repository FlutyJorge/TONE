using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommonMovement : MonoBehaviour
{
    public GameObject[] boxes;

    //���̃X�N���v�g����Type�n�ւ̂ւ̎Q�Ƃ������
    public delegate int Delegate1(GameObject clickedObj);
    public Delegate1 getTargetIndex;
    public delegate void Delegate2(int idx1, int idx2, GameObject clickedObj);
    public Delegate2 change;

    public int puzzleSize;     //Box����ׂ��Ƃ��̏c���̐�
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

    //�N���b�N����Box�̃C���f�b�N�X���擾
    public int GetBoxIndex(GameObject clickedObj)
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

    //Box�̓���ւ�
    public void ChangeBox(GameObject clickedObj)
    {
        //��������Box�̃C���f�b�N�X2��
        int idx1 = GetBoxIndex(clickedObj);
        int idx2 = getTargetIndex(clickedObj);

        //�G���[�`�F�b�N
        if (clickedObj == null || idx1 < 0 || idx2 < 0)
        {
            return;
        }

        change(idx1, idx2, clickedObj);
    }

    //�V���b�t��(�{�Ԃ͕s�v)
    public void Suffle(Delegate1 getTargetIndex)
    {
        //�V���b�t��
        for (int i = 0; i < suffleCount; ++i)
        {
            List<GameObject> mboxes = new List<GameObject>();
            foreach (GameObject box in boxes)
            {
                //�ړ��\�ȃu���b�N
                if (getTargetIndex(box) > -1)
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
            //ChangeBox(mboxes[rnd]);
            ChangeBox(mboxes[rnd]);
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
}
