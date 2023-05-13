using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesManager : MonoBehaviour
{
    [SerializeField] GameObject[] noteGenes;
    [SerializeField] GameObject note0;
    [SerializeField] GameObject note1;
    public AudioClip[] noteAuClips;
    public int pushedNoteNum;
    public bool isInterval = false;

    private bool canGenerate0 = true;
    private bool canGenerate1 = true;


    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

    //”½•œŽq(IEnumerator)‚Åref‚ªŽg‚¦‚È‚©‚Á‚½
    private IEnumerator GenerateNotes0()
    {
        canGenerate0 = false;
        int rndIndex = Random.Range(0, 16);
        Instantiate(note0, new Vector3(noteGenes[rndIndex].transform.position.x, noteGenes[rndIndex].transform.position.y, 0), Quaternion.identity);
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
        yield return new WaitForSeconds(Random.Range(5, 7));
        canGenerate1 = true;
        yield break;
    }
}
