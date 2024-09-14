using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    private float noteSpeed;

    // Start is called before the first frame update
    void Start()
    {
        noteSpeed = GManager.instance.noteSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GManager.instance.isStart)
        {
            transform.position += Vector3.down * Time.deltaTime * noteSpeed;
        }
    }
}
