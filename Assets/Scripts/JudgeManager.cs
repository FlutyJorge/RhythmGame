using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JudgeManager : MonoBehaviour
{
    [Header("アッタッチ&パラメータ")]
    public NoteManager noteMana;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject[] messageObj;
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] TextMeshProUGUI scoreText;

    [HideInInspector] public bool isNormal = true;

    private JudgeRunnerN jRunnerN;
    private JudgeRunnerS jRunnerS;

    private void Start()
    {
        jRunnerN = GetComponent<JudgeRunnerN>();
        jRunnerS = GetComponent<JudgeRunnerS>();
    }

    private void Update()
    {
        if (!GManager.instance.isStart || noteMana.NotesTime.Count == 0)
        {
            return;
        }


        if (isNormal)
        {
            for (int i = 0; i < 6; ++i)
            {
                jRunnerN.JudgeTap(i);
                jRunnerN.JudgeHold(i);
                jRunnerN.JudgeUp(i);
            }
        }
        else
        {
            for (int i = 0; i < 6; ++i)
            {
                if (i < 3)
                {
                    jRunnerS.JudgeTap(i, true);
                    jRunnerS.JudgeHold(i, true);
                    jRunnerS.JudgeUp(i, true);
                }
                else
                {
                    jRunnerS.JudgeTap(i, false);
                    jRunnerS.JudgeHold(i, false);
                    jRunnerS.JudgeUp(i, false);
                }
            }
        }

        for (int i = 0; i < 6; ++i)
        {
            if (noteMana.NotesTime.Count <= i)
            {
                break;
            }

            JudgeMiss(i);
        }
    }

    public float GetABS(float num)
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

    public void JudgeTiming(float timeLag, int noteIdx)
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

    public void JudgeHoldTiming(float judgeTime, int noteIdx)
    {
        if (Time.time >= judgeTime + GManager.instance.startTime)
        {
            SetMessage(0, noteIdx);
            GManager.instance.ChangeScore("Perfect");
            DeleteData(noteIdx);
        }
    }

    public void ShowScore()
    {
        comboText.SetText(GManager.instance.combo.ToString());
        scoreText.SetText(GManager.instance.nominalScore.ToString());
    }

    public void SetMessage(int messageIdx, int noteIdx)
    {
        GameObject messagePref = (GameObject)Instantiate(messageObj[messageIdx]);
        messagePref.transform.SetParent(canvas.transform, true);

        //ワールド座標でx座標を指定。y座標はAnimationのRectTransformで指定。
        Vector3 messagePos = new Vector3((noteMana.LaneNum[noteIdx] - 2.5f) * 2, 0, 0);
        messagePref.transform.position = messagePos;
    }

    public void DeleteData(int noteIndx)
    {
        noteMana.NotesTime.RemoveAt(noteIndx);
        noteMana.LaneNum.RemoveAt(noteIndx);
        noteMana.NoteType.RemoveAt(noteIndx);
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
}
