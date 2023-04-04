using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ClickObject : MonoBehaviour
{
    public CommonMovement comMov;
    public SoundManager sManager;
    [SerializeField] SoundManager2 sManager2;
    [SerializeField] TypeChanger typeChanger;
    public Camera cam;
    public bool clicked = false;

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

    public void OnSoundButtonClick()
    {
        GameObject clickedObj = eventTrigger.gameObject;
        //sManager.PlaySound(clickedObj);
        sManager2.PlaySepaSound(clickedObj);
    }

    public void MoveUp()
    {
        //clicked = true;
        Debug.Log("成功");
        cam.transform.DOMove(new Vector3(0, 10, -10), 1.5f).SetEase(Ease.InOutBack);
    }

    public void ClickPlayPoint()
    {
        //Debug.Log("成功");
        int playPointNum = int.Parse(eventTrigger.gameObject.name);
        sManager2.PlayPointButton(playPointNum);
    }

    //PointerEnter&Exit用
    public void ChangeObjectSize(float size)
    {
        if (!typeChanger.isChanging)
        {
            eventTrigger.gameObject.transform.DOScale(new Vector2(size, size), 0.2f);
        }
    }
}
