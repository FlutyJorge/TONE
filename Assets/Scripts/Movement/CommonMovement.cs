using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CommonMovement : MonoBehaviour
{
    public GameObject[] boxes;
    public GameObject parent;
    public bool swaping = false;
    public int swapLimit1, swapLimit2, swapLimit3;

    //���̃X�N���v�g����Type�n�ւ̂ւ̎Q�Ƃ������
    public delegate int Delegate1(GameObject clickedObj);
    public Delegate1 getTargetIndex;
    public delegate void Delegate2(int idx1, int idx2, GameObject clickedObj);
    public Delegate2 type2Change;

    public int puzzleSize; //Box����ׂ��Ƃ��̏c���̐�
    [SerializeField] int suffleCount;
    [SerializeField] bool isClear = false;
    public bool puzzleType1, puzzleType2, puzzleType3;

    private void Start()
    {
        //Suffle(getTargetIndex);
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
        swaping = true;
        //��������Box�̃C���f�b�N�X2��
        int idx1 = GetBoxIndex(clickedObj);
        int idx2 = getTargetIndex(clickedObj);

        //�G���[�`�F�b�N
        if (clickedObj == null || idx1 < 0 || idx2 < 0)
        {
            Debug.Log("�G���[");
            swaping = false;
            return;
        }

        if (puzzleType2)
        {
            type2Change(idx1, idx2, clickedObj);
        }
        else
        {
            Type1And3Change(idx1, idx2, clickedObj);
        }
        StartCoroutine(DetachChildren(parent));
    }

    //Type1��Type3�̃X���b�v�����͓���̂��߁A���̃N���X�ł܂Ƃ߂ď������L�q����
    private void Type1And3Change(int idx1, int idx2, GameObject clickedObj)
    {
        if (puzzleType1)
        {
            if (swapLimit1 == 0)
            {
                Debug.Log("�񐔏���I");
                return;
            }
            --swapLimit1;
        }
        else if (puzzleType3)
        {
            if (swapLimit3 == 0)
            {
                Debug.Log("�񐔏���I");
                return;
            }
            --swapLimit3;
        }
        else
        {
            Debug.Log("puzzletype3�ɂȂ��Ă�");
        }

        //�ʒu�ύX
        Vector3 tmpPos = clickedObj.transform.position;
        Vector3 parentPos = (tmpPos + boxes[idx2].transform.position) / 2;

        parent.transform.position = parentPos;
        clickedObj.transform.SetParent(parent.transform);
        boxes[idx2].transform.SetParent(parent.transform);
        parent.transform.DORotate(new Vector3(0, 0, -180), 1f).SetRelative();
        foreach (Transform children in parent.transform)
        {
            children.transform.DOLocalRotate(new Vector3(0, 0, 180), 1f).SetRelative();
        }

        //�z��̃f�[�^���X�V
        GameObject currentBox = boxes[idx1];
        boxes[idx1] = boxes[idx2];
        boxes[idx2] = currentBox;
    }

    //�X���b�v�I�����ɐe�q�֌W����
    public IEnumerator DetachChildren(GameObject parent)
    {
        yield return new WaitForSeconds(1f);
        parent.transform.DetachChildren();
        swaping = false;
        yield break;
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
                break;
            }
        }

        if (canclear)
        {
            isClear = true;
            Debug.Log("�N���A");
        }
    }
}
