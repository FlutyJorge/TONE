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

    void Start()
    {
        eveTrigger = GetComponent<EventTrigger>();
    }

    private void Update()
    {
        if (sManager.isSongFinished)
        {
            sManager.isSongFinished = false;
            StartCoroutine(sManager.ChangeFlagAndScale(false, play, stop, 1, 0));
        }
    }

    public void ClickPlayAndStopButton()
    {
        if (!sManager.isButtonChanging)
        {
            sManager.PlayAndStopSong(eveTrigger, play, stop, reset);
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
