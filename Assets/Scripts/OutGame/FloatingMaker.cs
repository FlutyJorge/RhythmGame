using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingMaker : MonoBehaviour
{
    [Header("���V�I�u�W�F�N�g�̐U�ꕝ�A�X�s�[�h")]
    public float amplitude;
    public float amplitudeR;
    public float speed;

    private Vector3 firstPos;
    private float firstRotateZ;
    private int seedX, seedY, seedR;

    void Start()
    {
        //RandomRange�Ŏw�肷��l�͓K���ł悢�B�������AseedX��seedY�͓����̏d��������邽�߂ɕʁX�̒l���w�肷��
        seedX = Random.Range(0, 49);
        seedY = Random.Range(50, 100);
        seedR = Random.Range(0, 100);
        firstPos = transform.position;
        firstRotateZ = transform.rotation.z;
    }

    void Update()
    {
        FloatObject();
    }

    private void FloatObject()
    {
        //PerlinNoise��0�`1�̒l���Ƃ邽�߁A0.5�ŏ��Z����-0.5�`0.5�̒l���Ƃ�悤�ɂ��A����̕΂�������
        float NoiseValueX = amplitude * (Mathf.PerlinNoise(seedX, Time.time * speed) - 0.5f);
        float NoiseValueY = amplitude * (Mathf.PerlinNoise(Time.time * speed, seedY) - 0.5f);
        float NoiseValueR = amplitudeR * (Mathf.PerlinNoise(Time.time * speed, seedR) - 0.5f);

        transform.position = new Vector2(firstPos.x + NoiseValueX, firstPos.y + NoiseValueY);
        transform.rotation = Quaternion.Euler(0, 0, firstRotateZ + NoiseValueR);
    }
}
