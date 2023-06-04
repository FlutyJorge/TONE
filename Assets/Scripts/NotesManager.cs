using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesManager : MonoBehaviour
{
    [SerializeField] GameObject[] noteGenes;
    [SerializeField] GameObject note0;
    [SerializeField] GameObject note1;

    [Space(10)]
    [Header("SE")]
    public AudioClip[] note1AuClips;
    public AudioClip[] note0AuClips = new AudioClip[20];

    [HideInInspector] public AudioSource[] songAudioS;
    [HideInInspector] public int pushedNoteNum = -1;
    [HideInInspector] public int playSongNum = -1;
    [HideInInspector] public bool isInterval = false;
    [HideInInspector] public bool[] isSongPlaying = new bool[3];
    [HideInInspector] public bool isplayAllSongs = false;
    private bool canGenerate0 = true;
    private bool canGenerate1 = true;
    const int notesGeneratorNum = 16;


    private void Start()
    {
        //����R���|�[�l���g�̂��߁A�z���p���Ď擾
        AudioSource[] audioSource = GetComponents<AudioSource>();
        for (int i = 0; i < 3; ++i)
        {
            songAudioS[i] = audioSource[i];
        }
    }

    void Update()
    {
        if (!songAudioS[2].isPlaying)
        {
            for (int i = 0; i < 3; ++i)
            {
                songAudioS[i].Play();
            }
        }

        if (canGenerate0)
        {
            StartCoroutine(GenerateNotes0());
        }

        if (canGenerate1)
        {
            StartCoroutine(GenerateNotes1());
        }
    }

    //�����q(IEnumerator)��ref���g���Ȃ�����
    private IEnumerator GenerateNotes0()
    {
        canGenerate0 = false;
        int rndIndex = Random.Range(0, notesGeneratorNum); //�擾����l��0�ȏ�notesGeneratorNum����
        GameObject note0Obj = Instantiate(note0, new Vector3(noteGenes[rndIndex].transform.position.x, noteGenes[rndIndex].transform.position.y, 0), Quaternion.identity);

        AudioSource note0AudioS = note0Obj.GetComponent<AudioSource>();
        int note0SoundNum = 20;
        note0AudioS.clip = note0AuClips[Random.Range(0, note0SoundNum)];
        yield return new WaitForSeconds(Random.Range(2, 5)); //�K�؂Ȑ����C���^�[�o����݂���
        canGenerate0 = true;
        yield break;
    }

    private IEnumerator GenerateNotes1()
    {
        canGenerate1 = false;
        int rndIndex = Random.Range(0, notesGeneratorNum);
        yield return new WaitForSeconds(Random.Range(2, 5)); //�K�؂Ȑ����C���^�[�o����݂���
        Instantiate(note1, new Vector3(noteGenes[rndIndex].transform.position.x, noteGenes[rndIndex].transform.position.y, 0), Quaternion.identity);
        yield return new WaitForSeconds(5f); //�V�[���ɐ�������Ă���Note1����Ɍ�����悤�ɃC���^�[�o����ݒ肷��

        if (!isplayAllSongs)
        {
            canGenerate1 = true;
        }
        yield break;
    }
}
