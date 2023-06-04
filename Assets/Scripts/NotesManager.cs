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
        //同一コンポーネントのため、配列を用いて取得
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

    //反復子(IEnumerator)でrefが使えなかった
    private IEnumerator GenerateNotes0()
    {
        canGenerate0 = false;
        int rndIndex = Random.Range(0, notesGeneratorNum); //取得する値は0以上notesGeneratorNum未満
        GameObject note0Obj = Instantiate(note0, new Vector3(noteGenes[rndIndex].transform.position.x, noteGenes[rndIndex].transform.position.y, 0), Quaternion.identity);

        AudioSource note0AudioS = note0Obj.GetComponent<AudioSource>();
        int note0SoundNum = 20;
        note0AudioS.clip = note0AuClips[Random.Range(0, note0SoundNum)];
        yield return new WaitForSeconds(Random.Range(2, 5)); //適切な生成インターバルを設ける
        canGenerate0 = true;
        yield break;
    }

    private IEnumerator GenerateNotes1()
    {
        canGenerate1 = false;
        int rndIndex = Random.Range(0, notesGeneratorNum);
        yield return new WaitForSeconds(Random.Range(2, 5)); //適切な生成インターバルを設ける
        Instantiate(note1, new Vector3(noteGenes[rndIndex].transform.position.x, noteGenes[rndIndex].transform.position.y, 0), Quaternion.identity);
        yield return new WaitForSeconds(5f); //シーンに生成されているNote1が一つに限られるようにインターバルを設定する

        if (!isplayAllSongs)
        {
            canGenerate1 = true;
        }
        yield break;
    }
}
