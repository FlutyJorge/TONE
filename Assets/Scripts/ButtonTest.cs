using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[2];
    [SerializeField] SpriteRenderer sRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            sRenderer.sprite = sprites[0];
        }
        else if (Input.GetMouseButtonDown(1))
        {
            sRenderer.sprite = sprites[1];
        }
    }
}
