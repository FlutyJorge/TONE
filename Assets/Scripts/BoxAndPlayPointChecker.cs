using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BoxAndPlayPointChecker : MonoBehaviour
{
    public CommonMovement comMov;
    [SerializeField] SoundManager sManager;

    [Space(10)]
    [Header("Boxのみアタッチが必要")]
    [SerializeField] GameObject play;
    [SerializeField] GameObject stop;

    private EventTrigger eventTrigger;

    // Start is called before the first frame update
    void Start()
    {
        eventTrigger = GetComponent<EventTrigger>();
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
            GameObject clickedObj = eventTrigger.gameObject;
            sManager.PlaySepaSound(clickedObj, play, stop);
        }
    }

    public void ClickPlayPoint()
    {
        int playPointNum = int.Parse(eventTrigger.gameObject.name);
        sManager.PlayFromPoint(playPointNum);
    }
}
