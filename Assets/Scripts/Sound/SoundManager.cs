using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    //AudioSource�̎O��ގg�������ŉ��̏d����A�X�s�[�J�[�s�����J�o�[����B
    [HideInInspector] public AudioSource songAudioS;
    [HideInInspector] public AudioSource sepaSongAudioS;
    [HideInInspector] public static AudioSource seAudioS;
    [SerializeField] ObjectScaler objScaler;
    [SerializeField] Slider slider;
    [Header("�X�e�[�W�Ȃ�BPM")] [SerializeField] float bpm;
    [Header("�X�e�[�W�ǂ̔��q")] [SerializeField] float beats;
    [SerializeField] AudioClip seriesOfSong;

    private float samplePerBeats;
    private float[] playPoint = new float[17];
    private float scaleChangeTime = 0.2f;
    private int playPointNum;
    [HideInInspector] public bool isButtonChanging = false;
    [HideInInspector] public bool isSongFinished = false;
    private bool isSepaSongPlaying = false;
    private bool isSongPlaying = false;

    //���̃X�N���v�g�́AStart�ɂ���ĉ��ʂ��擾����̂ŁA���O��Awake�ŃR���|�[�l���g���擾����K�v������
    void Awake()
    {
        //����R���|�[�l���g��ʁX�Ɏ擾���邽�߁A�z����g�p
        AudioSource[] audioSource = GetComponents<AudioSource>();
        songAudioS = audioSource[0];
        sepaSongAudioS = audioSource[1];
        seAudioS = audioSource[2];

        songAudioS.clip = seriesOfSong;
        sepaSongAudioS.clip = seriesOfSong;

        //3���q�Ȃ̏ꍇ�A�Ȃ̒����̊֌W��6���q�ŋȂ𕪊�����
        if (beats == 3)
        {
            beats = 6;
        }

        //BPM�͈ꕪ�̂����ɑł��̉񐔂ł���A���L�̎��ɂ���āA�ꔏ�Ŏ��s�����T���v�����O�������߂���
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

        //�A�b�v�f�[�g�ŏ���������O�ɋȂ��I���ꍇ�ɔ����āA�ȏI�������O�Ŕ�����s��
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
        objScaler.ChangePlayerScale(play, stop, playScale, stopScale); //play��scale��0�ɁAstop�̃X�P�[����1��
        yield return new WaitForSeconds(scaleChangeTime);
        isButtonChanging = false;
        yield break;
    }

    public void PlayFromPoint(int playPointNum)
    {
        //timeSamples��int�^�Ȃ̂ŁAplayPoint��int�^�ɍ��킹��K�v������
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
