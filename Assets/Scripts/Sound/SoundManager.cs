using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    //AudioSourceの三種類使い分けで音の重複や、スピーカー不足をカバーする。
    [HideInInspector] public AudioSource songAudioS;
    [HideInInspector] public AudioSource sepaSongAudioS;
    [HideInInspector] public static AudioSource seAudioS;
    [SerializeField] ObjectScaler objScaler;
    [SerializeField] Slider slider;
    [Header("ステージ曲のBPM")] [SerializeField] float bpm;
    [Header("ステージ局の拍子")] [SerializeField] float beats;
    [SerializeField] AudioClip seriesOfSong;

    private float samplePerBeats;
    private float[] playPoint = new float[17];
    private float scaleChangeTime = 0.2f;
    private int playPointNum;
    [HideInInspector] public bool isButtonChanging = false;
    [HideInInspector] public bool isSongFinished = false;
    private bool isSepaSongPlaying = false;
    private bool isSongPlaying = false;

    //他のスクリプトは、Startによって音量を取得するので、事前にAwakeでコンポーネントを取得する必要がある
    void Awake()
    {
        //同一コンポーネントを別々に取得するため、配列を使用
        AudioSource[] audioSource = GetComponents<AudioSource>();
        songAudioS = audioSource[0];
        sepaSongAudioS = audioSource[1];
        seAudioS = audioSource[2];

        songAudioS.clip = seriesOfSong;
        sepaSongAudioS.clip = seriesOfSong;

        //3拍子曲の場合、曲の長さの関係上6拍子で曲を分割する
        if (beats == 3)
        {
            beats = 6;
        }

        //BPMは一分のうちに打つ拍の回数であり、下記の式によって、一拍で実行されるサンプリング数が求められる
        samplePerBeats = 60 / bpm * beats * songAudioS.clip.frequency;

        for (int i = 0; i < playPoint.Length; ++i)
        {
            playPoint[i] = samplePerBeats * i;
        }

        slider.maxValue = songAudioS.clip.length * songAudioS.clip.frequency;
    }

    void Update()
    {
        if (isSepaSongPlaying)
        {
            StopSepaSound();
        }

        //アップデートで処理が走る前に曲が終わる場合に備えて、曲終わり一歩手前で判定を行う
        if (songAudioS.timeSamples > songAudioS.clip.length * songAudioS.clip.frequency -1)
        {
            isSongFinished = true;
            songAudioS.Stop();
            songAudioS.timeSamples = 0;
        }

        slider.value = songAudioS.timeSamples;
    }

    public void PlayAndStopSong(EventTrigger eveTrigger, GameObject play, GameObject stop, GameObject reset)
    {
        if (eveTrigger.gameObject == play)
        {
            if (isSepaSongPlaying)
            {
                sepaSongAudioS.Stop();
            }

            StartCoroutine(ChangeFlagAndScale(true, play, stop, 0, 1));
            songAudioS.Play();
        }

        if (eveTrigger.gameObject == stop)
        {
            StartCoroutine(ChangeFlagAndScale(false, play, stop, 1, 0));
            songAudioS.Pause();
        }

        if (eveTrigger.gameObject == reset)
        {
            StartCoroutine(ChangeFlagAndScale(false, play, stop, 1, 0));
            songAudioS.Stop();
            songAudioS.timeSamples = 0;
        }
    }

    public IEnumerator ChangeFlagAndScale(bool isPlaying, GameObject play, GameObject stop, int playScale, int stopScale)
    {
        isSongPlaying = isPlaying;
        isButtonChanging = true;
        objScaler.ChangePlayerScale(play, stop, playScale, stopScale); //playのscaleを0に、stopのスケールを1に
        yield return new WaitForSeconds(scaleChangeTime);
        isButtonChanging = false;
        yield break;
    }

    public void PlayFromPoint(int playPointNum)
    {
        //timeSamplesはint型なので、playPointもint型に合わせる必要がある
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

            play.transform.DOScale(new Vector2(1, 1), scaleChangeTime);
            stop.transform.DOScale(new Vector2(0, 0), scaleChangeTime);

            float fadeTime = 0.1f;
            play.GetComponent<SpriteRenderer>().DOFade(1, fadeTime);
            stop.GetComponent<SpriteRenderer>().DOFade(0, fadeTime);
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
