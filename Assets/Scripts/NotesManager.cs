using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesManager : MonoBehaviour
{
    [SerializeField] GameObject[] noteGenes;
    [SerializeField] GameObject note0;
    [SerializeField] GameObject note1;
    public AudioClip[] note1AuClips;
    public AudioClip[] note0AuClips = new AudioClip[20];
    public AudioSource[] songAudioS;
    public int pushedNoteNum = -1;
    public bool isInterval = false;
    public int playSongNum = -1;
    [HideInInspector] public bool[] isSongPlaying = new bool[3];

    [HideInInspector] public bool isplayAllSongs = false;
    private bool canGenerate0 = true;
    private bool canGenerate1 = true;


    private void Start()
    {
        AudioSource[] audioSource = GetComponents<AudioSource>();
        for (int i = 0; i < 3; ++i)
        {
            songAudioS[i] = audioSource[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //曲の再生タイミングを合わせる
        if (!songAudioS[2].isPlaying)
        {
            for (int i = 0; i < 3; ++i)
            {
                songAudioS[i].Play();
            }
        }

        if (!canGenerate0)
        {
            return;
        }
        else
        {
            StartCoroutine(GenerateNotes0());
        }

        if (!canGenerate1)
        {
            return;
        }
        else
        {
            StartCoroutine(GenerateNotes1());
        }
    }

    //反復子(IEnumerator)でrefが使えなかった
    private IEnumerator GenerateNotes0()
    {
        canGenerate0 = false;
        int rndIndex = Random.Range(0, 16);
        GameObject note0Obj = Instantiate(note0, new Vector3(noteGenes[rndIndex].transform.position.x, noteGenes[rndIndex].transform.position.y, 0), Quaternion.identity);
        AudioSource note0AudioS = note0Obj.GetComponent<AudioSource>();
        note0AudioS.clip = note0AuClips[Random.Range(1, 20)];
        yield return new WaitForSeconds(Random.Range(2, 5));
        canGenerate0 = true;
        yield break;
    }

    private IEnumerator GenerateNotes1()
    {
        canGenerate1 = false;
        int rndIndex = Random.Range(0, 16);
        yield return new WaitForSeconds(Random.Range(2, 5));
        Instantiate(note1, new Vector3(noteGenes[rndIndex].transform.position.x, noteGenes[rndIndex].transform.position.y, 0), Quaternion.identity);
        yield return new WaitForSeconds(5f);

        if (!isplayAllSongs)
        {
            canGenerate1 = true;
        }
        yield break;
    }
}
