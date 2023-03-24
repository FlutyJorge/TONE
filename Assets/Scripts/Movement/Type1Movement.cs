using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Type1Movement : MonoBehaviour
{
    public CommonMovement comMov;
    [SerializeField] GameObject parent;

    private int[] dirs;

    //�����̈ړ��^�C�v��ݒ�
    private void Awake()
    {
        ChangeToType1();
    }

    //�ړ��\Box�̃C���f�b�N�X���擾
    private int GetTargetIndex(GameObject clickedObj)
    {
        int ret = -1;

        //�����ꂽBox�̃C���f�b�N�X���擾
        int idx = comMov.GetBoxIndex(clickedObj);

        //�G���[�`�F�b�N
        if (idx < 0)
        {
            Debug.Log("�N���b�N���ꂽBox�̃C���f�b�N�X���擾�ł��ĂȂ�");
            return ret;
        }

        dirs = new int[4]
        {
            -comMov.puzzleSize, //��
            +comMov.puzzleSize, //��
            -1, //��
            +1 //�E
        };

        for (int i = 0; i < dirs.Length; ++i)
        {
            //4����Box�̃C���f�b�N�X
            int nidx = idx + dirs[i];

            //�z��I�[�o�[�`�F�b�N
            if (nidx < 0 || nidx > comMov.boxes.Length - 1)
            {
                continue;
            }

            //���O�ɂ�锻�ʂňړ���̃C���f�b�N�X���m��
            if (comMov.boxes[nidx].name == "15")
            {
                ret = nidx;
            }
        }

        return ret;
    }

    private void Type1Change(int idx1, int idx2, GameObject clickedObj)
    {
        //�ʒu�ύX
        Vector3 tmpPos = clickedObj.transform.position;
        Vector3 parentPos = (tmpPos + comMov.boxes[idx2].transform.position) / 2;
        /*clickedObj.transform.DOMove(comMov.boxes[idx2].transform.position, 1f);
        comMov.boxes[idx2].transform.DOMove(tmpPos, 1);*/
        comMov.parent.transform.position = parentPos;
        clickedObj.transform.SetParent(comMov.parent.transform);
        comMov.boxes[idx2].transform.SetParent(comMov.parent.transform);
        comMov.parent.transform.DORotate(new Vector3(0, 0, -180), 1f).SetRelative();
        foreach (Transform children in comMov.parent.transform)
        {
            children.transform.DOLocalRotate(new Vector3(0, 0, 180), 1f).SetRelative();
        }

        //�z��̃f�[�^���X�V
        GameObject currentBox = comMov.boxes[idx1];
        comMov.boxes[idx1] = comMov.boxes[idx2];
        comMov.boxes[idx2] = currentBox;
    }

    public void ChangeToType1()
    {
        Debug.Log("Type1");
        comMov.getTargetIndex = GetTargetIndex;
        comMov.change = Type1Change;

        comMov.puzzleType1 = true;
        comMov.puzzleType2 = false;
        comMov.puzzleType3 = false;

        foreach (GameObject box in comMov.boxes)
        {
            box.transform.localScale = new Vector2(1.5f, 1.5f);
        }
    }
}
