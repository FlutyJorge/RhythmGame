using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    private float noteSpeed;
    private bool isStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        noteSpeed = GManager.instance.noteSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Movement");
            isStarted = true;
        }

        if (isStarted)
        {
            transform.position += Vector3.down * Time.deltaTime * noteSpeed;
        }
    }
}
