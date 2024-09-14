using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JudgeRunnerS : MonoBehaviour
{
    [Header("アタッチ")]
    [SerializeField] GameObject barL;
    [SerializeField] GameObject barR;

    private JudgeManager judgeMana;
    private NoteManager noteMana;

    private int maxLaneNum = 6;

    private PlayerInput playerInput;
    private InputAction tapL;
    private InputAction tapR;
    private InputAction holdL;
    private InputAction holdR;
    private InputAction upL;
    private InputAction upR;

    private float[] barArea = { -6, -4, -2, 0, 2, 4, 6 };

    // Start is called before the first frame update
    void Start()
    {
        judgeMana = GetComponent<JudgeManager>();
        noteMana = judgeMana.noteMana;

        playerInput = new PlayerInput();
        playerInput.Enable();
        tapL = playerInput.InGame.TapL;
        tapR = playerInput.InGame.TapR;
        holdL = playerInput.InGame.HoldL;
        holdR = playerInput.InGame.HoldR;
        upL = playerInput.InGame.UpL;
        upR = playerInput.InGame.UpR;
    }

    public void JudgeTap(int laneNum, bool isLeft)
    {
        if (isLeft && !tapL.triggered)
        {
            return;
        }
        else if (!isLeft && !tapR.triggered)
        {
            return;
        }

        for (int i = 0; i < maxLaneNum; ++i)
        {
            if (noteMana.LaneNum.Count <= i)
            {
                break;
            }

            if (noteMana.NoteType[i] == 2 || noteMana.NoteType[i] == 4)
            {
                continue;
            }

            if (noteMana.LaneNum[i] == laneNum && ReturnBarLane(isLeft) == laneNum)
            {
                float timeLag = Time.time - (noteMana.NotesTime[i] + GManager.instance.startTime);
                judgeMana.JudgeTiming(timeLag, i);
                break;
            }
        }
    }

    public void JudgeHold(int laneNum, bool isLeft)
    {
        if (isLeft && !holdL.IsPressed())
        {
            return;
        }
        else if (!isLeft && !holdR.IsPressed())
        {
            return;
        }

        for (int i = 0; i < 6; ++i)
        {
            if (noteMana.LaneNum.Count <= i)
            {
                break;
            }

            if (noteMana.NoteType[i] != 2)
            {
                continue;
            }

            if (noteMana.LaneNum[i] == laneNum && ReturnBarLane(isLeft) == laneNum)
            {
                judgeMana.JudgeHoldTiming(noteMana.NotesTime[i], i);
                break;
            }
        }
    }

    public void JudgeUp(int laneNum, bool isLeft)
    {
        if (isLeft && !upL.triggered)
        {
            return;
        }
        else if (!isLeft && !upR.triggered)
        {
            return;
        }

        for (int i = 0; i < 6; ++i)
        {
            if (noteMana.LaneNum.Count <= i)
            {
                break;
            }

            if (noteMana.NoteType[i] != 4)
            {
                continue;
            }

            if (noteMana.LaneNum[i] == laneNum && ReturnBarLane(isLeft) == laneNum)
            {
                float timeLag = Time.time - (noteMana.NotesTime[i] + GManager.instance.startTime);
                judgeMana.JudgeTiming(judgeMana.GetABS(timeLag), i);
                break;
            }
        }
    }

    private int ReturnBarLane(bool isLeft)
    {
        float barPos = 0;
        if (isLeft)
        {
            barPos = barL.transform.position.x;
            for (int i = 0; i <= 2; ++i)
            {
                if (barPos > barArea[i] && barPos <= barArea[i + 1])
                {
                    return i;
                }
            }
        }
        else
        {
            barPos = barR.transform.position.x;
            for (int i = 3; i <= 5; ++i)
            {
                if (barPos > barArea[i] && barPos <= barArea[i + 1])
                {
                    return i;
                }
            }
        }

        Debug.Log("バーの位置取得ができてない");
        return -1;
    }
}
