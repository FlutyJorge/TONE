using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingMovement : MonoBehaviour
{
    [Header("浮遊オブジェクトの振れ幅、スピード")]
    public float amplitude;
    public float amplitudeR;
    public float speed;

    public Transform trans;

    private Vector3 firstPos;
    private float timer = 0;
    private int seedX, seedY, seedR;

    void Awake()
    {
        //RandomRangeで指定する値は適当でよい。ただし、seedXとseedYは動きの重複を避けるために別々の値を指定する
        seedX = Random.Range(0, 49);
        seedY = Random.Range(50, 100);
        seedR = Random.Range(0, 100);
        firstPos = trans.position;
    }

    void Update()
    {
        FloatObject();
    }

    private void FloatObject()
    {
        timer += Time.deltaTime;

        //PerlinNoiseは0～1の値をとるため、0.5で除算して-0.5～0.5の値をとるようにし、動作の偏りを避ける
        float NoiseValueX = amplitude * (Mathf.PerlinNoise(seedX, timer * speed) - 0.5f);
        float NoiseValueY = amplitude * (Mathf.PerlinNoise(timer * speed, seedY) - 0.5f);
        float NoiseValueR = amplitude * (Mathf.PerlinNoise(timer * speed, seedR) - 0.5f);

        trans.position = new Vector2(firstPos.x + NoiseValueX, firstPos.y + NoiseValueY);
        trans.rotation = Quaternion.Euler(0, 0, this.gameObject.transform.rotation.z + NoiseValueR * amplitudeR);
    }
}
