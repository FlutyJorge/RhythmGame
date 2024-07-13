using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.DOMove(new Vector3(5f, 0f, 0f), 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
