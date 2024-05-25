using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesMovement : MonoBehaviour
{
    public float notesSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position += Vector3.down * Time.deltaTime * notesSpeed;
    }
}
