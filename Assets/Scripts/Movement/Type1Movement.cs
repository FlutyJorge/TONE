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
        comMov.getTargetIndex = GetTargetIndex;
    }

    //�ړ��\Box�̃C���f�b�N�X���擾
    public int GetTargetIndex(GameObject clickedObj)
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
}
