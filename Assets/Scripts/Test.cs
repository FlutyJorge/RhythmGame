using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float notesSpeed = 8;
    public bool isStarted;
    // Start is called before the first frame update
    void Start()
    {
        
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
            this.gameObject.transform.position += Vector3.down * Time.deltaTime * notesSpeed;
        }
    }
}
