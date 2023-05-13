using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameStarter gameSta;
    [SerializeField] TypeChanger typeChanger;
    [SerializeField] Type3Movement type3Mov;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] SpriteRenderer blackBoard;
    const int frontLayer = -1;
    [HideInInspector] public int paintToolUsed = 0;
    [HideInInspector] public bool isDropedPaintTool = false;

    private bool isTutorialStart = false;
    [HideInInspector] public bool isBoxClickedR = false;
    [HideInInspector] public bool isBoxClickedL = false;
    [HideInInspector] public bool readyToSwap = false;

    [Space(10)]
    [Header("�n�C���C�g�����I�u�W�F�N�g")]
    [SerializeField] GameObject[] boxes = new GameObject[16];

    [Space(10)]
    [Header("�n�C���C�g�����I�u�W�F�N�g�̃����_���[")]
    [SerializeField] SpriteRenderer[] playersRen = new SpriteRenderer[2];
    [SerializeField] Canvas sliderRen;
    [SerializeField] SpriteRenderer[] playPointsRen = new SpriteRenderer[16];
    [SerializeField] SpriteRenderer[] typeChangersRen = new SpriteRenderer[6];
    [SerializeField] SpriteRenderer[] countersRen = new SpriteRenderer[3];
    [SerializeField] SpriteRenderer[] paintToolsRen = new SpriteRenderer[6];
    [SerializeField] SpriteRenderer tryButtonRen;
    private SpriteRenderer[] boxesRen = new SpriteRenderer[16];

    [Space(10)]
    [Header("�n�C���C�g�p�̃����_���[")]
    [SerializeField] SpriteRenderer description;
    [SerializeField] SpriteRenderer boxesHighlight;
    [SerializeField] SpriteRenderer boxes6Highlight;
    [SerializeField] SpriteRenderer boxes9Highlight;
    [SerializeField] SpriteRenderer boxHighlight;
    [SerializeField] SpriteRenderer playersHighlight;
    [SerializeField] SpriteRenderer typeChangersHighlight;
    [SerializeField] SpriteRenderer typeChangerHighlight;
    [SerializeField] SpriteRenderer paintToolsHighlight;
    [SerializeField] SpriteRenderer sliderHighlight;
    [SerializeField] SpriteRenderer tryButtonHighlight;

    [Space(10)]
    [Header("�I�u�W�F�N�g�̃R���C�_�[")]
    [SerializeField] BoxCollider2D[] typeChangersCol = new BoxCollider2D[3];
    [SerializeField] CircleCollider2D[] paintToolsCol = new CircleCollider2D[6];
    private BoxCollider2D[] boxesCol = new BoxCollider2D[16];


    //�I�u�W�F�N�g��EventTriggerManager
    private EventTriggerManager[] boxesEveTrigger = new EventTriggerManager[16];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < boxes.Length; ++i)
        {
            boxesRen[i] = boxes[i].GetComponent<SpriteRenderer>();
            boxesCol[i] = boxes[i].GetComponent<BoxCollider2D>();
            boxesEveTrigger[i] = boxes[i].GetComponent<EventTriggerManager>();
        }
        StartCoroutine(PlayTutorial());
    }

    // Update is called once per frame
    void Update()
    {
        /*if (gameSta.isGameStarterEnd && !isTutorialStart)
        {

        }*/
    }

    private IEnumerator PlayTutorial()
    {
        blackBoard.DOFade(1, 0.5f);
        description.DOFade(1, 0.5f);
        text.DOFade(1, 0.5f);

        //�e�L�X�g�����S�ɕ\������A���ɍ��N���b�N�����܂ŏ����ҋ@
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("������A�Q�[���̗V�ѕ��ɂ��Ĉ�ʂ���������Ă��������܂��B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("�܂��A��ʒ�����16�̐������}�[�N�ɒ��ڂ��Ă��������B"));

        //Boxes���n�C���C�g����
        for (int i = 0; i < boxes.Length; ++i)
        {
            boxesRen[i].sortingOrder = -1;
        }
        boxesHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("�e�p�Y���s�[�X�ɂ͐�قǒ����Ă����������Ȃ�16�̃t���[�Y�ɕ��������\n���[����Ă��܂��B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("�ǂ̃s�[�X�ɂǂ̃t���[�Y�����[����Ă���̂��́A�s�[�X���E�N���b�N���Ċm���߂邱�Ƃ��ł��܂���B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("Challenge\n���Âꂩ�̃s�[�X���E�N���b�N����B 0/3"));

        yield return new WaitForSeconds(0.3f);

        //Boxes���N���b�N�ł���悤�ɂ���
        foreach (BoxCollider2D boxCol in boxesCol)
        {
            boxCol.enabled = true;
        }

        const int challengeNum = 3;
        for (int i = 1; i <= challengeNum; ++i)
        {
            yield return new WaitUntil(() => isBoxClickedR);
            text.SetText("Challenge\n���Âꂩ�̃s�[�X���E�N���b�N����B " + i + " / 3");

            //WaitUntil��Ƀt���O���ĂуI�t�ɂ��鎞�Ԃ�݂���
            isBoxClickedR = false;
            yield return new WaitForSeconds(0.1f);
        }

        //Boxes���N���b�N�ł��Ȃ��悤�ɂ���
        foreach (BoxCollider2D boxCol in boxesCol)
        {
            boxCol.enabled = false;
        }

        StartCoroutine(SetText("�m���߂Ă��������܂������H"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("���̃s�[�X�B�����ɕ~����Ă�������ɉ����āA�Ȃ̗���ʂ�ɕ��ׂ邱�Ƃ��v���C���[����̖ڕW�ł��B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("�Ȃ��m�F����ɂ͍����̍Đ��A��~�{�^�����g���Ă��������B"));


        //�n�C���C�g��Boxes����Players�ɕύX
        boxesHighlight.color = new Color(1, 1, 1, 0);
        foreach (SpriteRenderer boxRen in boxesRen)
        {
            boxRen.sortingOrder = -5;
        }

        foreach (SpriteRenderer playerRen in playersRen)
        {
            playerRen.sortingOrder = -1;
        }
        playersHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(MoveAndSetText(-2.85f, -2.7f, "����̈ʒu����Đ��������ꍇ�́A���̍Đ��o�[���g���ƕ֗��ł�����o���Ă����Ă��������ˁB"));

        yield return new WaitForSeconds(0.3f);

        //�n�C���C�g��Players����Slider�ɕύX
        playersHighlight.color = new Color(1, 1, 1, 0);
        foreach (SpriteRenderer playerRen in playersRen)
        {
            playerRen.sortingOrder = -6;
        }

        sliderRen.sortingOrder = -1;
        foreach (SpriteRenderer playPointRen in playPointsRen)
        {
            playPointRen.sortingOrder = 0;
        }
        sliderHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        //Slider�̃n�C���C�g������
        sliderHighlight.color = new Color(1, 1, 1, 0);
        foreach (SpriteRenderer playPointRen in playPointsRen)
        {
            playPointRen.sortingOrder = -5;
        }
        sliderRen.sortingOrder = -5;

        StartCoroutine(MoveAndSetText(-4.35f, -4.2f, "�ł́A���Ƀs�[�X�̕��בւ����@�ɂ��Ă��������܂��B"));

        //TypeChangers���n�C���C�g����
        foreach (SpriteRenderer typeChangerRen in typeChangersRen)
        {
            typeChangerRen.sortingOrder = -1;
        }
        foreach (SpriteRenderer counterRen in countersRen)
        {
            counterRen.sortingOrder = -1;
        }
        typeChangersHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("���בւ��ɂ́A�X���b�v���[�h�A���[�e�[�V�������[�h�A�V�����g���[���[�h��3��ނ��g���܂��B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("�܂��A�X���b�v���[�h�ł́A�ۂɈ͂܂�Ă���s�[�X��4�����ɗׂ荇�����s�[�X�����N���b�N���邱�ƂŁA�݂��̈ʒu�����ւ��邱�Ƃ��ł��܂���B"));

        //�X���b�v���[�h���n�C���C�g
        typeChangersHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[1].sortingOrder = -6;
        typeChangersRen[2].sortingOrder = -6;
        countersRen[1].sortingOrder = -4;
        countersRen[2].sortingOrder = -4;
        typeChangerHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("Challenge\n�s�[�X�����N���b�N���ăX���b�v������ 0/3"));

        //�n�C���C�g���X���b�v���[�h����Box�ɕύX��Box14���X���b�v�ł���悤��
        typeChangerHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[0].sortingOrder = -6;
        typeChangersRen[3].sortingOrder = -5;
        countersRen[0].sortingOrder = -6;

        boxesRen[14].sortingOrder = -1;
        boxesRen[15].sortingOrder = -1;
        boxesCol[14].enabled = true;
        boxHighlight.color = new Color(1, 1, 1, 1);
        readyToSwap = true;

        const float swapingTime = 1f;
        for (int counter = 1; counter <= challengeNum; ++counter)
        {
            yield return new WaitUntil(() => isBoxClickedL);

            switch (counter)
            {
                case 1:
                    //Box14���N���b�N���ABox9���N���b�N�\�ɂ���
                    isBoxClickedL = false;
                    text.SetText("Challenge\n�s�[�X�����N���b�N���ăX���b�v������ " + counter + " / 3");
                    yield return new WaitForSeconds(swapingTime);
                    boxesRen[14].sortingOrder = -5;
                    boxesRen[9].sortingOrder = -1;
                    boxHighlight.transform.position = new Vector3(-0.75f, -0.75f, 0);
                    boxesCol[14].enabled = false;
                    boxesCol[9].enabled = true;
                    break;

                case 2:
                    //Box9���N���b�N���ABox10���N���b�N�\�ɂ���
                    isBoxClickedL = false;
                    text.SetText("Challenge\n�s�[�X�����N���b�N���ăX���b�v������ " + counter + " / 3");
                    yield return new WaitForSeconds(swapingTime);
                    boxesRen[9].sortingOrder = -5;
                    boxesRen[10].sortingOrder = -1;
                    boxHighlight.transform.position = new Vector3(0.75f, -0.75f, 0);
                    boxesCol[9].enabled = false;
                    boxesCol[10].enabled = true;
                    break;

                case 3:
                    //Box10���N���b�N���A���[�v�𔲂���
                    isBoxClickedL = false;
                    text.SetText("Challenge\n�s�[�X�����N���b�N���ăX���b�v������ " + counter + " / 3");
                    yield return new WaitForSeconds(swapingTime);
                    boxesRen[10].sortingOrder = -5;
                    boxesRen[15].sortingOrder = -5;
                    boxesCol[10].enabled = false;
                    boxHighlight.color = new Color(1, 1, 1, 0);
                    break;
            }
        }

        StartCoroutine(SetText("���ɁA���[�e�[�V�������[�h�ɂ��Ă��Љ�܂��ˁB"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("���̑O�ɁA�X���b�v���[�h���烍�[�e�[�V�������[�h�ɐ؂�ւ��Ă݂܂��傤�B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("�e���[�h�ւ̐؂�ւ��͂��ł������̃{�^������ł��܂���B"));

        //TypeChangers���n�C���C�g����
        foreach (SpriteRenderer typeChangerRen in typeChangersRen)
        {
            typeChangerRen.sortingOrder = -1;
        }
        foreach (SpriteRenderer counterRen in countersRen)
        {
            counterRen.sortingOrder = -1;
        }
        typeChangersHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("Challenge\n���[�e�[�V�������[�h�ɐ؂�ւ���"));

        //�n�C���C�g��TypeChangers���烍�[�e�[�V�������[�h��
        typeChangersHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[0].sortingOrder = -6;
        typeChangersRen[2].sortingOrder = -6;
        typeChangersRen[3].sortingOrder = -5;
        typeChangersRen[5].sortingOrder = -5;
        typeChangersCol[1].enabled = true;
        countersRen[0].sortingOrder = -4;
        countersRen[2].sortingOrder = -4;
        typeChangerHighlight.transform.position = new Vector3(7.5f, 1.84f, 0);
        typeChangerHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => typeChanger.isTypeChanging);

        StartCoroutine(SetText("���[�e�[�V�������[�h�ł́A���N���b�N�őI�������s�[�X�����4�̃s�[�X�����v���ɉ�]�����邱�Ƃ��ł����ł��B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("Challenge\n�s�[�X�����N���b�N���ă��[�e�[�V����������B"));

        //�n�C���C�g�����[�e�[�V�������[�h����Box�ɕύX��Box11���X���b�v�ł���悤��
        typeChangerHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[1].sortingOrder = -6;
        typeChangersRen[4].sortingOrder = -5;
        boxesRen[15].sortingOrder = -1;
        boxesRen[5].sortingOrder = -1;
        boxesRen[4].sortingOrder = -1;
        boxesRen[11].sortingOrder = -1;
        boxesCol[11].enabled = true;
        boxHighlight.transform.position = new Vector3(2.25f, -0.75f, 0);
        boxHighlight.color = new Color(1, 1, 1, 1);

        for (int counter = 1; counter < challengeNum; ++counter)
        {
            yield return new WaitUntil(() => isBoxClickedL);

            switch (counter)
            {
                //Box11���N���b�N���ABox15���N���b�N�ł���悤�ɁB1, 2, 6, 15
                case 1:
                    isBoxClickedL = false;
                    text.SetText("Challenge\n�s�[�X�����N���b�N���ăX���b�v������ " + counter + " / 2");
                    yield return new WaitForSeconds(1f);
                    boxesRen[5].sortingOrder = -5;
                    boxesRen[4].sortingOrder = -5;
                    boxesRen[11].sortingOrder = -5;
                    boxesCol[11].enabled = false;
                    boxesRen[1].sortingOrder = -1;
                    boxesRen[2].sortingOrder = -1;
                    boxesRen[6].sortingOrder = -1;
                    boxesCol[15].enabled = true;
                    boxHighlight.transform.position = new Vector3(0.75f, 0.75f, 0);
                    break;

                //Box15���N���b�N���A���[�v�𔲂���

                case 2:
                    isBoxClickedL = false;
                    text.SetText("Challenge\n�s�[�X�����N���b�N���ăX���b�v������ " + counter + " / 2");
                    yield return new WaitForSeconds(1f);
                    boxesRen[1].sortingOrder = -5;
                    boxesRen[2].sortingOrder = -5;
                    boxesRen[6].sortingOrder = -5;
                    boxesRen[15].sortingOrder = -5;
                    boxesCol[15].enabled = false;
                    countersRen[1].sortingOrder = -4;
                    boxHighlight.color = new Color(1, 1, 1, 0);
                    break;
            }
        }

        StartCoroutine(SetText("���������ł��ˁB"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("�Ō�ɁA�V�����g���[���[�h�ɂ��ďЉ���Ă��������܂����A��������Ȃ̂ł���΂��Ă��������ˁB"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("���̃��[�h�ł̓s�[�X�𓮂������߂ɁA�����̃s�[�X��͈͑I������K�v������܂��B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("Challenge\n�V�����g���[���[�h�ɐ؂�ւ���"));

        //�V�����g���[���[�h���n�C���C�g
        typeChangersRen[2].sortingOrder = -1;
        typeChangersRen[5].sortingOrder = -1;
        typeChangersCol[2].enabled = true;
        countersRen[2].sortingOrder = -1;
        typeChangerHighlight.transform.position = new Vector3(6.35f, -0.86f, 0);
        typeChangerHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => typeChanger.isTypeChanging);

        //�V�����g���[���[�h�̃n�C���C�g���I�t��
        typeChangerHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[2].sortingOrder = -6;
        typeChangersRen[5].sortingOrder = -5;
        typeChangersCol[2].enabled = false;
        countersRen[2].sortingOrder = -4;

        StartCoroutine(SetText("Challenge\n�s�[�X���J�[�\���h���b�O�Ŕ͈͑I������B"));

        //Boxes9���n�C���C�g&Box0, 6, 1, 7, 15, 2, 8, 10, 11��I���ł���悤��
        boxesRen[0].sortingOrder = -1;
        boxesRen[6].sortingOrder = -1;
        boxesRen[1].sortingOrder = -1;
        boxesRen[7].sortingOrder = -1;
        boxesRen[15].sortingOrder = -1;
        boxesRen[2].sortingOrder = -1;
        boxesRen[8].sortingOrder = -1;
        boxesRen[10].sortingOrder = -1;
        boxesRen[11].sortingOrder = -1;
        boxes9Highlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => CheckSelectedBox(9, 0, 11));

        StartCoroutine(SetText("�I�����ꂽ�s�[�X�B�������傫���Ȃ��Ă���̂��킩��܂����H"));
        //���N���b�N�����Ă�Box�I����Ԃ���������Ȃ��悤��
        type3Mov.canResetBoxSize = false;

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("���̏�ԂŔ͈͑I�����ꂽ�s�[�X�̈�����N���b�N����ƁA���̃s�[�X�ƑI��͈͂̒��őΏۂ̈ʒu�ɂ���s�[�X�Ƃ����ւ��邱�Ƃ��ł��܂��B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("Challenge\n����̃s�[�X���N���b�N���ĉE���̃s�[�X�Ɠ���ւ���"));

        //�n�C���C�g��Boxes9����Box��
        boxes9Highlight.color = new Color(1, 1, 1, 0);

        boxHighlight.transform.position = new Vector3(-2.25f, 2.25f, 0);
        boxHighlight.size = new Vector3(1.1f, 1.1f, 1);
        boxHighlight.color = new Color(1, 1, 1, 1);
        boxesCol[0].enabled = true;

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => isBoxClickedL);

        isBoxClickedL = false;

        //Box�̃T�C�Y�����ɖ߂�
        type3Mov.canType3Swap = false;
        type3Mov.DeactivateAllBoxes();
        type3Mov.canResetBoxSize = true;

        yield return new WaitForSeconds(1f);

        StartCoroutine(SetText("�����Ɉړ��ł��܂����ˁB"));

        boxHighlight.color = new Color(1, 1, 1, 0);
        boxesRen[0].sortingOrder = -5;
        boxesRen[6].sortingOrder = -5;
        boxesRen[1].sortingOrder = -5;
        boxesRen[7].sortingOrder = -5;
        boxesRen[15].sortingOrder = -5;
        boxesRen[2].sortingOrder = -5;
        boxesRen[8].sortingOrder = -5;
        boxesRen[10].sortingOrder = -5;
        boxesRen[11].sortingOrder = -5;
        boxesCol[0].enabled = false;

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("������x�����Ă݂܂��傤"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("Challenge\n�s�[�X���J�[�\���h���b�O�Ŕ͈͑I������"));

        //Box0, 6, 1, 7, 15, 2, 8, 10, 11���n�C���C�g����
        boxesRen[6].sortingOrder = -1;
        boxesRen[1].sortingOrder = -1;
        boxesRen[7].sortingOrder = -1;
        boxesRen[15].sortingOrder = -1;
        boxesRen[2].sortingOrder = -1;
        boxesRen[11].sortingOrder = -1;
        boxes6Highlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => CheckSelectedBox(6, 11, 2));

        StartCoroutine(SetText("Challenge\n����̃s�[�X���N���b�N���ĉE���̃s�[�X�Ɠ���ւ���"));

        type3Mov.canResetBoxSize = false;
        boxes6Highlight.color = new Color(1, 1, 1, 0);
        boxHighlight.color = new Color(1, 1, 1, 1);
        boxesCol[11].enabled = true;

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => isBoxClickedL);

        isBoxClickedL = false;

        //Box�̃T�C�Y�����ɖ߂�
        type3Mov.canType3Swap = true;
        type3Mov.DeactivateAllBoxes();
        type3Mov.canResetBoxSize = true;

        yield return new WaitForSeconds(1f);

        StartCoroutine(SetText("3�̓���ւ����@�͎g�����Ȃ������ł��傤���H"));

        boxHighlight.color = new Color(1, 1, 1, 0);
        boxesRen[6].sortingOrder = -5;
        boxesRen[1].sortingOrder = -5;
        boxesRen[7].sortingOrder = -5;
        boxesRen[15].sortingOrder = -5;
        boxesRen[2].sortingOrder = -5;
        boxesRen[11].sortingOrder = -5;
        boxesCol[11].enabled = false;

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("�����ŁA�s�[�X����ёւ���ɂ������ĕ֗��ȋ@�\�����Љ�܂��傤�B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("�����̓y�C���g�c�[���ƌĂ΂����̂ŁA�s�[�X�̐F��ς��邱�Ƃ��ł��܂��B"));

        paintToolsHighlight.color = new Color(1, 1, 1, 1);
        foreach (SpriteRenderer paintToolRen in paintToolsRen)
        {
            paintToolRen.sortingOrder = -1;
        }

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("�y�C���g�c�[�����N���b�N�����܂܁A�ς������s�[�X�̈ʒu�܂Ńh���b�O���Ď��ۂɐF��ς��Ă݂܂��傤�B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("Challenge\n�y�C���g�c�[�����s�[�X�Ƀh���b�O&�h���b�v����B 0/3"));

        foreach (CircleCollider2D paintToolCol in paintToolsCol)
        {
            paintToolCol.enabled = true;
        }

        //Boxes�̃n�C���C�g���I���ɂ���
        for (int i = 0; i < boxes.Length; ++i)
        {
            boxesRen[i].sortingOrder = -1;
        }
        boxesHighlight.color = new Color(1, 1, 1, 1);

        for (int i = 1; i <= 3; ++i)
        {
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => isDropedPaintTool);
            isDropedPaintTool = false;
            text.SetText("Challenge\n�y�C���g�c�[�����s�[�X�Ƀh���b�O & �h���b�v����B " + i + "/3");
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(SetText("�g�p�񐔂ɐ����͂���܂���̂ŁA���Жڈ�Ƃ��ėL�����p���Ă��������B"));

        foreach (CircleCollider2D paintToolCol in paintToolsCol)
        {
            paintToolCol.enabled = false;
        }

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("�����܂ŁA�Q�[���̐i�ߕ��ɂ��Ă��������܂������A�Ō�ɃN���A���@�ɂ��Ă��������܂��B"));

        //Boxes��PaintTools�̃n�C���C�g���I�t�ɂ���
        boxesHighlight.color = new Color(1, 1, 1, 0);
        for (int i = 0; i < boxes.Length; ++i)
        {
            boxesRen[i].sortingOrder = -5;
        }

        paintToolsHighlight.color = new Color(1, 1, 1, 0);
        foreach (SpriteRenderer paintToolRen in paintToolsRen)
        {
            paintToolRen.sortingOrder = -4;
        }

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("�N���A���@�͂������ăV���v���ŁA�s�[�X���Ȃ̏��Ԃɕ��וς��I������炱��TRY�{�^�������������ł��B"));

        //Try�̃n�C���C�g���I����
        tryButtonRen.sortingOrder = -1;
        tryButtonHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("�����A���������Ԃ̃s�[�X�����בւ����Ă��Ȃ��ƃ{�^���������Ă��N���A�ł��܂���̂Œ��ӂ��Ă��������ˁB"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(SetText("�ȏ�ŃQ�[�������͏I���ł��B����ꂳ�܂ł����I"));

        //Box11���N���b�N�ł���悤�ɂ���

        yield break;
    }

    private IEnumerator SetText(string textContent)
    {
        text.DOFade(0, 0.3f);
        yield return new WaitForSeconds(0.3f);
        text.SetText(textContent);
        text.DOFade(1, 0.3f);
    }

    private IEnumerator MoveAndSetText(float textPosY, float descriptionPosY, string textContent)
    {
        text.DOFade(0, 0.3f);
        description.DOFade(0, 0.3f);
        yield return new WaitForSeconds(0.3f);
        text.rectTransform.position = new Vector3(0, textPosY, 0);
        description.transform.position = new Vector3(0, descriptionPosY, 0);
        text.SetText(textContent);
        text.DOFade(1, 0.3f);
        description.DOFade(1, 0.3f);
    }

    //�V�����g���[���[�h�Ŕ͈͑I�������ہA�Ӑ}���ꂽ�͈͑I�����s���Ă��邩�m�F
    private bool CheckSelectedBox(int boxCount, int topLeftBox, int bottomRightBox)
    {
        if (type3Mov.selectedBoxes.Count != boxCount)
        {
            return false;
        }

        int counter = 0;

        for (int i = 0; i < type3Mov.selectedBoxes.Count; ++i)
        {
            if (type3Mov.selectedBoxes[i] == boxes[topLeftBox] || type3Mov.selectedBoxes[i] == boxes[bottomRightBox])
            {
                ++counter;
            }

            if (counter == 2)
            {
                return true;
            }
        }
        return false;
    }
}
