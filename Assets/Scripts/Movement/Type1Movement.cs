using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Type1Movement : MonoBehaviour
{
    public CommonMovement comMov;

    private int[] dirs;

    private void Awake()
    {
        comMov.getTargetIndex = GetTargetIndex;
    }

    public int GetTargetIndex(GameObject clickedObj)
    {
        int ret = -1;
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
            int nidx = idx + dirs[i];

            //�z��I�[�o�[�`�F�b�N
            if (nidx < 0 || nidx > comMov.boxes.Length - 1)
            {
                continue;
            }

            GameObject childObj = comMov.boxes[nidx].transform.GetChild(0).gameObject;

            if (childObj.tag == "PaintToolCheckerForCircle")
            {
                ret = nidx;
            }
        }
        return ret;
    }
}
