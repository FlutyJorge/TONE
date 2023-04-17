using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TryButton : MonoBehaviour
{
    private EventTrigger eveTrigger;
    private bool isSizeChaning = false;

    // Start is called before the first frame update
    void Start()
    {
        eveTrigger = GetComponent<EventTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Coroutine(float size)
    {
        StartCoroutine(ChangeObjectSize(size));
    }

    public IEnumerator ChangeObjectSize(float size)
    {
        if (!isSizeChaning)
        {
            isSizeChaning = true;
            eveTrigger.gameObject.transform.DOScale(new Vector2(size, size), 0.2f);
            yield return new WaitForSeconds(0.2f);
            isSizeChaning = false;
            yield break;
        }
    }

    public void ResetObjectSize(float size)
    {
        eveTrigger.gameObject.transform.DOScale(new Vector2(size, size), 0.1f);
    }
}
