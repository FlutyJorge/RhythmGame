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

        JudgeInput(KeyCode.D, 0);
        JudgeInput(KeyCode.F, 1);
        JudgeInput(KeyCode.G, 2);
        JudgeInput(KeyCode.H, 3);
        JudgeInput(KeyCode.J, 4);
        JudgeInput(KeyCode.K, 5);

        if (noteMana.NotesTime.Count == 0)
        {
            return;
        }

        float judgeTime1 = noteMana.NotesTime[0] + 0.15f + GManager.instance.startTime;
        JudgeMiss(judgeTime1, 0);

        if (noteMana.NotesTime.Count < 2)
        {
            return;
        }
        float judgeTime2 = noteMana.NotesTime[1] + 0.15f + GManager.instance.startTime;
        JudgeMiss(judgeTime2, 1);
    }

    private void JudgeInput(KeyCode keyCode, int laneIdx)
    {

        if (Input.GetKeyDown(keyCode))
        {
            if (noteMana.LaneNum[0] == laneIdx)
            {
                JudgeTiming(GetABS(Time.time - (noteMana.NotesTime[0] + GManager.instance.startTime)), 0);
                DeleteData(0);
            }
            else if (noteMana.LaneNum[1] == laneIdx)
            {
                JudgeTiming(GetABS(Time.time - (noteMana.NotesTime[1] + GManager.instance.startTime)), 1);
                DeleteData(1);
            }
        }
    }

    private void JudgeTiming(float timeLag, int laneIdx)
    {
        if (timeLag <= 0.05f)
        {
            Debug.Log("Perfect");
            Message(0, laneIdx);
            GManager.instance.ChangeScore("Perfect");
            ShowScore();
        }
        else if (timeLag <= 0.1f)
        {
            Debug.Log("Great");
            Message(1, laneIdx);
            GManager.instance.ChangeScore("Great");
            ShowScore();
        }
        else if (timeLag <= 0.15f)
        {
            Debug.Log("Bad");
            Message(2, laneIdx);
            GManager.instance.ChangeScore("Bad");
            ShowScore();
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

    private void JudgeMiss(float judgeTime, int noteIndx)
    {
        if (Time.time > judgeTime)
        {
            Message(3, noteIndx);
            GManager.instance.ChangeScore("Miss");
            ShowScore();
            DeleteData(noteIndx);
        }
    }

    private void DeleteData(int noteIndx)
    {
        noteMana.NotesTime.RemoveAt(noteIndx);
        noteMana.LaneNum.RemoveAt(noteIndx);
        noteMana.NoteType.RemoveAt(noteIndx);
    }

    private void Message(int judge, int laneIdx)
    {
        Instantiate(MessageObj[judge], new Vector3((noteMana.LaneNum[laneIdx] - 2.5f) * 2, -3.5f, 0), Quaternion.identity);
    }

    private void ShowScore()
    {
        comboText.SetText(GManager.instance.combo.ToString());
        scoreText.SetText(GManager.instance.nominalScore.ToString());
    }
}