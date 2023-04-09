using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingMovement : MonoBehaviour
{
    public float amplitude;
    public float amplitudeR;
    public float speed;

    public EventTriggerManager eveManager;
    public Transform trans;

    private Vector3 firstPos;
    private float timer = 0;
    private int seedX, seedY, seedR;

    // Start is called before the first frame update
    void Awake()
    {
        seedX = Random.Range(0, 49);
        seedY = Random.Range(50, 100);
        seedR = Random.Range(0, 100);
        firstPos = trans.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!eveManager.clicked)
        {
            FloatObject();
        }
    }

    private void FloatObject()
    {
        timer += Time.deltaTime;

        float NoiseValueX = amplitude * (Mathf.PerlinNoise(timer * speed, seedX) - 0.5f);
        float NoiseValueY = amplitude * (Mathf.PerlinNoise(timer * speed, seedY) - 0.5f);
        float NoiseValueR = amplitude * (Mathf.PerlinNoise(timer * speed, seedR) - 0.5f);

        trans.position = new Vector2(firstPos.x + NoiseValueX, firstPos.y + NoiseValueY);
        trans.rotation = Quaternion.Euler(0, 0, this.gameObject.transform.rotation.z + NoiseValueR * amplitudeR);
    }
}
