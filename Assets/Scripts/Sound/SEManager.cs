using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    [Header("タイトルの場合にアタッチが必要")]
    [SerializeField] AudioSource audioSFortitleSE;

    //PlayOneShot用
    public void PlayOneShot(AudioClip oneShotAudio)
    {
        SoundManager.seAudioS.PlayOneShot(oneShotAudio);
    }

    //PaintToolsのEnter用
    //ドラッグ中、まれにカーソルがペイントツール外にはみ出して音が重複してなるタイミングがあるため防ぐ
    public void PlayOneShotForPaintTools(AudioClip oneShotAudio)
    {
        if (!PaintToolMovement.isDraging)
        {
            SoundManager.seAudioS.PlayOneShot(oneShotAudio);
        }
    }

    //タイトルSE用
    //タイトルにはSoundManagerが配置されないため、個別で音を鳴らす関数を用意する
    public void PlayOneShotForTitleSE(AudioClip sound)
    {
        audioSFortitleSE.PlayOneShot(sound);
    }
}
