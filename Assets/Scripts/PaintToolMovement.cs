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

    [HideInInspector] public bool isCollision = false;
    //ObjectScalerの引数が一つしか指定できない関数で使用するため、引数や参照なしでアクセスできるようにする
    [HideInInspector] public static bool isDraging = false;
    private GameObject obj; //BoxもしくはCircleが格納される
    private SpriteRenderer spriteRen;
    private Vector3 startPos;
    private bool isDeletedSelector = false;
    private bool isCircle = false;
    const float moveTime = 0.5f;
    const float fadeTime = 0.5f;
    const float scaleChangeTime = 0.1f;

    [Space(10)]
    [Header("SE")]
    [SerializeField] AudioClip colorChangeSound;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        spriteRen = GetComponent<SpriteRenderer>();
    }

    public void DragPaintTool()
    {
        isDraging = true;
        transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);

        //ドラッグ開始とselector描写開始にずれがあるため、ドラッグ開始と同時にわずかに反応するselectorのサイズを0にする
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
            transform.DOMove(startPos, moveTime);
        }
        else if (isCollision)
        {
            spriteRen.DOFade(0, fadeTime);
            StartCoroutine(ResetPosition());
            StartCoroutine(ChangeBoxColor());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PaintToolChecker"))
        {
            ChangeBoxOrCircleScale(collision.transform.parent.gameObject, 1.1f, true);
        }
        else if (collision.CompareTag("PaintToolCheckerForCircle"))
        {
            ChangeBoxOrCircleScale(collision.transform.parent.gameObject, 1.1f, true);
            isCircle = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PaintToolChecker"))
        {
            ChangeBoxOrCircleScale(collision.transform.parent.gameObject, 1, false);
        }
        else if (collision.CompareTag("PaintToolCheckerForCircle"))
        {
            ChangeBoxOrCircleScale(collision.transform.parent.gameObject, 1, false);
            isCircle = false;
        }
    }

    private void ChangeBoxOrCircleScale(GameObject collisionObject, float size, bool judgeCollision)
    {
        obj = collisionObject;
        obj.transform.DOScale(new Vector2(size, size), scaleChangeTime);
        isCollision = judgeCollision;
    }

    private IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(fadeTime);
        transform.position = startPos;
        spriteRen.DOFade(1, fadeTime);
    }

    private IEnumerator ChangeBoxColor()
    {
        seMana.PlayOneShot(colorChangeSound);

        SpriteRenderer boxSpriteRen = obj.GetComponent<SpriteRenderer>();
        int spriteIdx = -1;

        for (int i = 0; i < paintTools.Length; ++i)
        {
            if (this.gameObject == paintTools[i])
            {
                spriteIdx = i;
                break;
            }
        }

        boxSpriteRen.DOFade(0, fadeTime);
        yield return new WaitForSeconds(fadeTime);

        if (isCircle)
        {
            boxSpriteRen.sprite = paintedCircle[spriteIdx];
            isCircle = false;
        }
        else
        {
            boxSpriteRen.sprite = paintedBox[spriteIdx];
        }
        boxSpriteRen.DOFade(1, fadeTime);
        yield break;
    }
}
