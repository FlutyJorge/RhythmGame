using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class ObjectScaler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeScale(GameObject obj, float size, float scaleTime)
    {
        obj.transform.DOScale(new Vector3(size, size, 0), scaleTime);
    }
}
