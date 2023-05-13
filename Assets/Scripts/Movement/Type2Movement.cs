using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Type2Movement : MonoBehaviour
{
    public CommonMovement comMov;

    private void Awake()
    {
        comMov.type2Change = Type2Change;
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

        int nidx = idx -1;

        //nidx����]�ł���z�񂩊m�F����
        int r = idx / comMov.puzzleSize;
        int c = idx % comMov.puzzleSize;

        if (r == 0 || c == 0)
        {
            Debug.Log("�[�͉�]�ł���I");
            return ret;
        }

        for (int i = 0; i < comMov.boxes.Length; ++i)
        {
            if (nidx == i)
            {
                ret = nidx;
                return ret;
            }
        }
        Debug.Log("��]�ł���I");
        return ret;
    }

    private void Type2Change(int idx1, int idx2, GameObject clickedObj)
    {
        if (comMov.swapLimit2 == 0)
        {
            Debug.Log("�񐔏���I");
            return;
        }
        --comMov.swapLimit2;
        StartCoroutine(comMov.ChangeCounterNum(2, comMov.swapLimit2));

        //�ʒu�ύX
        Vector3 tmpPos = clickedObj.transform.position;
        Vector3 parentPos = (tmpPos + comMov.boxes[idx2 - comMov.puzzleSize].transform.position) / 2;

        comMov.parent.transform.position = parentPos;
        clickedObj.transform.SetParent(comMov.parent.transform);
        comMov.boxes[idx2].transform.SetParent(comMov.parent.transform);
        comMov.boxes[idx2 - comMov.puzzleSize].transform.SetParent(comMov.parent.transform);
        comMov.boxes[idx1 - comMov.puzzleSize].transform.SetParent(comMov.parent.transform);
        comMov.parent.transform.DORotate(new Vector3(0, 0, -90), 1f).SetRelative();
        foreach (Transform children in comMov.parent.transform)
        {
            children.transform.DOLocalRotate(new Vector3(0, 0, 90), 1f).SetRelative();
        }

        //�z��̃f�[�^���X�V
        GameObject tmpBox1 = comMov.boxes[idx1];
        GameObject tmpBox2 = comMov.boxes[idx2];
        GameObject tmpBox3 = comMov.boxes[idx2 - comMov.puzzleSize];

        comMov.boxes[idx1] = comMov.boxes[idx1 - comMov.puzzleSize];
        comMov.boxes[idx2] = tmpBox1;
        comMov.boxes[idx2 - comMov.puzzleSize] = tmpBox2;
        comMov.boxes[idx1 - comMov.puzzleSize] = tmpBox3;
    }
}
