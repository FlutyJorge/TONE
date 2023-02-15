using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class OnBoxClick : MonoBehaviour
{
    public MovementSystem moveSystem;

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
    
    public void OnClick()
    {
        GameObject clickedObj = eventTrigger.gameObject;
        //Debug.Log(clickedObj);
        moveSystem.ChangeBox(clickedObj);
    }
}
