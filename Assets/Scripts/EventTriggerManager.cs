using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class EventTriggerManager : MonoBehaviour
{
    public CommonMovement comMov;
    [SerializeField] SoundManager sManager;
    [SerializeField] TypeChanger typeChanger;
    [SerializeField] GameObject play;
    [SerializeField] GameObject stop;
    private bool isclicked = false;

    private EventTrigger eventTrigger;

    // Start is called before the first frame update
    void Start()
    {
        eventTrigger = GetComponent<EventTrigger>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnBoxClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!comMov.swaping)
            {
                GameObject clickedObj = eventTrigger.gameObject;
                comMov.ChangeBox(clickedObj);
            }
            else
            {
                Debug.Log("スワップ中！");
            }
        }
    }

    public void OnSoundButtonClick()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("通貨");
            GameObject clickedObj = eventTrigger.gameObject;
            sManager.PlaySepaSound(clickedObj, play, stop);
        }
    }

    public void SlideCamera(GameObject cam)
    {
        if (eventTrigger.gameObject.name == "StartForTitle" && !isclicked)
        {
            isclicked = true;
            cam.transform.DOMove(new Vector3(20, 0, -10), 1.5f).SetEase(Ease.InOutBack);
            StartCoroutine(SetInterval(1.5f));
        }
        else if (eventTrigger.gameObject.name == "Back" && !isclicked)
        {
            isclicked = true;
            cam.transform.DOMove(new Vector3(0, 0, -10), 1.5f).SetEase(Ease.InOutBack);
            StartCoroutine(SetInterval(1.5f));
        }
    }

    public void ClickPlayPoint()
    {
        //Debug.Log("成功");
        int playPointNum = int.Parse(eventTrigger.gameObject.name);
        sManager.PlayFromPoint(playPointNum);
    }

    //PointerEnter&Exit用
    public void ChangeObjectSize(float size)
    {
        eventTrigger.gameObject.transform.DOScale(new Vector2(size, size), 0.1f);
    }

    //連打防止用にインターバルを設ける関数
    private IEnumerator SetInterval(float intervalTime)
    {
        yield return new WaitForSeconds(intervalTime);
        isclicked = false;
    }
}
