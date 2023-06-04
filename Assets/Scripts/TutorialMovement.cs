using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

//�`���[�g���A���ɂ����āA����̃}�E�X���삪�Ȃ��ꂽ���ǂ����𔻒肷��N���X
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
                Debug.Log("�X���b�v���I");
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

    //�y�C���g�c�[�����s�̉񐔑���p
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
