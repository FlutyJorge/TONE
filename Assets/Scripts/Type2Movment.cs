using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type2Movment : MonoBehaviour
{
    public CommonMovement comMov;

    private int dir;

    private void Update()
    {
        if (Input.GetKeyDown("2"))
        {
            ChangeType2();
        }
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

        dir = -1; //���v���
        int nidx = idx + dir;

        //nidx����]�ł���z�񂩊m�F����
        for (int r = 1; r < comMov.puzzleSize; ++r)
        {
            for (int c = 0; c < comMov.puzzleSize - 1; ++c)
            {
                if (nidx == r * comMov.puzzleSize + c)
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

    private void Type2Change(int idx1, int idx2, GameObject clickedObj)
    {
        //�ʒu�ύX
        Vector2 tmpPos = clickedObj.transform.position;
        clickedObj.transform.position = comMov.boxes[idx2].transform.position;
        comMov.boxes[idx2].transform.position = comMov.boxes[idx2 - comMov.puzzleSize].transform.position;
        comMov.boxes[idx2 - comMov.puzzleSize].transform.position = comMov.boxes[idx1 - comMov.puzzleSize].transform.position;
        comMov.boxes[idx1 - comMov.puzzleSize].transform.position = tmpPos;

        //�z��̃f�[�^���X�V
        GameObject tmpBox1 = comMov.boxes[idx1];
        GameObject tmpBox2 = comMov.boxes[idx2];
        GameObject tmpBox3 = comMov.boxes[idx2 - comMov.puzzleSize];

        comMov.boxes[idx1] = comMov.boxes[idx1 - comMov.puzzleSize];
        comMov.boxes[idx2] = tmpBox1;
        comMov.boxes[idx2 - comMov.puzzleSize] = tmpBox2;
        comMov.boxes[idx1 - comMov.puzzleSize] = tmpBox3;
    }

    private void ChangeType2()
    {
        comMov.getTargetIndex = GetTargetIndex;
        comMov.change = Type2Change;
    }
}
