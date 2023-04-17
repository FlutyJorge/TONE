using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ClearManager : MonoBehaviour
{
    public GameObject clearBoard;
    public GameObject clearBackground;
    public ParticleSystem[] particles = new ParticleSystem[8];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickTry()
    {
        StartCoroutine(ShowClearScene());
    }

    private IEnumerator ShowClearScene()
    {
        clearBackground.GetComponent<BoxCollider2D>().enabled = true;
        clearBackground.GetComponent<SpriteRenderer>().DOFade(1f, 3f);
        yield return new WaitForSeconds(3f);
        clearBoard.transform.DOMove(new Vector3(0, 0, 0), 1f).SetEase(Ease.InOutBack);
        yield return new WaitForSeconds(1f);
        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }
        yield break;
    }
}
