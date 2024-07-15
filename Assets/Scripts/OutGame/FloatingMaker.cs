using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingMaker : MonoBehaviour
{
    [Header("浮遊オブジェクトの振れ幅、スピード")]
    public float amplitude;
    public float amplitudeR;
    public float speed;

    private Vector3 firstPos;
    private float firstRotateZ;
    private int seedX, seedY, seedR;

    void Start()
    {
        //RandomRangeで指定する値は適当でよい。ただし、seedXとseedYは動きの重複を避けるために別々の値を指定する
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
        //PerlinNoiseは0～1の値をとるため、0.5で除算して-0.5～0.5の値をとるようにし、動作の偏りを避ける
        float NoiseValueX = amplitude * (Mathf.PerlinNoise(seedX, Time.time * speed) - 0.5f);
        float NoiseValueY = amplitude * (Mathf.PerlinNoise(Time.time * speed, seedY) - 0.5f);
        float NoiseValueR = amplitudeR * (Mathf.PerlinNoise(Time.time * speed, seedR) - 0.5f);

        transform.position = new Vector2(firstPos.x + NoiseValueX, firstPos.y + NoiseValueY);
        transform.rotation = Quaternion.Euler(0, 0, firstRotateZ + NoiseValueR);
    }
}
