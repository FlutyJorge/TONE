using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public Slider musicVolSider;
    public Slider SEVolSlider;

    [Header("�^�C�g���ȊO�̏ꍇ�v�A�^�b�`")]
    [SerializeField] SoundManager soundMana;
    [SerializeField] GameStarter gameSta;
    [SerializeField] Type3Movement type3Mov;

    [Space(10)]
    [Header("�^�C�g����ʂ̏ꍇ�v�A�^�b�`")]
    [SerializeField] AudioSource note0AudioS;
    [SerializeField] AudioSource note1AudioS;
    [SerializeField] AudioSource audioSFortitleSE;
    [SerializeField] NotesManager noteMana;
    public bool isTitle = false;


    // Start is called before the first frame update
    void Start()
    {
        //�V�[���ԂŃ{�����[���������p��
        if (!isTitle)
        {
            soundMana.songAudioS.volume = VolumeHolder.instance.musicVolume;
            gameSta.auSource.volume = VolumeHolder.instance.musicVolume;
            SoundManager.seAudioS.volume = VolumeHolder.instance.SEVolume;
        }
        else if (isTitle)
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
        //�V�[���J�ڒ��Ȃ珈���𖳌���
        if (SceneChanger.firstPush)
        {
            return;
        }

        if (!isTitle)
        {
            type3Mov.selectorImage.sizeDelta = Vector2.zero;
            gameSta.auSource.volume = newSliderValue;
            soundMana.songAudioS.volume = newSliderValue;
            soundMana.sepaSongAudioS.volume = newSliderValue;
        }

        if (isTitle)
        {
            for (int i = 0; i < 3; ++i)
            {
                if (noteMana.isSongPlaying[i])
                {
                    noteMana.songAudioS[i].volume = newSliderValue;
                }
            }
        }

        //�V�[���J�ڎ����{�����[����ێ��ł���悤�ɂ���
        VolumeHolder.instance.musicVolume = newSliderValue;
    }

    public void ChangeSEVolume(float newSliderValue)
    {
        //�V�[���J�ڒ��Ȃ珈���𖳌���
        if (SceneChanger.firstPush)
        {
            return;
        }

        if (!isTitle)
        {
            type3Mov.selectorImage.sizeDelta = Vector2.zero;
            SoundManager.seAudioS.volume = newSliderValue;
        }

        if (isTitle)
        {
            note0AudioS.volume = newSliderValue;
            note1AudioS.volume = newSliderValue;
            audioSFortitleSE.volume = newSliderValue;
        }

        VolumeHolder.instance.SEVolume = newSliderValue;
    }
}
