using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

//チュートリアルにおいて、特定のマウス操作がなされたかどうかを判定するクラス
public class TutorialMovement : MonoBehaviour
{
    [SerializeField] CommonMovement comMov;
    [SerializeField] TutorialManager tutorialMana;
    [SerializeField] PaintToolMovement paintToolMov;

    private EventTrigger eveTrigger;

    // Start is called before the first frame update
    void Start()
    {
        eveTrigger = GetComponent<EventTrigger>();
    }

    public void ClickBoxL()
    {
        if (tutorialMana != null && !tutorialMana.readyToSwap)
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!comMov.swaping)
            {
                GameObject clickedObj = eveTrigger.gameObject;
                comMov.ChangeBox(clickedObj);
                tutorialMana.isBoxClickedL = true;
            }
            else
            {
                Debug.Log("スワップ中！");
            }
        }
    }

    public void ClickeBoxR()
    {
        if (Input.GetMouseButtonUp(1))
        {
            tutorialMana.isBoxClickedR = true;
        }
    }

    //ペイントツール実行の回数測定用
    public void CheckCollisionForTutorial()
    {
        paintToolMov.CheckCollision();

        if (paintToolMov.isCollision)
        {
            tutorialMana.isDropedPaintTool = true;
            ++tutorialMana.paintToolUsed;
        }
    }
}
