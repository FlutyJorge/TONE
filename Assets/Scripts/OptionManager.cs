using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public Slider musicVolSider;
    public Slider SEVolSlider;

    [Header("タイトル以外の場合要アタッチ")]
    [SerializeField] SoundManager soundMana;
    [SerializeField] GameStarter gameSta;
    [SerializeField] Type3Movement type3Mov;

    [Space(10)]
    [Header("タイトル画面の場合要アタッチ")]
    [SerializeField] AudioSource note0AudioS;
    [SerializeField] AudioSource note1AudioS;
    [SerializeField] AudioSource audioSFortitleSE;
    [SerializeField] NotesManager noteMana;
    public bool isTitle = false;

    void Start()
    {
        //シーン間でボリュームを引き継ぐ(Awakeで処理してはいけない)
        if (!isTitle)
        {
            soundMana.songAudioS.volume = VolumeHolder.instance.musicVolume;
            gameSta.auSource.volume = VolumeHolder.instance.musicVolume;
            SoundManager.seAudioS.volume = VolumeHolder.instance.SEVolume;
        }
        else
        {
            note0AudioS.volume = VolumeHolder.instance.SEVolume;
            note1AudioS.volume = VolumeHolder.instance.SEVolume;
            audioSFortitleSE.volume = VolumeHolder.instance.SEVolume;
        }

        musicVolSider.value = VolumeHolder.instance.musicVolume;
        SEVolSlider.value = VolumeHolder.instance.SEVolume;
    }

    public void ChangeMusicVolume(float newSliderValue)
    {
        if (SceneChanger.firstPush)
        {
            return;
        }

        if (!isTitle)
        {
            //Slider設定中にシンメトリーモードのセレクターが反応するのを防ぐ
            type3Mov.selectorImage.sizeDelta = Vector2.zero;

            gameSta.auSource.volume = newSliderValue;
            soundMana.songAudioS.volume = newSliderValue;
            soundMana.sepaSongAudioS.volume = newSliderValue;
        }
        else
        {
            for (int i = 0; i < 3; ++i)
            {
                if (noteMana.isSongPlaying[i])
                {
                    noteMana.songAudioS[i].volume = newSliderValue;
                }
            }
        }

        //シーン遷移時もボリュームを保持できるようにする
        VolumeHolder.instance.musicVolume = newSliderValue;
    }

    public void ChangeSEVolume(float newSliderValue)
    {
        if (SceneChanger.firstPush)
        {
            return;
        }

        if (!isTitle)
        {
            type3Mov.selectorImage.sizeDelta = Vector2.zero;
            SoundManager.seAudioS.volume = newSliderValue;
        }
        else
        {
            note0AudioS.volume = newSliderValue;
            note1AudioS.volume = newSliderValue;
            audioSFortitleSE.volume = newSliderValue;
        }

        VolumeHolder.instance.SEVolume = newSliderValue;
    }
}
