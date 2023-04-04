using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager2 : MonoBehaviour
{
    private AudioSource bgmAudioSource;
    private AudioSource seAudioSource;
    private AudioSource songAudioSource;
    [SerializeField] Slider slider;
    [SerializeField] float bpm;
    [SerializeField] float beats;

    [SerializeField] AudioClip seriesOfSong;
    [SerializeField] AudioClip[] separetedSong;

    private float samplePerBeats;
    private float[] playPoint = new float[17];
    private int playPointNum;
    private bool isSepaSongPlaying = false;
    private bool isSongPlaying = false;
    private KeyCode[] keyCode = new KeyCode[]
    {KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4,
      KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9};

    // Start is called before the first frame update
    void Start()
    {
        //同一コンポーネントをBGM用とSE用で別々に取得
        AudioSource[] audioSource = GetComponents<AudioSource>();
        bgmAudioSource = audioSource[0];
        seAudioSource = audioSource[1];
        songAudioSource = audioSource[2];

        songAudioSource.clip = seriesOfSong;
        seAudioSource.clip = seriesOfSong;
        samplePerBeats = 60 / bpm * beats * songAudioSource.clip.frequency;
        Debug.Log(samplePerBeats);

        for (int i = 0; i < playPoint.Length; ++i)
        {
            playPoint[i] = samplePerBeats * i;
            Debug.Log(playPoint[i]);
        }

        //スライダー設定
        slider.maxValue = songAudioSource.clip.length * songAudioSource.clip.frequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (isSepaSongPlaying)
            {
                seAudioSource.Stop();
            }

            isSongPlaying = true;
            songAudioSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            songAudioSource.Pause();
            isSongPlaying = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("0だ");
            songAudioSource.Stop();
            songAudioSource.timeSamples = 0;
            isSongPlaying = false;
        }

        SelectPlayPoint();
        if (isSepaSongPlaying)
        {
            StopSepaSound();
        }

        slider.value = songAudioSource.timeSamples;
    }

    private void SelectPlayPoint()
    {
        if (Input.anyKeyDown)
        {
            for (int i = 0; i < keyCode.Length; ++i)
            {
                if (Input.GetKeyDown(keyCode[i]))
                {
                    Debug.Log(i);
                    songAudioSource.Stop();
                    songAudioSource.timeSamples = (int) playPoint[i];
                    break;
                }
            }
        }
    }

    public void PlayPointButton(int playPointNum)
    {
        songAudioSource.Stop();
        songAudioSource.timeSamples = (int)playPoint[playPointNum];
    }

    private int GetSoundButtonNum(GameObject clickedObj)
    {
        int ret = int.Parse(clickedObj.name);

        return ret;
    }

    public void PlaySepaSound(GameObject clickedObj)
    {
        if (isSongPlaying)
        {
            songAudioSource.Pause();
            isSongPlaying = false;
        }

        isSepaSongPlaying = false;
        playPointNum = GetSoundButtonNum(clickedObj);

        seAudioSource.timeSamples = (int) playPoint[playPointNum];
        seAudioSource.Play();
        isSepaSongPlaying = true;
    }
    private void StopSepaSound()
    {
        if (playPointNum != 15 && seAudioSource.timeSamples > (int)playPoint[playPointNum + 1] - 1)
        {
            seAudioSource.Stop();
            isSepaSongPlaying = false;
        }
    }
}
