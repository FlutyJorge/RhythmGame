using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
    [SerializeField] GameObject[] MessageObj;
    [SerializeField] NotesManager notesMana;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GManager.instance.isStart || notesMana.NotesTime.Count == 0)
        {
            return;
        }

        JudgeInput(KeyCode.D, 0);
        JudgeInput(KeyCode.F, 1);
        JudgeInput(KeyCode.G, 2);
        JudgeInput(KeyCode.H, 3);
        JudgeInput(KeyCode.J, 4);
        JudgeInput(KeyCode.K, 5);

        if (Time.time > notesMana.NotesTime[0] + 0.15f + GManager.instance.startTime)
        {
            Message(3);
            DeleteData();
            Debug.Log("サイテー");
        }
    }

    private void JudgeInput(KeyCode keyCode, int laneNum)
    {

        if (Input.GetKeyDown(keyCode))
        {
            if (notesMana.LaneNum[0] == laneNum)
            {
                JudgeTiming(GetABS(Time.time - (notesMana.NotesTime[0] + GManager.instance.startTime)));
            }
        }
    }

    private void JudgeTiming(float timeLag)
    {
        if (timeLag <= 0.05f)
        {
            Debug.Log("Perfect");
            Message(0);
            DeleteData();
        }
        else if (timeLag <= 0.1f)
        {
            Debug.Log("Great");
            Message(1);
            DeleteData();
        }
        else if (timeLag <= 0.15f)
        {
            Debug.Log("Good");
            Message(2);
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
        notesMana.NotesTime.RemoveAt(0);
        notesMana.LaneNum.RemoveAt(0);
        notesMana.NoteType.RemoveAt(0);
    }

    void Message(int judge)
    {
        Instantiate(MessageObj[judge], new Vector2((notesMana.LaneNum[0] - 2.5f) * 2, -3.5f), Quaternion.identity);
    }
}
