using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    //private AudioSource seAudioSource;
    [HideInInspector] public AudioSource songAudioS;
    [HideInInspector] public AudioSource sepaSongAudioS;
    [HideInInspector] public static AudioSource seAudioS;
    [SerializeField] Slider slider;
    [SerializeField] float bpm;
    [SerializeField] float beats;
    [SerializeField] ObjectScaler objScaler;

    [SerializeField] AudioClip seriesOfSong;

    private float samplePerBeats;
    private float[] playPoint = new float[17];
    private int playPointNum;
    private bool isSepaSongPlaying = false;
    private bool isSongPlaying = false;
    public bool isButtonChanging = false;

    // Start is called before the first frame update
    void Awake()
    {
        //同一コンポーネントをBGM用とSE用で別々に取得
        AudioSource[] audioSource = GetComponents<AudioSource>();
        songAudioS = audioSource[0];
        sepaSongAudioS = audioSource[1];
        seAudioS = audioSource[2];

        songAudioS.clip = seriesOfSong;
        sepaSongAudioS.clip = seriesOfSong;

        //三拍子の場合6拍子で曲を分割したい

        if (beats == 3)
        {
            beats = 6;
        }

        samplePerBeats = 60 / bpm * beats * songAudioS.clip.frequency;

        for (int i = 0; i < playPoint.Length; ++i)
        {
            playPoint[i] = samplePerBeats * i;
            Debug.Log(playPoint[i]);
        }

        //スライダー設定
        slider.maxValue = songAudioS.clip.length * songAudioS.clip.frequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSepaSongPlaying)
        {
            StopSepaSound();
        }

        slider.value = songAudioS.timeSamples;
    }

    public IEnumerator PlayAndStopSong(EventTrigger eveTrigger, GameObject play, GameObject stop, GameObject reset)
    {
        if (eveTrigger.gameObject == play)
        {
            if (isSepaSongPlaying)
            {
                sepaSongAudioS.Stop();
            }

            isSongPlaying = true;
            isButtonChanging = true;
            objScaler.ChangePlayerScale(play, stop, 0, 1);
            songAudioS.Play();
            yield return new WaitForSeconds(0.2f);
            isButtonChanging = false;
            yield break;
        }

        if (eveTrigger.gameObject == stop)
        {
            isSongPlaying = false;
            isButtonChanging = true;
            objScaler.ChangePlayerScale(play, stop, 1, 0);
            songAudioS.Pause();
            yield return new WaitForSeconds(0.2f);
            isButtonChanging = false;
            yield break;
        }

        if (eveTrigger.gameObject == reset)
        {
            isSongPlaying = false;
            isButtonChanging = true;
            objScaler.ChangePlayerScale(play, stop, 1, 0);
            songAudioS.Stop();
            songAudioS.timeSamples = 0;
            yield return new WaitForSeconds(0.2f);
            isButtonChanging = false;
            yield break;
        }
    }

    public void PlayFromPoint(int playPointNum)
    {
        //songAudioSource.Stop();
        songAudioS.timeSamples = (int)playPoint[playPointNum];
    }

    private int GetSoundButtonNum(GameObject clickedObj)
    {
        int ret = int.Parse(clickedObj.name);

        return ret;
    }

    public void PlaySepaSound(GameObject clickedObj, GameObject play, GameObject stop)
    {
        if (isSongPlaying)
        {
            songAudioS.Pause();
            play.transform.DOScale(new Vector2(1, 1), 0.2f);
            stop.transform.DOScale(new Vector2(0, 0), 0.2f);
            play.GetComponent<SpriteRenderer>().DOFade(1, 0.1f);
            stop.GetComponent<SpriteRenderer>().DOFade(0, 0.1f);
            isSongPlaying = false;
        }

        isSepaSongPlaying = false;
        playPointNum = GetSoundButtonNum(clickedObj);

        sepaSongAudioS.timeSamples = (int) playPoint[playPointNum];
        sepaSongAudioS.Play();
        isSepaSongPlaying = true;
    }
    private void StopSepaSound()
    {
        if (playPointNum != 15 && sepaSongAudioS.timeSamples > (int)playPoint[playPointNum + 1] - 1)
        {
            sepaSongAudioS.Stop();
            isSepaSongPlaying = false;
        }
    }
}
