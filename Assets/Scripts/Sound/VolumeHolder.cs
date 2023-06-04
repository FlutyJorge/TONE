using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeHolder : MonoBehaviour
{
    static public VolumeHolder instance;
    public float musicVolume;
    public float SEVolume;

    //シングルトンにより、シーン変更が生じた際も音量がこのクラスで保持される
    //他のスクリプトがStartでこのスクリプトを参照する前にVolumeHolderのインスタンスを確定させる
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
}
