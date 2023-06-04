using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NoteProperty : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool canChangeSound;

    [Space(10)]
    [Header("SE")]
    [SerializeField] AudioClip buzzer;

    [Space(10)]
    [Header("Note1のみアタッチが必要")]
    [SerializeField] bool isNote1;
    [SerializeField] GameObject note1Prefab;
    [SerializeField] Sprite[] differentNote1;
    private SpriteRenderer note1Renderer;

    private float randomRotate;
    private OptionManager optionMana;
    private NotesManager notesMana;
    private AudioSource notesAudioS;
    private Vector3 randomPos;
    private Vector3 targetPos;
    private SpriteRenderer spriteRen;

    // Start is called before the first frame update
    void Start()
    {
        //NotePropertyはPrefabにアタッチするため、インスタンスが生成された直後に必要なコンポーネントを取得する
        notesMana = GameObject.FindWithTag("NotesManager").GetComponent<NotesManager>();
        notesAudioS = GetComponent<AudioSource>();
        spriteRen = GetComponent<SpriteRenderer>();
        optionMana = GameObject.Find("OptionScene").GetComponent<OptionManager>();

        //ChangeNote1Color(notesMana.playSongNum);

        //RandomRangeで指定する値は、画面内に収まる座標にしなければNoteが見切れるためこれを避ける
        randomPos = new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0);

        float rotateRange = 0.5f;
        randomRotate = Random.Range(-rotateRange, rotateRange);

        //カメラの対角線の距離は18^2 + 10^2の平方根であり、targetPosの大きさをこの数値以上にすることでNoteが画面内で停止することを防ぐ
        //平方根を計算するMathf.Sqrtは処理が重いため、計算には定数を直接用いる
        int maxDistance = 21;
        targetPos = (randomPos - new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0)).normalized * maxDistance;

        if (!isNote1)
        {
            return;
        }

        note1Renderer = note1Prefab.GetComponent<SpriteRenderer>();
        ChangeNote1Color(notesMana.playSongNum);

        if (notesMana.pushedNoteNum < notesMana.note1AuClips.Length - 1)
        {
            notesAudioS.clip = notesMana.note1AuClips[notesMana.pushedNoteNum + 1];
        }
        else
        {
            notesAudioS.clip = notesMana.note1AuClips[0];
        }

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

            switch (notesMana.playSongNum)
            {
                case 0: //1レイヤー目が再生
                    notesMana.songAudioS[0].DOFade(optionMana.musicVolSider.value, 1);
                    notesMana.isSongPlaying[0] = true;
                    break;

                case 1: //2レイヤー目が再生
                    notesMana.songAudioS[1].DOFade(optionMana.musicVolSider.value, 1);
                    notesMana.isSongPlaying[1] = true;
                    break;

                case 2: //3レイヤー目が再生
                    notesMana.songAudioS[2].DOFade(optionMana.musicVolSider.value, 1);
                    notesMana.isSongPlaying[2] = true;

                    notesMana.isplayAllSongs = true;
                    break;

                default:
                    break;
            }
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
                float fadeTime = 0.3f;
                spriteRen.DOFade(0, fadeTime).SetLink(this.gameObject);
            }
            
            Invoke("DestroyNote", 1f);
        }
    }

    private void DestroyNote()
    {
        Destroy(this.gameObject);
    }

    private void ChangeNote1Color(int playSongNum)
    {
        if (isNote1 && playSongNum >= 0)
        {
            note1Renderer.sprite = differentNote1[playSongNum];
        }
    }
}
