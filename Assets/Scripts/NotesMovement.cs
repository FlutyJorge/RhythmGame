using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesMovement : MonoBehaviour
{
    public float notesSpeed;
    private bool isStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(transform.position);
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
            transform.position += Vector3.down * Time.deltaTime * notesSpeed;
        }

        //transform.position += Vector3.down * Time.deltaTime * notesSpeed;
    }
}
