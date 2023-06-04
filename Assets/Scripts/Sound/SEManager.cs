using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    [Header("�^�C�g���̏ꍇ�ɃA�^�b�`���K�v")]
    [SerializeField] AudioSource audioSFortitleSE;

    //PlayOneShot�p
    public void PlayOneShot(AudioClip oneShotAudio)
    {
        SoundManager.seAudioS.PlayOneShot(oneShotAudio);
    }

    //PaintTools��Enter�p
    //�h���b�O���A�܂�ɃJ�[�\�����y�C���g�c�[���O�ɂ͂ݏo���ĉ����d�����ĂȂ�^�C�~���O�����邽�ߖh��
    public void PlayOneShotForPaintTools(AudioClip oneShotAudio)
    {
        if (!PaintToolMovement.isDraging)
        {
            SoundManager.seAudioS.PlayOneShot(oneShotAudio);
        }
    }

    //�^�C�g��SE�p
    //�^�C�g���ɂ�SoundManager���z�u����Ȃ����߁A�ʂŉ���炷�֐���p�ӂ���
    public void PlayOneShotForTitleSE(AudioClip sound)
    {
        audioSFortitleSE.PlayOneShot(sound);
    }
}
