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

    [Header("ボタン2種")]
    [SerializeField] GameObject[] buttons = new GameObject[3];
    [SerializeField] GameObject[] pushedButtons = new GameObject[3];

    [HideInInspector] public bool isChanging = false;

    public void ChangeType(int num)
    {
        if (!isChanging)
        {
            isChanging = true;

            switch (num)
            {
                case 0:
                    Debug.Log("Type1");
                    comMov.getTargetIndex = type1Mov.GetTargetIndex;

                    StartCoroutine(ChangeToPushedImage(num));
                    comMov.puzzleType1 = true;
                    comMov.puzzleType2 = false;
                    comMov.puzzleType3 = false;

                    ResetBoxSize();
                    break;

                case 1:
                    Debug.Log("Type2");
                    comMov.getTargetIndex = type2Mov.GetTargetIndex;

                    StartCoroutine(ChangeToPushedImage(num));
                    comMov.puzzleType1 = false;
                    comMov.puzzleType2 = true;
                    comMov.puzzleType3 = false;

                    ResetBoxSize();
                    break;

                case 2:
                    Debug.Log("Type3");
                    comMov.getTargetIndex = type3Mov.GetTargetIndex;

                    StartCoroutine(ChangeToPushedImage(num));
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
    private IEnumerator ChangeToPushedImage(int num)
    {
        if (comMov.puzzleType1)
        {
            buttons[0].transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
            pushedButtons[0].transform.DOScale(new Vector3(0, 0, 0), 0.2f);
        }
        else if (comMov.puzzleType2)
        {
            buttons[1].transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
            pushedButtons[1].transform.DOScale(new Vector3(0, 0, 0), 0.2f);
        }
        else
        {
            buttons[2].transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
            pushedButtons[2].transform.DOScale(new Vector3(0, 0, 0), 0.2f);
        }

        buttons[num].transform.DOScale(new Vector3(0, 0, 0), 0.2f);
        pushedButtons[num].transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);

        yield return new WaitForSeconds(0.2f);
        isChanging = false;
        yield break;
    }
}
