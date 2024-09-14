using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    [SerializeField] FadeManager fadeMana;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject startObj;
    [SerializeField] GameObject[] stageObj = new GameObject[3];
    public float pushScale;
    public float scaleTime;

    private PlayerInput playerInput;
    private ObjectScaler objScaler;
    private bool isTitle = true; // タイトル、ステージセレクト画面判別
    private bool isPress = false; //ステージボタンを押したまま移動した時用
    private bool isStageChange = false;
    private bool isCamMoving = false; //カメラ移動時、連打防止用
    private int stageNum = 3;
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
        if (fadeMana.isFading)
        {
            return;
        }

        if (isTitle && !isCamMoving)
        {
            StartCoroutine(PressStart());
        }

        if (!isTitle)
        {
            SelectStage();
            PushStage();
        }

        if (isStageChange)
        {
            isStageChange = false;
            StartCoroutine(fadeMana.FadeAndChangeScene(0, 1));
        }
    }

    private IEnumerator PressStart()
    {
        if (playerInput.OutGame.StartPress.triggered)
        {
            objScaler.ChangeScale(startObj, pushScale, scaleTime);
        }

        if (playerInput.OutGame.StartRelease.triggered)
        {
            isCamMoving = true;
            cam.transform.DOMoveY(-20, 1.5f).SetEase(Ease.InOutBack);
            objScaler.ChangeScale(startObj, 1, scaleTime);

            yield return new WaitForSeconds(1.5f);

            Debug.Log("ステージセレクト移動");
            isCamMoving = false;
            isTitle = false;
        }

        yield break;
    }

    private void SelectStage()
    {
        if (!playerInput.OutGame.Select.triggered)
        {
            return;
        }

        Vector2 inpValue = playerInput.OutGame.Select.ReadValue<Vector2>();

        int nextStage = 0;
        bool isChange = false;
        for (int i = 0; i < stageNum; ++i)
        {
            if (!stageTrigger[i])
            {
                continue;
            }

            if (inpValue.x == 1)
            {
                isChange = true;
                nextStage = i + 1;
            }
            else if (inpValue.x == -1)
            {
                isChange = true;
                nextStage = i - 1;
            }

            //ステージの端から移動する時
            if (nextStage == stageNum)
            {
                nextStage = 0;
            }
            else if (nextStage == -1)
            {
                nextStage = stageNum - 1;
            }

            if (isChange)
            {
                objScaler.ChangeScale(stageObj[i], 1, scaleTime);
                objScaler.ChangeScale(stageObj[nextStage], 1.2f, scaleTime);
                stageTrigger[i] = false;
                stageTrigger[nextStage] = true;
                isPress = false;
                break;
            }
        }
    }

    private void PushStage()
    {
        int targetStage = -1;

        for (int i = 0; i < stageNum; ++i)
        {
            if (stageTrigger[i])
            {
                targetStage = i;
                break;
            }
        }

        if (playerInput.OutGame.StartPress.triggered)
        {
            isPress = true;
            objScaler.ChangeScale(stageObj[targetStage], 1, scaleTime);
        }

        if (playerInput.OutGame.StartRelease.triggered)
        {
            objScaler.ChangeScale(stageObj[targetStage], 1.2f, scaleTime);

            if (isPress)
            {
                isStageChange = true;
            }
        }
    }
}
