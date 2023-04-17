using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TypeChanger : MonoBehaviour
{
    [Header("スクリプトの参照")]
    [SerializeField] CommonMovement comMov;
    [SerializeField] Type1Movement type1Mov;
    [SerializeField] Type2Movement type2Mov;
    [SerializeField] Type3Movement type3Mov;
    [SerializeField] ObjectScaler objScaler;

    [Header("ボタン2種")]
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

                    StartCoroutine(objScaler.ChangeTypeButtonScale(this, comMov, buttons, pushedButtons, num));
                    comMov.puzzleType1 = true;
                    comMov.puzzleType2 = false;
                    comMov.puzzleType3 = false;

                    ResetBoxSize();
                    break;

                case 1:
                    Debug.Log("Type2");
                    comMov.getTargetIndex = type2Mov.GetTargetIndex;

                    StartCoroutine(objScaler.ChangeTypeButtonScale(this, comMov, buttons, pushedButtons, num));
                    comMov.puzzleType1 = false;
                    comMov.puzzleType2 = true;
                    comMov.puzzleType3 = false;

                    ResetBoxSize();
                    break;

                case 2:
                    Debug.Log("Type3");
                    comMov.getTargetIndex = type3Mov.GetTargetIndex;

                    StartCoroutine(objScaler.ChangeTypeButtonScale(this, comMov, buttons, pushedButtons, num));
                    comMov.puzzleType1 = false;
                    comMov.puzzleType2 = false;
                    comMov.puzzleType3 = true;
                    break;
            }
        }
    }

    private void ResetBoxSize()
    {
        foreach (GameObject box in comMov.boxes)
        {
            box.transform.DOScale(new Vector2(1f, 1f), 0.2f);
        }
    }
}
