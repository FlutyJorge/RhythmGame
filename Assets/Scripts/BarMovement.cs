using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BarMovement : MonoBehaviour
{
    [SerializeField] bool isLeft;
    [SerializeField] float speedCoef;
    private PlayerInput playerInput;
    private float leftMin = -5;
    private float leftMax = -1;
    private float rightMin = 1;
    private float rightMax = 5;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GManager.instance.isStart)
        {
            return;
        }

        if (playerInput.InGame.BarLeft.IsPressed() && isLeft)
        {
            Vector2 stickValueL = playerInput.InGame.BarLeft.ReadValue<Vector2>();

            if (stickValueL.x < 0 && transform.position.x > leftMin)
            {
                ChangePos(stickValueL);
            }
            else if (stickValueL.x > 0 && transform.position.x < leftMax)
            {
                ChangePos(stickValueL);
            }
        }

        if (playerInput.InGame.BarRight.IsPressed() && !isLeft)
        {
            Vector2 stickValueR = playerInput.InGame.BarRight.ReadValue<Vector2>();

            if (stickValueR.x < 0 && transform.position.x > rightMin)
            {
                ChangePos(stickValueR);
            }
            else if (stickValueR.x > 0 && transform.position.x < rightMax)
            {
                ChangePos(stickValueR);
            }
        }
    }

    private void ChangePos(Vector2 stickValue)
    {
        Vector3 direction = new Vector3(stickValue.x, 0, 0);
        transform.position += direction * Time.deltaTime * GManager.instance.noteSpeed * speedCoef;
    }
}
