using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JudgeManager : MonoBehaviour
{
    [SerializeField] GameObject[] MessageObj;
    [SerializeField] NoteManager noteMana;
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] TextMeshProUGUI scoreText;

    //ロングノーツの長押し部分判定用
    [SerializeField] private bool[] lNoteTrigger = new bool[6] { false, false, false, false, false, false };
    int[] lNoteCounter = new int[6] { 0, 0, 0, 0, 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GManager.instance.isStart || noteMana.NotesTime.Count == 0)
        {
            return;
        }

        JudgeTap(KeyCode.D, 0);
        JudgeTap(KeyCode.F, 1);
        JudgeTap(KeyCode.G, 2);
        JudgeTap(KeyCode.H, 3);
        JudgeTap(KeyCode.J, 4);
        JudgeTap(KeyCode.K, 5);
        JudgeHold(KeyCode.D);
        JudgeHold(KeyCode.F);
        JudgeHold(KeyCode.G);
        JudgeHold(KeyCode.H);
        JudgeHold(KeyCode.J);
        JudgeHold(KeyCode.K);
        JudgeUp(KeyCode.D, 0);
        JudgeUp(KeyCode.F, 1);
        JudgeUp(KeyCode.G, 2);
        JudgeUp(KeyCode.H, 3);
        JudgeUp(KeyCode.J, 4);
        JudgeUp(KeyCode.K, 5);

        //ミス判定
        if (noteMana.NotesTime.Count == 0)
        {
            return;
        }

        for (int i = 0; i <= 5; ++i)
        {
            JudgeHoldMiss(i);
        }

        float limTime1 = noteMana.NotesTime[0] + 0.15f + GManager.instance.startTime;
        JudgeTapMiss(limTime1, 0);

        if (noteMana.NotesTime.Count < 2)
        {
            return;
        }

        float limTime2 = noteMana.NotesTime[1] + 0.15f + GManager.instance.startTime;
        JudgeTapMiss(limTime2, 1);
    }

    private void JudgeTap(KeyCode keyCode, int laneNum)
    {
        if (!Input.GetKeyDown(keyCode) || noteMana.LaneNum.Count == 0)
        {
            return;
        }

        if (noteMana.LaneNum[0] == laneNum && noteMana.NoteType[0] != 3)
        {
            JudgeTiming(GetABS(Time.time - (noteMana.NotesTime[0] + GManager.instance.startTime)), 0, true);
            if (noteMana.NoteType.Count != 0 && noteMana.NoteType[0] == 2)
            {
                lNoteTrigger[noteMana.LaneNum[0]] = true;
                ++lNoteCounter[noteMana.LaneNum[0]];
            }
        }
        else if (noteMana.LaneNum[1] == laneNum && noteMana.NoteType[1] != 3)
        {
            JudgeTiming(GetABS(Time.time - (noteMana.NotesTime[1] + GManager.instance.startTime)), 1, true);
            if (noteMana.NoteType.Count != 0 && noteMana.NoteType[0] == 2)
            {
                lNoteTrigger[noteMana.LaneNum[1]] = true;
                ++lNoteCounter[noteMana.LaneNum[1]];
            }
        }
    }

    private void JudgeHold(KeyCode keyCode)
    {
        if (!Input.GetKey(keyCode))
        {
            return;
        }

        for (int i = 0; i <= 5; ++i)
        {
            if (!lNoteTrigger[i] || noteMana.holdTiming[i].Count == 0)
            {
                continue;
            }

            JudgeHoldTiming(noteMana.holdTiming[i][0], i);
            break;
        }
    }

    private void JudgeUp(KeyCode keyCode, int laneNum)
    {
        if (!Input.GetKeyUp(keyCode))
        {
            return;
        }

        if (noteMana.LaneNum[0] == laneNum && noteMana.NoteType[0] == 3)
        {
            JudgeTiming(GetABS(Time.time - (noteMana.NotesTime[0] + GManager.instance.startTime)), 0, false);
        }
        else if (noteMana.LaneNum[1] == laneNum && noteMana.NoteType[1] != 3)
        {
            JudgeTiming(GetABS(Time.time - (noteMana.NotesTime[1] + GManager.instance.startTime)), 1, false);
        }
    }

    private void JudgeTiming(float timeLag, int noteIdx, bool isTap)
    {
        bool isHit = false;

        if (timeLag <= 0.05f)
        {
            //Debug.Log("Perfect");
            SetTapMessage(0, noteIdx);
            GManager.instance.ChangeScore("Perfect");
            isHit = true;
        }
        else if (timeLag <= 0.1f)
        {
            //Debug.Log("Great");
            SetTapMessage(1, noteIdx);
            GManager.instance.ChangeScore("Great");
            isHit = true;
        }
        else if (timeLag <= 0.15f)
        {
            //Debug.Log("Bad");
            SetTapMessage(2, noteIdx);
            GManager.instance.ChangeScore("Bad");
            isHit = true;
        }

        if (isHit)
        {
            if (!isTap)
            {
                lNoteTrigger[noteMana.LaneNum[noteIdx]] = false;
            }
            ShowScore();
            DeleteTapData(noteIdx);
        }
    }

    private void JudgeHoldTiming(float holdTiming, int laneNum)
    {
        if (Time.time >= holdTiming + GManager.instance.startTime)
        {
            SetHoldMessage(0, laneNum);
            GManager.instance.ChangeScore("Perfect");
            DeleteHoldData(laneNum);
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

    private void JudgeTapMiss(float judgeTime, int noteIdx)
    {
        if (Time.time <= judgeTime)
        {
            return;
        }

        SetTapMessage(3, noteIdx);
        GManager.instance.ChangeScore("Miss");
        ShowScore();

        if (noteMana.NoteType[noteIdx] == 3)
        {
            lNoteTrigger[noteMana.LaneNum[noteIdx]] = false;
        }

        DeleteTapData(noteIdx);
    }

    private void JudgeHoldMiss(int laneNum)
    {
        if (noteMana.holdTiming[laneNum].Count == 0)
        {
            return;
        }

        float limTime = noteMana.holdTiming[laneNum][0] + GManager.instance.startTime;
        if (Time.time > limTime)
        {
            SetHoldMessage(3, laneNum);
            GManager.instance.ChangeScore("Miss");
            ShowScore();
            DeleteHoldData(laneNum);
        }
    }

    private void DeleteTapData(int noteIndx)
    {
        noteMana.NotesTime.RemoveAt(noteIndx);
        noteMana.LaneNum.RemoveAt(noteIndx);
        noteMana.NoteType.RemoveAt(noteIndx);
    }

    private void DeleteHoldData(int laneNum)
    {
        for (int i = 0; i <= 5; ++i)
        {
            if (laneNum == i)
            {
                noteMana.holdTiming[i].RemoveAt(0);
            }
        }
    }

    private void SetTapMessage(int judge, int noteIdx)
    {
        Vector3 messagePos = new Vector3((noteMana.LaneNum[noteIdx] - 2.5f) * 2, -3.5f, 0);
        Instantiate(MessageObj[judge], messagePos, Quaternion.identity);
    }

    private void SetHoldMessage(int judge, int laneNum)
    {
        Debug.Log("メッセージが呼ばれた");
        Vector3 messagePos = new Vector3((laneNum - 2.5f) * 2, -3.5f, 0);
        Instantiate(MessageObj[judge], messagePos, Quaternion.identity);
    }

    private void ShowScore()
    {
        comboText.SetText(GManager.instance.combo.ToString());
        scoreText.SetText(GManager.instance.nominalScore.ToString());
    }
}