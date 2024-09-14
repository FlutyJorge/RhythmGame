using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JudgeRunnerN : MonoBehaviour
{
    private NoteManager noteMana;
    private JudgeManager judgeMana;

    private int maxLaneNum = 6;

    //プレイヤーの入力装置
    private PlayerInput playerInput;
    private InputAction[] tapInput = new InputAction[6];
    private InputAction[] holdInput = new InputAction[6];
    private InputAction[] upInput = new InputAction[6];

    // Start is called before the first frame update
    void Start()
    {
        judgeMana = GetComponent<JudgeManager>();
        noteMana = judgeMana.noteMana;
        playerInput = new PlayerInput();
        playerInput.Enable();

        tapInput[0] = playerInput.InGame.Tap0;
        tapInput[1] = playerInput.InGame.Tap1;
        tapInput[2] = playerInput.InGame.Tap2;
        tapInput[3] = playerInput.InGame.Tap3;
        tapInput[4] = playerInput.InGame.Tap4;
        tapInput[5] = playerInput.InGame.Tap5;
        holdInput[0] = playerInput.InGame.Hold0;
        holdInput[1] = playerInput.InGame.Hold1;
        holdInput[2] = playerInput.InGame.Hold2;
        holdInput[3] = playerInput.InGame.Hold3;
        holdInput[4] = playerInput.InGame.Hold4;
        holdInput[5] = playerInput.InGame.Hold5;
        upInput[0] = playerInput.InGame.Up0;
        upInput[1] = playerInput.InGame.Up1;
        upInput[2] = playerInput.InGame.Up2;
        upInput[3] = playerInput.InGame.Up3;
        upInput[4] = playerInput.InGame.Up4;
        upInput[5] = playerInput.InGame.Up5;
    }

    public void JudgeTap(int laneNum)
    {
        if (!tapInput[laneNum].triggered || noteMana.LaneNum.Count == 0)
        {
            return;
        }

        for (int i = 0; i < maxLaneNum; ++i)
        {
            if (noteMana.LaneNum.Count <= i)
            {
                break;
            }

            if (noteMana.LaneNum[i] == laneNum && (noteMana.NoteType[i] == 1 || noteMana.NoteType[i] == 3))
            {
                float timeLag = Time.time - (noteMana.NotesTime[i] + GManager.instance.startTime);
                judgeMana.JudgeTiming(judgeMana.GetABS(timeLag), i);
                break;
            }
        }
    }

    public void JudgeHold(int laneNum)
    {
        if (!holdInput[laneNum].IsPressed())
        {
            return;
        }

        for (int i = 0; i < maxLaneNum; ++i)
        {
            if (noteMana.LaneNum.Count <= i)
            {
                break;
            }

            if (noteMana.LaneNum[i] == laneNum && noteMana.NoteType[i] == 2)
            {
                judgeMana.JudgeHoldTiming(noteMana.NotesTime[i], i);
                break;
            }
        }
    }

    public void JudgeUp(int laneNum)
    {
        if (!upInput[laneNum].triggered)
        {
            return;
        }

        for (int i = 0; i < maxLaneNum; ++i)
        {
            if (noteMana.LaneNum.Count <= i)
            {
                break;
            }

            if (noteMana.LaneNum[i] == laneNum && noteMana.NoteType[i] == 4)
            {
                float timeLag = Time.time - (noteMana.NotesTime[i] + GManager.instance.startTime);
                judgeMana.JudgeTiming(judgeMana.GetABS(timeLag), i);
                break;
            }
        }
    }
}