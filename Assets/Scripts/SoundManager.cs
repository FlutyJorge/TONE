using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public CommonMovement comMov;
    [SerializeField] AudioSource[] sounds;
    [SerializeField] GameObject[] lockedButton;
    [SerializeField] bool isLockedArea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int GetSoundButtonNum(GameObject clickedObj)
    {
        int ret = int.Parse(clickedObj.name);

        return ret;
    }

    public void PlaySound(GameObject clickedObj)
    {
        if (!isLockedArea)
        {
            int num = GetSoundButtonNum(clickedObj);
            sounds[num].Play();
        }
        else
        {
            int idx = -1;

            for (int i = 0; i < lockedButton.Length; ++i)
            {
                if (lockedButton[i] == null)
                {
                    continue;
                }

                if (clickedObj == lockedButton[i])
                {
                    idx = i;
                    break;
                }
            }

            int num = int.Parse(comMov.boxes[idx].name);
            sounds[num].Play();
        }
    }
}
