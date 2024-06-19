using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class JudgeManager : MonoBehaviour
{
    [SerializeField] GameObject[] MessageObj;
    [SerializeField] NoteManager noteMana;
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] TextMeshProUGUI scoreText;
    private int maxLaneNum = 6;
    private PlayerInput playerInput;
    private InputAction[] tapInput = new InputAction[6];
    private InputAction[] holdInput = new InputAction[6];
    private InputAction[] upInput = new InputAction[6];

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        if (!GManager.instance.isStart || noteMana.NotesTime.Count == 0)
        {
            return;
        }

        JudgeTap(0);
        JudgeTap(1);
        JudgeTap(2);
        JudgeTap(3);
        JudgeTap(4);
        JudgeTap(5);
        JudgeHold(0);
        JudgeHold(1);
        JudgeHold(2);
        JudgeHold(3);
        JudgeHold(4);
        JudgeHold(5);
        JudgeUp(0);
        JudgeUp(1);
        JudgeUp(2);
        JudgeUp(3);
        JudgeUp(4);
        JudgeUp(5);

        for (int i = 0; i < maxLaneNum; ++i)
        {
            if (noteMana.NotesTime.Count <= i)
            {
                break;
            }

            JudgeMiss(i);
        }
    }

    private void JudgeTap(int laneNum)
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
                JudgeTiming(GetABS(Time.time - (noteMana.NotesTime[i] + GManager.instance.startTime)), i);
                break;
            }
        }
    }

    private void JudgeHold(int laneNum)
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
                JudgeHoldTiming(noteMana.NotesTime[i], i);
                break;
            }
        }
    }

    private void JudgeUp(int laneNum)
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
                JudgeTiming(GetABS(Time.time - (noteMana.NotesTime[i] + GManager.instance.startTime)), i);
            }
        }
    }

    private void JudgeTiming(float timeLag, int noteIdx)
    {
        bool isHit = false;

        if (timeLag <= 0.05f)
        {
            //Debug.Log("Perfect");
            SetMessage(0, noteIdx);
            GManager.instance.ChangeScore("Perfect");
            isHit = true;
        }
        else if (timeLag <= 0.1f)
        {
            //Debug.Log("Great");
            SetMessage(1, noteIdx);
            GManager.instance.ChangeScore("Great");
            isHit = true;
        }
        else if (timeLag <= 0.15f)
        {
            //Debug.Log("Bad");
            SetMessage(2, noteIdx);
            GManager.instance.ChangeScore("Bad");
            isHit = true;
        }

        if (isHit)
        {
            ShowScore();
            DeleteData(noteIdx);
        }
    }

    private void JudgeHoldTiming(float judgeTime, int noteIdx)
    {
        if (Time.time >= judgeTime + GManager.instance.startTime)
        {
            SetMessage(0, noteIdx);
            GManager.instance.ChangeScore("Perfect");
            DeleteData(noteIdx);
        }
    }

    private float GetABS(float num)
    {
        if (num >= 0)
        {
            return num;
        }
        else
        {
            return -num;
        }
    }

    private void JudgeMiss(int noteIdx)
    {
        float limTime = 0f;

        if (noteMana.NoteType[noteIdx] != 2)
        {
            limTime = noteMana.NotesTime[noteIdx] + 0.15f + GManager.instance.startTime;
        }
        else
        {
            limTime = noteMana.NotesTime[noteIdx] + GManager.instance.startTime;
        }

        if (Time.time > limTime)
        {
            SetMessage(3, noteIdx);
            GManager.instance.ChangeScore("Miss");
            ShowScore();
            DeleteData(noteIdx);
        }
    }

    private void DeleteData(int noteIndx)
    {
        noteMana.NotesTime.RemoveAt(noteIndx);
        noteMana.LaneNum.RemoveAt(noteIndx);
        noteMana.NoteType.RemoveAt(noteIndx);
    }

    private void SetMessage(int judge, int noteIdx)
    {
        Vector3 messagePos = new Vector3((noteMana.LaneNum[noteIdx] - 2.5f) * 2, -3.5f, 0);
        Instantiate(MessageObj[judge], messagePos, Quaternion.identity);
    }

    private void ShowScore()
    {
        comboText.SetText(GManager.instance.combo.ToString());
        scoreText.SetText(GManager.instance.nominalScore.ToString());
    }
}