using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Judge : MonoBehaviour
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

        if (Time.time > noteMana.NotesTime[0] + 0.15f + GManager.instance.startTime)
        {
            Message(3);
            GManager.instance.ChangeScore("Miss");
            ShowScore();
            DeleteData();
        }
    }

    private void JudgeInput(KeyCode keyCode, int laneNum)
    {

        if (Input.GetKeyDown(keyCode))
        {
            if (noteMana.LaneNum[0] == laneNum)
            {
                JudgeTiming(GetABS(Time.time - (noteMana.NotesTime[0] + GManager.instance.startTime)));
            }
        }
    }

    private void JudgeTiming(float timeLag)
    {
        if (timeLag <= 0.05f)
        {
            Debug.Log("Perfect");
            Message(0);
            GManager.instance.ChangeScore("Perfect");
            ShowScore();
            DeleteData();
        }
        else if (timeLag <= 0.1f)
        {
            Debug.Log("Great");
            Message(1);
            GManager.instance.ChangeScore("Great");
            ShowScore();
            DeleteData();
        }
        else if (timeLag <= 0.15f)
        {
            Debug.Log("Bad");
            Message(2);
            GManager.instance.ChangeScore("Bad");
            ShowScore();
            DeleteData();
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

    private void DeleteData()
    {
        noteMana.NotesTime.RemoveAt(0);
        noteMana.LaneNum.RemoveAt(0);
        noteMana.NoteType.RemoveAt(0);
    }

    private void Message(int judge)
    {
        Instantiate(MessageObj[judge], new Vector2((noteMana.LaneNum[0] - 2.5f) * 2, -3.5f), Quaternion.identity);
    }

    private void ShowScore()
    {
        comboText.SetText(GManager.instance.combo.ToString());
        scoreText.SetText(GManager.instance.nominalScore.ToString());
    }
}
