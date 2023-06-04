using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingMovement : MonoBehaviour
{
    [Header("���V�I�u�W�F�N�g�̐U�ꕝ�A�X�s�[�h")]
    public float amplitude;
    public float amplitudeR;
    public float speed;

    public Transform trans;

    private Vector3 firstPos;
    private float timer = 0;
    private int seedX, seedY, seedR;

    void Awake()
    {
        //RandomRange�Ŏw�肷��l�͓K���ł悢�B�������AseedX��seedY�͓����̏d��������邽�߂ɕʁX�̒l���w�肷��
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

        //PerlinNoise��0�`1�̒l���Ƃ邽�߁A0.5�ŏ��Z����-0.5�`0.5�̒l���Ƃ�悤�ɂ��A����̕΂�������
        float NoiseValueX = amplitude * (Mathf.PerlinNoise(seedX, timer * speed) - 0.5f);
        float NoiseValueY = amplitude * (Mathf.PerlinNoise(timer * speed, seedY) - 0.5f);
        float NoiseValueR = amplitude * (Mathf.PerlinNoise(timer * speed, seedR) - 0.5f);

        trans.position = new Vector2(firstPos.x + NoiseValueX, firstPos.y + NoiseValueY);
        trans.rotation = Quaternion.Euler(0, 0, this.gameObject.transform.rotation.z + NoiseValueR * amplitudeR);
    }
}
