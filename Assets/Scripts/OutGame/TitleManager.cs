using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject cam;
    [SerializeField] GameObject startObj;
    public float scaleSize;
    public float scaleTime;

    private PlayerInput playerInput;
    private ObjectScaler objScaler;
    private bool isTitle = true;
    private bool[] stageTrigger = new bool[3] { true, false, false };

    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();
        objScaler = GetComponent<ObjectScaler>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(PressStart());
        SelectStage();
    }

    private IEnumerator PressStart()
    {
        if (!isTitle)
        {
            yield break;
        }

        if (playerInput.OutGame.StartPress.triggered)
        {
            objScaler.ChangeScale(startObj, scaleSize, scaleTime);
        }

        if (playerInput.OutGame.StartRelease.triggered)
        {
            cam.transform.DOMoveY(-20, 1.5f).SetEase(Ease.InOutBack);
            objScaler.ChangeScale(startObj, 1, scaleTime);

            yield return new WaitForSeconds(1.5f);

            Debug.Log("ステージセレクト移動");
            isTitle = false;
        }

        yield break;
    }

    private void SelectStage()
    {
        if (isTitle || playerInput.OutGame.Select.triggered)
        {
            return;
        }

        Vector2 inpValue = playerInput.OutGame.Select.ReadValue<Vector2>();

        if (inpValue.x >= inpValue.y)
        {
            Debug.Log("横");
        }
        else
        {
            Debug.Log("縦");
        }
    }
}
