using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    [Header("�^�C�g���̏ꍇ�ɃA�^�b�`���K�v")]
    [SerializeField] AudioSource audioSFortitleSE;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //PlayOneShot�p
    public void PlayOneShot(AudioClip oneShotAudio)
    {
        SoundManager.seAudioS.PlayOneShot(oneShotAudio);
    }

    //PaintTools��Enter�p
    public void PlayOneShotForPaintTools(AudioClip oneShotAudio)
    {
        if (!PaintToolMovement.isDraging)
        {
            SoundManager.seAudioS.PlayOneShot(oneShotAudio);
        }
    }

    //�^�C�g��SE�p
    public void PlayOneShotForTitleSE(AudioClip sound)
    {
        audioSFortitleSE.PlayOneShot(sound);
    }
}
