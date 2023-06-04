using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CameraSlider : MonoBehaviour
{
    private EventTrigger eveTrigger;
    private bool isClicked = false;

    void Start()
    {
        eveTrigger = GetComponent<EventTrigger>();
    }

    public void SlideCamera(GameObject cam)
    {
        if (eveTrigger.gameObject.name == "StartForTitle" && !isClicked)
        {
            isClicked = true;
            cam.transform.DOMove(new Vector3(20, 0, -10), 1.5f).SetEase(Ease.InOutBack);
            StartCoroutine(SetInterval(1.5f));
        }
        else if (eveTrigger.gameObject.name == "Back" && !isClicked)
        {
            isClicked = true;
            cam.transform.DOMove(new Vector3(0, 0, -10), 1.5f).SetEase(Ease.InOutBack);
            StartCoroutine(SetInterval(1.5f));
        }
    }

    //連打防止用にインターバルを設ける関数
    private IEnumerator SetInterval(float intervalTime)
    {
        yield return new WaitForSeconds(intervalTime);
        isClicked = false;
    }
}
