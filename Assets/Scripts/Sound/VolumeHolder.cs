using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeHolder : MonoBehaviour
{
    static public VolumeHolder instance;
    public float musicVolume;
    public float SEVolume;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
