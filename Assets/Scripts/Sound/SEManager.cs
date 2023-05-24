using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    [Header("タイトルの場合にアタッチが必要")]
    [SerializeField] AudioSource audioSFortitleSE;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //PlayOneShot用
    public void PlayOneShot(AudioClip oneShotAudio)
    {
        SoundManager.seAudioS.PlayOneShot(oneShotAudio);
    }

    //PaintToolsのEnter用
    public void PlayOneShotForPaintTools(AudioClip oneShotAudio)
    {
        if (!PaintToolMovement.isDraging)
        {
            SoundManager.seAudioS.PlayOneShot(oneShotAudio);
        }
    }

    //タイトルSE用
    public void PlayOneShotForTitleSE(AudioClip sound)
    {
        audioSFortitleSE.PlayOneShot(sound);
    }
}
