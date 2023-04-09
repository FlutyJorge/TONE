using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class SoundManager : MonoBehaviour
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
    [HideInInspector] public bool isButtonChanging = false;

    // Start is called before the first frame update
    void Start()
    {
        //����R���|�[�l���g��BGM�p��SE�p�ŕʁX�Ɏ擾
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

        //�X���C�_�[�ݒ�
        slider.maxValue = songAudioSource.clip.length * songAudioSource.clip.frequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSepaSongPlaying)
        {
            StopSepaSound();
        }

        slider.value = songAudioSource.timeSamples;
    }

    public IEnumerator PlayAndStopSong(EventTrigger eveTrigger, GameObject play, GameObject stop, GameObject reset)
    {
        if (eveTrigger.gameObject == play)
        {
            isButtonChanging = true;
            if (isSepaSongPlaying)
            {
                seAudioSource.Stop();
            }

            isSongPlaying = true;
            play.transform.DOScale (new Vector2 (0, 0), 0.2f);
            stop.transform.DOScale (new Vector2 (1, 1), 0.2f);
            play.GetComponent<SpriteRenderer>().DOFade(0, 0.1f);
            stop.GetComponent<SpriteRenderer>().DOFade(1, 0.1f);
            songAudioSource.Play();
            yield return new WaitForSeconds(0.2f);
            isButtonChanging = false;
            yield break;
        }

        if (eveTrigger.gameObject == stop)
        {
            isButtonChanging = true;
            play.transform.DOScale (new Vector2 (1, 1), 0.2f);
            stop.transform.DOScale (new Vector2 (0, 0), 0.2f);
            play.GetComponent<SpriteRenderer>().DOFade(1, 0.1f);
            stop.GetComponent<SpriteRenderer>().DOFade(0, 0.1f);
            songAudioSource.Pause();
            isSongPlaying = false;
            yield return new WaitForSeconds(0.2f);
            isButtonChanging = false;
            yield break;
        }

        if (eveTrigger.gameObject == reset)
        {
            isButtonChanging = true;
            play.transform.DOScale(new Vector2(1, 1), 0.2f);
            stop.transform.DOScale(new Vector2(0, 0), 0.2f);
            play.GetComponent<SpriteRenderer>().DOFade(1, 0.1f);
            stop.GetComponent<SpriteRenderer>().DOFade(0, 0.1f);
            songAudioSource.Stop();
            songAudioSource.timeSamples = 0;
            isSongPlaying = false;
            yield return new WaitForSeconds(0.2f);
            isButtonChanging = false;
            yield break;
        }
    }

    public void PlayFromPoint(int playPointNum)
    {
        //songAudioSource.Stop();
        songAudioSource.timeSamples = (int)playPoint[playPointNum];
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
            songAudioSource.Pause();
            play.transform.DOScale(new Vector2(1, 1), 0.2f);
            stop.transform.DOScale(new Vector2(0, 0), 0.2f);
            play.GetComponent<SpriteRenderer>().DOFade(1, 0.1f);
            stop.GetComponent<SpriteRenderer>().DOFade(0, 0.1f);
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
