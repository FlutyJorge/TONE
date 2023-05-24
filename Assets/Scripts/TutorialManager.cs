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
    [SerializeField] OptionClick optionClick;
    [SerializeField] SceneChanger sceneChan;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] SpriteRenderer blackBoard;

    const int frontLayer = -1;
    [HideInInspector] public int paintToolUsed = 0;
    private bool isTutorialStarted = false;
    [HideInInspector] public bool isBoxClickedR = false;
    [HideInInspector] public bool isBoxClickedL = false;
    [HideInInspector] public bool readyToSwap = false;
    [HideInInspector] public bool isDropedPaintTool = false;

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
    [SerializeField] SpriteRenderer pathRen;
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
    [SerializeField] CircleCollider2D[] paintToolCheckersCol = new CircleCollider2D[16];
    private BoxCollider2D[] boxesCol = new BoxCollider2D[16];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < boxes.Length; ++i)
        {
            boxesRen[i] = boxes[i].GetComponent<SpriteRenderer>();
            boxesCol[i] = boxes[i].GetComponent<BoxCollider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSta.isGameStarterEnd && !isTutorialStarted)
        {
            StartCoroutine(PlayTutorial());
            isTutorialStarted = true;
        }
    }

    private IEnumerator PlayTutorial()
    {
        blackBoard.DOFade(1, 0.5f);
        description.DOFade(1, 0.5f);
        text.DOFade(1, 0.5f);

        //�e�L�X�g�����S�ɕ\������A���ɍ��N���b�N�����܂ŏ����ҋ@
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("������A�Q�[���̗V�ѕ��ɂ��Ĉ�ʂ���������Ă��������܂��ˁB"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("�܂��A��ʒ����ɂ���16�̐������}�[�N�ɒ��ڂ��Ă��������B"));

        //Boxes��path�̃n�C���C�g���I����
        for (int i = 0; i < boxes.Length; ++i)
        {
            boxesRen[i].sortingOrder = -1;
        }
        pathRen.sortingOrder = -2;
        boxesHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("���̃s�[�X�B�����ɕ~����Ă�������ɉ����āA�Ȃ̗���ʂ�ɕ��ׂ�\n���Ƃ��v���C���[����̖ڕW�ł��B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("����΁A���̃p�Y���Q�[���Ƃ������Ƃ���ł��傤���H"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("�e�p�Y���s�[�X�ɂ͎n�߂ɒ����Ă����������Ȃ�16�̃t���[�Y�ɕ��������\n���蓖�Ă��Ă��܂��B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("�ǂ̃s�[�X�ɂǂ̃t���[�Y�����蓖�Ă��Ă���̂��́A�s�[�X���E�N���b�N���Ċm���߂邱�Ƃ��ł��܂���B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("Challenge\n���Âꂩ�̃s�[�X���E�N���b�N����B 0/3"));

        yield return new WaitForSeconds(0.3f);

        //Boxes���N���b�N�ł���悤�ɂ���
        foreach (BoxCollider2D boxCol in boxesCol)
        {
            boxCol.enabled = true;
        }

        const int threeChallenges = 3;
        for (int i = 1; i <= threeChallenges; ++i)
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
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("�p�Y���������Ă����ɂ������āA������Ȃƃs�[�X�Ɋ��蓖�Ă�ꂽ�t���[�Y�𒮂���ׂ邱�ƂɂȂ�ł��傤�B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("����Ȏ��ɂ́A�����̍Đ��A��~�{�^�����g���Ă��������B"));


        //Boxes�̃n�C���C�g���I�t�ɁAPlayers�̃n�C���C�g���I����
        boxesHighlight.color = new Color(1, 1, 1, 0);
        foreach (SpriteRenderer boxRen in boxesRen)
        {
            boxRen.sortingOrder = -5;
        }
        pathRen.sortingOrder = -6;

        foreach (SpriteRenderer playerRen in playersRen)
        {
            playerRen.sortingOrder = -2;
        }
        playersHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        //�X���C�_�[�������邽�߁A�e�L�X�g�̈ʒu����ɂ��炷
        StartCoroutine(MoveAndSetText(-2.85f, -2.7f, "����̈ʒu����Đ��������ꍇ�́A���̍Đ��o�[���g���ƕ֗��ł�����o���Ă����Ă��������ˁB"));

        yield return new WaitForSeconds(0.3f);

        //Players�̃n�C���C�g���I�t�ɁASlider�̃n�C���C�g���I����
        playersHighlight.color = new Color(1, 1, 1, 0);
        foreach (SpriteRenderer playerRen in playersRen)
        {
            playerRen.sortingOrder = -5;
        }

        sliderRen.sortingOrder = -2;
        foreach (SpriteRenderer playPointRen in playPointsRen)
        {
            playPointRen.sortingOrder = -1;
        }
        sliderHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        //Slider�̃n�C���C�g���I�t��
        sliderHighlight.color = new Color(1, 1, 1, 0);
        foreach (SpriteRenderer playPointRen in playPointsRen)
        {
            playPointRen.sortingOrder = -5;
        }
        sliderRen.sortingOrder = -6;

        StartCoroutine(MoveAndSetText(-4.35f, -4.2f, "�ł́A���Ƀs�[�X�̕��בւ����@�ɂ��Ă��������܂��B"));

        //TypeChangers�̃n�C���C�g���I����
        foreach (SpriteRenderer typeChangerRen in typeChangersRen)
        {
            typeChangerRen.sortingOrder = -2;
        }
        foreach (SpriteRenderer counterRen in countersRen)
        {
            counterRen.sortingOrder = -2;
        }
        typeChangersHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("���בւ��ɂ́A�X���b�v���[�h�A���[�e�[�V�������[�h�A�V�����g���[���[�h��3��ނ��g���܂��B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("�܂��A�X���b�v���[�h�ł́A�ۂň͂܂�Ă���s�[�X��4�����ɗׂ荇����\n�s�[�X�����N���b�N���邱�ƂŁA�݂��̈ʒu�����ւ��邱�Ƃ��ł��܂��B"));

        //TypeChangers�̃n�C���C�g���I�t�ɁA�X���b�v���[�h�̃n�C���C�g�͈ێ�
        typeChangersHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[1].sortingOrder = -5;
        typeChangersRen[2].sortingOrder = -5;
        countersRen[1].sortingOrder = -5;
        countersRen[2].sortingOrder = -5;
        typeChangerHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("Challenge\n�s�[�X�����N���b�N���ăX���b�v������B 0/2"));

        //�X���b�v���[�h�̃n�C���C�g���I�t�ɁABox�̃n�C���C�g���I����
        typeChangerHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[0].sortingOrder = -5;
        typeChangersRen[3].sortingOrder = -5;
        countersRen[0].sortingOrder = -5;

        //Box9���N���b�N�\��
        boxesRen[9].sortingOrder = -2;
        boxesRen[15].sortingOrder = -2;
        boxesCol[9].enabled = true;
        boxHighlight.color = new Color(1, 1, 1, 1);
        readyToSwap = true;

        const int twoChallenges = 2;
        const float swapingTime = 1f;
        for (int counter = 1; counter <= twoChallenges; ++counter)
        {
            yield return new WaitUntil(() => isBoxClickedL);

            switch (counter)
            {
                case 1:
                    //Box9���N���b�N�����̂ŁABox6���N���b�N�\�ɂ���
                    isBoxClickedL = false;
                    text.SetText("Challenge\n�s�[�X�����N���b�N���ăX���b�v������B " + counter + " / 2");
                    yield return new WaitForSeconds(swapingTime);
                    boxesRen[9].sortingOrder = -5;
                    boxesRen[6].sortingOrder = -2;
                    boxHighlight.transform.position = new Vector3(-2.25f, -2.25f, 0);
                    boxesCol[9].enabled = false;
                    boxesCol[6].enabled = true;
                    break;

                case 2:
                    //Box13���N���b�N�����̂ŁA���[�v�𔲂���
                    isBoxClickedL = false;
                    text.SetText("Challenge\n�s�[�X�����N���b�N���ăX���b�v������B " + counter + " / 2");
                    yield return new WaitForSeconds(swapingTime);
                    boxesRen[6].sortingOrder = -5;
                    boxesRen[15].sortingOrder = -5;
                    boxesCol[6].enabled = false;
                    boxHighlight.color = new Color(1, 1, 1, 0);
                    break;
            }
        }

        StartCoroutine(SetText("���ɁA���[�e�[�V�������[�h�ɂ��Ă��Љ�܂��ˁB"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("���̑O�ɁA�X���b�v���[�h���烍�[�e�[�V�������[�h�ɐ؂�ւ��Ă݂܂��傤�B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("�e���[�h�ւ̐؂�ւ��͂��ł������̃{�^������ł��܂���B"));

        //TypeChangers���n�C���C�g����
        foreach (SpriteRenderer typeChangerRen in typeChangersRen)
        {
            typeChangerRen.sortingOrder = -2;
        }
        foreach (SpriteRenderer counterRen in countersRen)
        {
            counterRen.sortingOrder = -2;
        }
        typeChangersHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("Challenge\n���[�e�[�V�������[�h�ɐ؂�ւ���"));

        //TypeChanger�̃n�C���C�g���I�t�ɁA���[�e�[�V�������[�h�̃n�C���C�g�͈ێ�
        typeChangersHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[0].sortingOrder = -5;
        typeChangersRen[2].sortingOrder = -5;
        typeChangersRen[3].sortingOrder = -5;
        typeChangersRen[5].sortingOrder = -5;
        typeChangersCol[1].enabled = true;
        countersRen[0].sortingOrder = -5;
        countersRen[2].sortingOrder = -5;
        typeChangerHighlight.transform.position = new Vector3(7.5f, 1.84f, 0);
        typeChangerHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => typeChanger.isTypeChanging);

        StartCoroutine(SetText("���[�e�[�V�������[�h�ł́A���N���b�N�őI�������s�[�X�����4�̃s�[�X�����v���ɉ�]�����邱�Ƃ��ł��܂��B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("Challenge\n�s�[�X�����N���b�N���ă��[�e�[�V����������B 0/2"));

        //���[�e�[�V�������[�h�̃n�C���C�g���I�t�ɁABox9, 13, 6, 14�̃n�C���C�g���I����
        typeChangerHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[1].sortingOrder = -5;
        typeChangersRen[4].sortingOrder = -5;
        boxesRen[9].sortingOrder = -2;
        boxesRen[13].sortingOrder = -2;
        boxesRen[6].sortingOrder = -2;
        boxesRen[14].sortingOrder = -2;
        boxHighlight.transform.position = new Vector3(0.75f, -2.25f, 0);
        boxHighlight.color = new Color(1, 1, 1, 1);

        //Box14���N���b�N�\��
        boxesCol[14].enabled = true;

        for (int counter = 1; counter <= twoChallenges; ++counter)
        {
            yield return new WaitUntil(() => isBoxClickedL);

            switch (counter)
            {
                case 1:
                    //Box14���N���b�N�����̂ŁA�n�C���C�g��0, 5, 6, 9�ɕύX
                    isBoxClickedL = false;
                    text.SetText("Challenge\n�s�[�X�����N���b�N���ăX���b�v������B " + counter + " / 2");
                    yield return new WaitForSeconds(1f);
                    boxesRen[13].sortingOrder = -5;
                    boxesRen[14].sortingOrder = -5;
                    boxesRen[0].sortingOrder = -2;
                    boxesRen[5].sortingOrder = -2;
                    boxHighlight.transform.position = new Vector3(0.75f, -0.75f, 0);

                    //Box14���N���b�N�s�ɁABox9���N���b�N�\��
                    boxesCol[14].enabled = false;
                    boxesCol[9].enabled = true;
                    break;

                case 2:
                    //Box9���N���b�N�����̂ŁA���[�v�𔲂���
                    isBoxClickedL = false;
                    text.SetText("Challenge\n�s�[�X�����N���b�N���ăX���b�v������B " + counter + " / 2");
                    yield return new WaitForSeconds(1f);
                    boxesRen[0].sortingOrder = -5;
                    boxesRen[5].sortingOrder = -5;
                    boxesRen[6].sortingOrder = -5;
                    boxesRen[9].sortingOrder = -5;
                    countersRen[1].sortingOrder = -5;
                    boxHighlight.color = new Color(1, 1, 1, 0);

                    //Box9���N���b�N�s��
                    boxesCol[9].enabled = false;
                    break;
            }
        }

        StartCoroutine(SetText("���������ł��ˁB"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("�Ō�ɁA�V�����g���[���[�h�ɂ��ďЉ���Ă��������܂����A��������Ȃ̂ł���΂��Ă��������ˁB"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("���̃��[�h�ł̓s�[�X�𓮂������߂ɁA�����̃s�[�X��͈͑I������K�v������܂��B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("Challenge\n�V�����g���[���[�h�ɐ؂�ւ���B"));

        //�V�����g���[���[�h�̃n�C���C�g���I����
        typeChangersRen[2].sortingOrder = -2;
        typeChangersRen[5].sortingOrder = -2;
        typeChangersCol[2].enabled = true;
        countersRen[2].sortingOrder = -2;
        typeChangerHighlight.transform.position = new Vector3(6.35f, -0.86f, 0);
        typeChangerHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => typeChanger.isTypeChanging);

        //�V�����g���[���[�h�̃n�C���C�g���I�t��
        typeChangerHighlight.color = new Color(1, 1, 1, 0);
        typeChangersRen[2].sortingOrder = -5;
        typeChangersRen[5].sortingOrder = -5;
        typeChangersCol[2].enabled = false;
        countersRen[2].sortingOrder = -5;

        StartCoroutine(SetText("Challenge\n�s�[�X���J�[�\���h���b�O�Ŕ͈͑I������B"));

        //Box10, 1, 2, 7, 6, 0, 8, 9, 5���n�C���C�g&�I���ł���悤��
        boxesRen[10].sortingOrder = -2;
        boxesRen[1].sortingOrder = -2;
        boxesRen[2].sortingOrder = -2;
        boxesRen[7].sortingOrder = -2;
        boxesRen[6].sortingOrder = -2;
        boxesRen[0].sortingOrder = -2;
        boxesRen[8].sortingOrder = -2;
        boxesRen[9].sortingOrder = -2;
        boxesRen[5].sortingOrder = -2;
        boxes9Highlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => CheckSelectedBox(9, 10, 5));

        StartCoroutine(SetText("�I�����ꂽ�s�[�X�B�������傫���Ȃ��Ă���̂��킩��܂����H"));

        //���N���b�N�����Ă�Box�I����Ԃ���������Ȃ��悤��
        type3Mov.canResetBoxSize = false;

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("���̏�ԂŔ͈͑I�����ꂽ�s�[�X�̈�����N���b�N����ƁA���̃s�[�X��\n�I��͈͂̒��őΏۂ̈ʒu�ɂ���s�[�X�Ƃ����ւ��邱�Ƃ��ł��܂��B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("Challenge\n����̃s�[�X���N���b�N���ĉE���̃s�[�X�Ɠ���ւ���B"));

        //�n�C���C�g��Boxes9����Box��
        boxes9Highlight.color = new Color(1, 1, 1, 0);
        boxHighlight.transform.position = new Vector3(-2.25f, 2.25f, 0);
        boxHighlight.size = new Vector3(1.1f, 1.1f, 1);
        boxHighlight.color = new Color(1, 1, 1, 1);

        //Box10���N���b�N�\��
        boxesCol[10].enabled = true;

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => isBoxClickedL);

        isBoxClickedL = false;

        //Box�̃T�C�Y�����ɖ߂�
        type3Mov.canType3Swap = false;
        type3Mov.DeactivateAllBoxes();
        type3Mov.canResetBoxSize = true;

        yield return new WaitForSeconds(1f);

        StartCoroutine(SetText("�����Ɉړ��ł��܂����ˁB"));

        //box�̃n�C���C�g���I�t��
        boxHighlight.color = new Color(1, 1, 1, 0);
        boxesRen[10].sortingOrder = -5;
        boxesRen[1].sortingOrder = -5;
        boxesRen[2].sortingOrder = -5;
        boxesRen[7].sortingOrder = -5;
        boxesRen[6].sortingOrder = -5;
        boxesRen[0].sortingOrder = -5;
        boxesRen[8].sortingOrder = -5;
        boxesRen[9].sortingOrder = -5;
        boxesRen[5].sortingOrder = -5;
        boxesCol[10].enabled = false;

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("������x�����Ă݂܂��傤�B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("Challenge\n�s�[�X���J�[�\���h���b�O�Ŕ͈͑I������B"));

        //Box5, 1, 2, 7, 6, 0���n�C���C�g&�I���\��
        boxesRen[5].sortingOrder = -2;
        boxesRen[1].sortingOrder = -2;
        boxesRen[2].sortingOrder = -2;
        boxesRen[7].sortingOrder = -2;
        boxesRen[6].sortingOrder = -2;
        boxesRen[0].sortingOrder = -2;
        boxes6Highlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => CheckSelectedBox(6, 5, 0));

        StartCoroutine(SetText("Challenge\n����̃s�[�X���N���b�N���ĉE���̃s�[�X�Ɠ���ւ���B"));

        type3Mov.canResetBoxSize = false;
        boxes6Highlight.color = new Color(1, 1, 1, 0);
        boxHighlight.color = new Color(1, 1, 1, 1);
        boxesCol[5].enabled = true;

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
        boxesRen[5].sortingOrder = -5;
        boxesRen[1].sortingOrder = -5;
        boxesRen[2].sortingOrder = -5;
        boxesRen[7].sortingOrder = -5;
        boxesRen[6].sortingOrder = -5;
        boxesRen[0].sortingOrder = -5;
        boxesCol[5].enabled = false;

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("�e���[�h�Ɏg�p�񐔐������݂����Ă���X�e�[�W������̂Œ��ӂ��Ă��������ˁB"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("���ɁA�s�[�X����ёւ���ɂ������ĕ֗��ȋ@�\�����Љ�܂��傤�B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("�����̓y�C���g�c�[���ƌĂ΂����̂ŁA�Ȃ�ƃs�[�X�̐F��ς��邱�Ƃ��ł��܂��B"));

        paintToolsHighlight.color = new Color(1, 1, 1, 1);
        foreach (SpriteRenderer paintToolRen in paintToolsRen)
        {
            paintToolRen.sortingOrder = 0;
        }

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("�y�C���g�c�[�����N���b�N�����܂܁A�ς������s�[�X�̈ʒu�܂Ńh���b�O���Ď��ۂɐF��ς��Ă݂܂��傤�B"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("Challenge\n�y�C���g�c�[�����s�[�X�Ƀh���b�O&�h���b�v����B 0/3"));

        foreach (CircleCollider2D paintToolCol in paintToolsCol)
        {
            paintToolCol.enabled = true;
        }
        foreach (CircleCollider2D paintToolCheckerCol in paintToolCheckersCol)
        {
            paintToolCheckerCol.enabled = true;
        }

        //Boxes�̃n�C���C�g���I����
        for (int i = 0; i < boxes.Length; ++i)
        {
            boxesRen[i].sortingOrder = -2;
        }
        boxesHighlight.color = new Color(1, 1, 1, 1);

        for (int i = 1; i <= threeChallenges; ++i)
        {
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => isDropedPaintTool);
            isDropedPaintTool = false;
            text.SetText("Challenge\n�y�C���g�c�[�����s�[�X�Ƀh���b�O & �h���b�v����B " + i + "/3");
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(SetText("�g�p�񐔂ɐ����͂���܂���̂ŁA���Жڈ�Ƃ��ėL�����p���Ă��������ˁB"));

        foreach (CircleCollider2D paintToolCol in paintToolsCol)
        {
            paintToolCol.enabled = false;
        }
        foreach (CircleCollider2D paintToolCheckerCol in paintToolCheckersCol)
        {
            paintToolCheckerCol.enabled = false;
        }

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("�����܂ŁA�Q�[���̐i�ߕ��ɂ��Ă��������܂������A�Ō�ɃN���A���@��\n���Ă��������܂��B"));

        //Boxes��PaintTools�̃n�C���C�g���I�t��
        boxesHighlight.color = new Color(1, 1, 1, 0);
        for (int i = 0; i < boxes.Length; ++i)
        {
            boxesRen[i].sortingOrder = -5;
        }

        paintToolsHighlight.color = new Color(1, 1, 1, 0);
        foreach (SpriteRenderer paintToolRen in paintToolsRen)
        {
            paintToolRen.sortingOrder = -5;
        }

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("�N���A���@�͂������ăV���v���B�s�[�X���Ȃ̏��Ԃɕ��וς��I������A����TRY�{�^�������������ł��B"));

        //Try�̃n�C���C�g���I����
        tryButtonRen.sortingOrder = -2;
        tryButtonHighlight.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("�����A���������Ԃ̃s�[�X�����בւ����Ă��Ȃ��ƃ{�^���������Ă��N���A�ł��܂���̂Œ��ӂ��Ă��������ˁB"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        StartCoroutine(SetText("�ȏ�ŃQ�[�������͏I���ł��B�撣���Ă��������ˁI"));

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !optionClick.isEnterOption);

        sceneChan.ChangeToTitleScene();

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
