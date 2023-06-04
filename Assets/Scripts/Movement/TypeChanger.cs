using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TypeChanger : MonoBehaviour
{
    [SerializeField] CommonMovement comMov;
    [SerializeField] Type1Movement type1Mov;
    [SerializeField] Type2Movement type2Mov;
    [SerializeField] Type3Movement type3Mov;
    [SerializeField] ObjectScaler objScaler;

    [Space(10)]
    [Header("モード変更ボタン")]
    [SerializeField] GameObject[] buttons = new GameObject[3];
    [SerializeField] GameObject[] pushedButtons = new GameObject[3];

    public bool isTypeChanging = false;

    public void ChangeType(int num)
    {
        if (!isTypeChanging)
        {
            isTypeChanging = true;

            switch (num)
            {
                case 0:
                    Debug.Log("Type1");
                    comMov.getTargetIndex = type1Mov.GetTargetIndex;
                    ChangeScaleAndTypeFlag(num, ref comMov.puzzleType1, ref comMov.puzzleType2, ref comMov.puzzleType3);
                    ResetBoxSize();
                    break;

                case 1:
                    Debug.Log("Type2");
                    comMov.getTargetIndex = type2Mov.GetTargetIndex;
                    ChangeScaleAndTypeFlag(num, ref comMov.puzzleType2, ref comMov.puzzleType1, ref comMov.puzzleType3);
                    ResetBoxSize();
                    break;

                case 2:
                    Debug.Log("Type3");
                    comMov.getTargetIndex = type3Mov.GetTargetIndex;
                    ChangeScaleAndTypeFlag(num, ref comMov.puzzleType3, ref comMov.puzzleType1, ref comMov.puzzleType2);
                    break;
            }
        }
    }

    private void ChangeScaleAndTypeFlag(int num, ref bool trueFlag, ref bool falseFlag1, ref bool falseFlag2)
    {
        StartCoroutine(objScaler.ChangeTypeButtonScale(this, comMov, buttons, pushedButtons, num));
        trueFlag = true;
        falseFlag1 = false;
        falseFlag2 = false;
    }

    private void ResetBoxSize()
    {
        foreach (GameObject box in comMov.boxes)
        {
            int boxNormalScale = 1;
            float scaleChangeTime = 0.2f;
            box.transform.DOScale(new Vector2(boxNormalScale, boxNormalScale), scaleChangeTime);
        }
    }
}
