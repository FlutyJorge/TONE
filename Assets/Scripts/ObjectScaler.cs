using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ObjectScaler : MonoBehaviour
{
    [HideInInspector] public bool isScaleChanging = false;

    private EventTrigger eveTrigger;
    private bool isClicked = false;
    private float scaleChangeTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        eveTrigger = GetComponent<EventTrigger>();
    }

    //スケーリングに制約がない場合用
    public void ChangeScale(float size)
    {
        eveTrigger.gameObject.transform.DOScale(new Vector2(size, size), scaleChangeTime);
    }

    //クリック中はEnterとExitを反応させない場合
    public void ChangeScaleForEnterAndExit(float size)
    {
        if (!isClicked)
        {
            eveTrigger.gameObject.transform.DOScale(new Vector2(size, size), scaleChangeTime);
        }
    }

    //クリック中のスケール変更(共通用)
    public void ChangeScaleForPointerUpAndDown(float size)
    {
        StartCoroutine(ChangeScaleSet(size));
    }

    private IEnumerator ChangeScaleSet(float size)
    {
        isClicked = true;
        eveTrigger.gameObject.transform.DOScale(new Vector2(size, size), scaleChangeTime);
        yield return new WaitForSeconds(scaleChangeTime);
        isClicked = false;
        yield break;
    }

    //TypeChanger用
    public void ChangeTypeChangerScaleForEnter(TypeChanger typeChanger)
    {
        if (!typeChanger.isTypeChanging)
        {
            eveTrigger.gameObject.transform.DOScale(new Vector2(1.1f, 1.1f), scaleChangeTime);
        }
    }

    public void ChangeTypeChangerScaleForExit(TypeChanger typeChanger)
    {
        if (!typeChanger.isTypeChanging)
        {
            eveTrigger.gameObject.transform.DOScale(new Vector2(1f, 1f), scaleChangeTime);
        }
    }

    //Box用のスケーリング
    public void ChangeBoxScaleForEnter(Type3Movement type3Mov)
    {
        if (!type3Mov.canType3Swap)
        {
            eveTrigger.gameObject.transform.DOScale(new Vector2(1.1f, 1.1f), scaleChangeTime);
        }
    }

    public void ChangeBoxScaleForExit(Type3Movement type3Mov)
    {
        if (!type3Mov.canType3Swap)
        {
            eveTrigger.gameObject.transform.DOScale(new Vector2(1f, 1f), scaleChangeTime);
        }
    }

    //Type変更時のスケーリング
    public IEnumerator ChangeTypeButtonScale(TypeChanger typeChanger, CommonMovement comMov, GameObject[] buttons, GameObject[] pushedButtons, int num)
    {
        float scaleChangeTime = 0.2f;

        if (comMov.puzzleType1)
        {
            ChangeTypeButtonScaleSet(buttons, pushedButtons, 0, scaleChangeTime);
        }
        else if (comMov.puzzleType2)
        {
            ChangeTypeButtonScaleSet(buttons, pushedButtons, 1, scaleChangeTime);
        }
        else
        {
            ChangeTypeButtonScaleSet(buttons, pushedButtons, 2, scaleChangeTime);
        }

        buttons[num].transform.DOScale(new Vector3(0, 0, 0), scaleChangeTime);
        pushedButtons[num].transform.DOScale(new Vector3(1f, 1f, 1f), scaleChangeTime);

        yield return new WaitForSeconds(scaleChangeTime);
        typeChanger.isTypeChanging = false;
        yield break;
    }

    private void ChangeTypeButtonScaleSet(GameObject[] buttons, GameObject[] pushedButtons, int buttonIndex, float scaleChangeTime)
    {
        buttons[buttonIndex].transform.DOScale(new Vector2(1f, 1f), scaleChangeTime);
        pushedButtons[buttonIndex].transform.DOScale(new Vector2(0, 0), scaleChangeTime);
    }

    //player用
    public void ChangePlayerScale(GameObject play, GameObject stop, int playNum, int stopNum)
    {
        float scaleChangeTime = 0.2f;
        float fadeTime = 0.1f;

        play.transform.DOScale(new Vector2(playNum, playNum), scaleChangeTime);
        stop.transform.DOScale(new Vector2(stopNum, stopNum), scaleChangeTime);
        play.GetComponent<SpriteRenderer>().DOFade(playNum, fadeTime);
        stop.GetComponent<SpriteRenderer>().DOFade(stopNum, fadeTime);
    }

    public void ChangePlayerButtonScaleForEnter(SoundManager sManager)
    {
        float scaleChangeTime = 0.2f;

        if (!sManager.isButtonChanging)
        {
            eveTrigger.gameObject.transform.DOScale(new Vector2(1.1f, 1.1f), scaleChangeTime);
        }
    }

    public void ChangePlayerButtonScaleForExit(SoundManager sManager)
    {
        float scaleChangeTime = 0.2f;

        if (!sManager.isButtonChanging)
        {
            eveTrigger.gameObject.transform.DOScale(new Vector2(1f, 1f), scaleChangeTime);
        }
    }

    //PaintTool用
    public void ChangePaintToolScaleForEnterAndExit(float size)
    {
        if (!PaintToolMovement.isDraging)
        {
            eveTrigger.gameObject.transform.DOScale(new Vector2(size, size), scaleChangeTime);
        }
    }
}
