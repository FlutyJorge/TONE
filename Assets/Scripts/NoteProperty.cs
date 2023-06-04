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
    [Header("Note1�̂݃A�^�b�`���K�v")]
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
        //NoteProperty��Prefab�ɃA�^�b�`���邽�߁A�C���X�^���X���������ꂽ����ɕK�v�ȃR���|�[�l���g���擾����
        notesMana = GameObject.FindWithTag("NotesManager").GetComponent<NotesManager>();
        notesAudioS = GetComponent<AudioSource>();
        spriteRen = GetComponent<SpriteRenderer>();
        optionMana = GameObject.Find("OptionScene").GetComponent<OptionManager>();

        //ChangeNote1Color(notesMana.playSongNum);

        //RandomRange�Ŏw�肷��l�́A��ʓ��Ɏ��܂���W�ɂ��Ȃ����Note�����؂�邽�߂���������
        randomPos = new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0);

        float rotateRange = 0.5f;
        randomRotate = Random.Range(-rotateRange, rotateRange);

        //�J�����̑Ίp���̋�����18^2 + 10^2�̕������ł���AtargetPos�̑傫�������̐��l�ȏ�ɂ��邱�Ƃ�Note����ʓ��Œ�~���邱�Ƃ�h��
        //���������v�Z����Mathf.Sqrt�͏������d�����߁A�v�Z�ɂ͒萔�𒼐ڗp����
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

        //volume�̐ݒ�
        notesAudioS.volume = VolumeHolder.instance.SEVolume;
    }

    // Update is called once per frame
    void Update()
    {
        //�ړ��̎��s
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

        //�ō����ɒB�����珉����
        if (notesMana.pushedNoteNum == 7)
        {
            notesMana.pushedNoteNum = -1;
            ++notesMana.playSongNum;

            switch (notesMana.playSongNum)
            {
                case 0: //1���C���[�ڂ��Đ�
                    notesMana.songAudioS[0].DOFade(optionMana.musicVolSider.value, 1);
                    notesMana.isSongPlaying[0] = true;
                    break;

                case 1: //2���C���[�ڂ��Đ�
                    notesMana.songAudioS[1].DOFade(optionMana.musicVolSider.value, 1);
                    notesMana.isSongPlaying[1] = true;
                    break;

                case 2: //3���C���[�ڂ��Đ�
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
