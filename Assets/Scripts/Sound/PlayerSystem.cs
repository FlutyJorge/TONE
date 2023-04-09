using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PlayerSystem : MonoBehaviour
{
    [SerializeField] SoundManager sManager;
    [SerializeField] GameObject play;
    [SerializeField] GameObject stop;
    [SerializeField] GameObject reset;

    private EventTrigger eveTrigger;

    // Start is called before the first frame update
    void Start()
    {
        eveTrigger = GetComponent<EventTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePlayerButtonSize(float size)
    {
        if (!sManager.isButtonChanging)
        {
            eveTrigger.gameObject.transform.DOScale(new Vector2(size, size), 0.2f);
        }
    }

    public void ClickPlayAndStopButton()
    {
        if (!sManager.isButtonChanging)
        {
            StartCoroutine(sManager.PlayAndStopSong(eveTrigger, play, stop, reset));
        }
    }
    public void ChangeResetButtonSize(float size)
    {
        if (!sManager.isButtonChanging)
        {
            eveTrigger.gameObject.transform.DOScale(new Vector2(size, size), 0.1f);
        }
    }

}
