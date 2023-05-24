using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PaintToolMovement : MonoBehaviour
{
    [SerializeField] SEManager seMana;
    public Type3Movement type3Mov;
    [SerializeField] Camera cam;
    [SerializeField] GameObject[] paintTools;
    [SerializeField] Sprite[] paintedBox;
    [SerializeField] Sprite[] paintedCircle;
    public bool isCollision = false;
    [HideInInspector] public static bool isDraging = false;

    private GameObject box;
    private SpriteRenderer spriteRen;
    private Vector3 startPos;
    private bool isDeletedSelector = false;
    private bool isCircle = false;

    [SerializeField] AudioClip colorChangeSound;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        spriteRen = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DragPaintTool()
    {
        isDraging = true;
        transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);

        //ドラッグ開始とselector描写開始にずれがあるため、ドラッグ開始と同時に描かれたselectorのサイズを0にする

        if (!isDeletedSelector)
        {
            type3Mov.startPos = type3Mov.endPos = Vector2.zero;
            type3Mov.DrawRectangle();
            isDeletedSelector = true;
        }
    }

    public void CheckCollision()
    {
        isDraging = false;
        isDeletedSelector = false;

        if (!isCollision)
        {
            transform.DOMove(startPos, 0.5f);
        }
        else if (isCollision)
        {
            spriteRen.DOFade(0, 0.5f);
            StartCoroutine(ResetPosition());
            StartCoroutine(ChangeBoxColor());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PaintToolChecker"))
        {
            box = collision.transform.parent.gameObject;
            box.transform.DOScale(new Vector2(1.1f, 1.1f), 0.1f);
            isCollision = true;
        }
        else if (collision.CompareTag("PaintToolCheckerForCircle"))
        {
            box = collision.transform.parent.gameObject;
            box.transform.DOScale(new Vector2(1.1f, 1.1f), 0.1f);
            isCollision = true;
            isCircle = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PaintToolChecker"))
        {
            box = collision.transform.parent.gameObject;
            box.transform.DOScale(new Vector2(1f, 1f), 0.1f);
            isCollision = false;
        }
        else if (collision.CompareTag("PaintToolCheckerForCircle"))
        {
            box = collision.transform.parent.gameObject;
            box.transform.DOScale(new Vector2(1f, 1f), 0.1f);
            isCollision = false;
            isCircle = false;
        }
    }

    private IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = startPos;
        spriteRen.DOFade(1, 0.5f);
    }

    private IEnumerator ChangeBoxColor()
    {
        //カラー変更時のSE
        seMana.PlayOneShot(colorChangeSound);

        SpriteRenderer boxSpriteRen = box.GetComponent<SpriteRenderer>();
        int spriteIdx = -1;

        for (int i = 0; i < paintTools.Length; ++i)
        {
            if (this.gameObject == paintTools[i])
            {
                spriteIdx = i;
                break;
            }
        }

        boxSpriteRen.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);

        if (isCircle)
        {
            boxSpriteRen.sprite = paintedCircle[spriteIdx];
            isCircle = false;
        }
        else
        {
            boxSpriteRen.sprite = paintedBox[spriteIdx];
        }
        boxSpriteRen.DOFade(1, 0.5f);
        yield break;
    }
}
