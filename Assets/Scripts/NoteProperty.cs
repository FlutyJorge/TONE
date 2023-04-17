using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteProperty : MonoBehaviour
{
    [SerializeField] float speed;
    private NotesManager notesMana;

    private Vector3 randomPos;
    private Vector3 targetPos;
    private float randomRotate;
    private AudioSource noteAuSource;

    // Start is called before the first frame update
    void Start()
    {
        notesMana = GameObject.FindWithTag("NotesManager").GetComponent<NotesManager>();
        noteAuSource = GetComponent<AudioSource>();

        if (notesMana.pushedNoteNum < notesMana.noteAuClips.Length - 1)
        {
            noteAuSource.clip = notesMana.noteAuClips[notesMana.pushedNoteNum + 1];
        }
        else
        {
            noteAuSource.clip = notesMana.noteAuClips[0];
        }

        //回転速度、進行位置を決定
        randomPos = new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0);
        randomRotate = Random.Range(-0.5f, 0.5f);
        targetPos = randomPos - new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0) * 2;
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
        noteAuSource.PlayOneShot(notesMana.noteAuClips[notesMana.pushedNoteNum]);
    }

    public void ClickNote1()
    {
        noteAuSource.PlayOneShot(noteAuSource.clip);
        if (notesMana.isInterval)
        {
            return;
        }
        else
        {
            notesMana.isInterval = true;
            StartCoroutine(SetNoteChangeInterval());
        }
    }

    private IEnumerator SetNoteChangeInterval()
    {
        ++notesMana.pushedNoteNum;

        //最高音に達したら初期化
        if (notesMana.pushedNoteNum == 7)
        {
            notesMana.pushedNoteNum = 0;
        }

        yield return new WaitForSeconds (9f);
        notesMana.isInterval = false;
        yield break;
    }
}
