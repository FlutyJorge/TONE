using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ObjectScaler : MonoBehaviour
{
    private EventTrigger eveTrigger;
    [HideInInspector] public bool isScaleChanging = false;

    private bool isClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        eveTrigger = GetComponent<EventTrigger>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //スケーリングに制約がない場合用
    public void ChangeScale(float size)
    {
        eveTrigger.gameObject.transform.DOScale(new Vector2(size, size), 0.1f);
    }

    //クリック中はEnterとExitを反応させない場合
    public void ChangeScaleForEnterAndExit(float size)
    {
        if (!isClicked)
        {
            eveTrigger.gameObject.transform.DOScale(new Vector2(size, size), 0.1f);
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
        eveTrigger.gameObject.transform.DOScale(new Vector2(size, size), 0.1f);
        yield return new WaitForSeconds(0.1f);
        isClicked = false;
        yield break;
    }

    //TypeChanger用
    public void ChangeTypeChangerScaleForEnter(TypeChanger typeChanger)
    {
        if (!typeChanger.isTypeChanging)
        {
            eveTrigger.gameObject.transform.DOScale(new Vector2(1.1f, 1.1f), 0.1f);
        }
    }

    public void ChangeTypeChangerScaleForExit(TypeChanger typeChanger)
    {
        if (!typeChanger.isTypeChanging)
        {
            eveTrigger.gameObject.transform.DOScale(new Vector2(1f, 1f), 0.1f);
        }
    }

    //Box用のスケーリング
    public void ChangeBoxScaleForEnter(Type3Movement type3Mov)
    {
        if (!type3Mov.canType3Swap)
        {
            eveTrigger.gameObject.transform.DOScale(new Vector2(1.1f, 1.1f), 0.1f);
        }
    }

    public void ChangeBoxScaleForExit(Type3Movement type3Mov)
    {
        if (!type3Mov.canType3Swap)
        {
            eveTrigger.gameObject.transform.DOScale(new Vector2(1f, 1f), 0.1f);
        }
    }

    //Type変更時のスケーリング
    public IEnumerator ChangeTypeButtonScale(TypeChanger typeChanger, CommonMovement comMov, GameObject[] buttons, GameObject[] pushedButtons, int num)
    {
        if (comMov.puzzleType1)
        {
            ChangeTypeButtonScaleSet(buttons, pushedButtons, 0);
        }
        else if (comMov.puzzleType2)
        {
            ChangeTypeButtonScaleSet(buttons, pushedButtons, 1);
        }
        else
        {
            ChangeTypeButtonScaleSet(buttons, pushedButtons, 2);
        }

        buttons[num].transform.DOScale(new Vector3(0, 0, 0), 0.2f);
        pushedButtons[num].transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);

        yield return new WaitForSeconds(0.2f);
        typeChanger.isTypeChanging = false;
        yield break;
    }

    private void ChangeTypeButtonScaleSet(GameObject[] buttons, GameObject[] pushedButtons, int buttonIndex)
    {
        buttons[buttonIndex].transform.DOScale(new Vector2(1f, 1f), 0.2f);
        pushedButtons[buttonIndex].transform.DOScale(new Vector2(0, 0), 0.2f);
    }

    public void ChangePlayerScale(GameObject play, GameObject stop, int playNum, int stopNum)
    {
        play.transform.DOScale(new Vector2(playNum, playNum), 0.2f);
        stop.transform.DOScale(new Vector2(stopNum, stopNum), 0.2f);
        play.GetComponent<SpriteRenderer>().DOFade(playNum, 0.1f);
        stop.GetComponent<SpriteRenderer>().DOFade(stopNum, 0.1f);
    }

    //PaintTool用
    public void ChangePaintToolScaleForEnterAndExit(float size)
    {
        if (!PaintToolMovement.isDraging)
        {
            eveTrigger.gameObject.transform.DOScale(new Vector2(size, size), 0.1f);
        }
    }
}
