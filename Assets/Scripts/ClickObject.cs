using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ClickObject : MonoBehaviour
{
    public CommonMovement comMov;
    public SoundManager sManager;

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SoundArea")
        {
            Debug.Log("a");
        }
    }

    public void OnSoundButtonClick()
    {
        GameObject clickedObj = eventTrigger.gameObject;
        sManager.PlaySound(clickedObj);

    }
}
