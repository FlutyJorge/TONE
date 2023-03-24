using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ClickObject : MonoBehaviour
{
    public CommonMovement comMov;
    public SoundManager sManager;
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
        GameObject clickedObj = eventTrigger.gameObject;
        comMov.ChangeBox(clickedObj);
    }

    public void OnSoundButtonClick()
    {
        GameObject clickedObj = eventTrigger.gameObject;
        sManager.PlaySound(clickedObj);
    }

    public void MoveUp()
    {
        //clicked = true;
        Debug.Log("ê¨å˜");
        cam.transform.DOMove(new Vector3(0, 10, -10), 1.5f).SetEase(Ease.InOutBack);
    }
}
