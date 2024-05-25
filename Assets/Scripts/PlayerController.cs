using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInp;

    // Start is called before the first frame update
    void Start()
    {
        playerInp = new PlayerInput();
        playerInp.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInp.Player.Fire.triggered)
        {
            Debug.Log("ファイアー！");
        }
    }
}
