using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeHolder : MonoBehaviour
{
    static public VolumeHolder instance;
    public float musicVolume;
    public float SEVolume;

    //�V���O���g���ɂ��A�V�[���ύX���������ۂ����ʂ����̃N���X�ŕێ������
    //���̃X�N���v�g��Start�ł��̃X�N���v�g���Q�Ƃ���O��VolumeHolder�̃C���X�^���X���m�肳����
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
