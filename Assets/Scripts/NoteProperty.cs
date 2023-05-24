using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NoteProperty : MonoBehaviour
{
    private OptionManager optionMana;
    [SerializeField] float speed;
    [SerializeField] bool canChangeSound;
    [SerializeField] bool isNote1;
    [SerializeField] AudioClip buzzer;
    private NotesManager notesMana;
    private AudioSource notesAudioS;
    private Vector3 randomPos;
    private Vector3 targetPos;
    private float randomRotate;
    private SpriteRenderer spriteRen;

    // Start is called before the first frame update
    void Start()
    {
        notesMana = GameObject.FindWithTag("NotesManager").GetComponent<NotesManager>();
        notesAudioS = GetComponent<AudioSource>();
        spriteRen = GetComponent<SpriteRenderer>();
        optionMana = GameObject.Find("OptionScene").GetComponent<OptionManager>();


        //回転速度、進行位置を決定
        randomPos = new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0);
        randomRotate = Random.Range(-0.5f, 0.5f);
        targetPos = randomPos - new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0) * 2;

        //Note1であれば処理を行う

        if (!isNote1)
        {
            return;
        }

        if (notesMana.pushedNoteNum < notesMana.note1AuClips.Length - 1)
        {
            notesAudioS.clip = notesMana.note1AuClips[notesMana.pushedNoteNum + 1];
        }
        else
        {
            notesAudioS.clip = notesMana.note1AuClips[0];
        }

        //回転速度、進行位置を決定
        randomPos = new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0);
        randomRotate = Random.Range(-0.5f, 0.5f);
        targetPos = randomPos - new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0) * 2;

        //volumeの設定
        notesAudioS.volume = VolumeHolder.instance.SEVolume;
    }

    // Update is called once per frame
    void Update()
    {
        //移動の実行
        this.gameObject.transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        this.gameObject.transform.Rotate(0, 0, randomRotate, Space.Self);
    }

    public void ClickNote0()
    {
        canChangeSound = true;
        notesAudioS.PlayOneShot(notesAudioS.clip);
    }

    public void ClickNote1()
    {
        notesAudioS.PlayOneShot(notesAudioS.clip);
        if (notesMana.isInterval)
        {
            return;
        }
        else
        {
            notesMana.isInterval = true;
            canChangeSound = true;
            StartCoroutine(SetNoteChangeInterval());
        }
    }

    private IEnumerator SetNoteChangeInterval()
    {
        ++notesMana.pushedNoteNum;

        //最高音に達したら初期化
        if (notesMana.pushedNoteNum == 7)
        {
            notesMana.pushedNoteNum = -1;
            ++notesMana.playSongNum;
        }

        switch (notesMana.playSongNum)
        {
            case 0:
                notesMana.songAudioS[0].DOFade(optionMana.musicVolSider.value, 1);
                notesMana.isSongPlaying[0] = true;
                break;

            case 1:
                notesMana.songAudioS[1].DOFade(optionMana.musicVolSider.value, 1);
                notesMana.isSongPlaying[1] = true;
                break;

            case 2:
                notesMana.songAudioS[2].DOFade(optionMana.musicVolSider.value, 1);
                notesMana.isSongPlaying[2] = true;

                //曲が完全に再生されたらNote1の生成を中止する
                notesMana.isplayAllSongs = true;
                break;

            default:
                break;
        }
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DestroyNoteCollider"))
        {
            if (!canChangeSound && notesMana.pushedNoteNum > -1)
            {
                notesAudioS.PlayOneShot(buzzer);
                notesMana.pushedNoteNum = -1;
            }

            if (this.gameObject.CompareTag("Note1"))
            {
                notesMana.isInterval = false;
            }

            if (spriteRen != null)
            {
                spriteRen.DOFade(0, 0.5f).SetLink(this.gameObject);
            }
            
            Invoke("DestroyNote", 1f);
        }
    }

    private void DestroyNote()
    {
        Destroy(this.gameObject);
    }
}
