using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeChanger : MonoBehaviour
{
    [SerializeField] NoteManager noteMana;
    [SerializeField] JudgeManager judgeMana;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject[] countDownObj = new GameObject[4];
    [SerializeField] float[] turningBeatPoint = new float[2];
    private bool[] isModeChange = new bool[2] {false, false}; //���[�h�`�F���W�ɓ�������t���O�𗧂Ă�

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CheckModeChange(true, turningBeatPoint[0]))
        {
            isModeChange[0] = true;
            StartCoroutine(ChangeMode(turningBeatPoint[0]));
        }

        if (CheckModeChange(false, turningBeatPoint[1]))
        {
            isModeChange[1] = true;
            StartCoroutine(ChangeMode(turningBeatPoint[1]));
        }
    }

    private bool CheckModeChange(bool isFirst, float turningBeatPoint)
    {
        if (isFirst)
        {
            if (isModeChange[0])
            {
                return false;
            }
        }
        else
        {
            if (!isModeChange[0] || isModeChange[1])
            {
                return false;
            }
        }

        float triggerTime = noteMana.timePerBeat * (turningBeatPoint - 3f) + GManager.instance.startTime;
        if (Time.time < triggerTime)
        {
            return false;
        }

        return true;
    }

    private IEnumerator ChangeMode(float turningBeatPoint)
    {
        int countNum = 4;
        for (int i = 0; i < countNum; ++i)
        {
            GameObject countDownPref = (GameObject)Instantiate(countDownObj[i]);
            countDownPref.transform.SetParent(canvas.transform, false);

            if (i == 3)
            {
                if (judgeMana.isNormal)
                {
                    Debug.Log("���[�h�`�F���W");
                    judgeMana.isNormal = false;
                }
                else
                {
                    Debug.Log("���[�h�`�F���W");
                    judgeMana.isNormal = true;
                }
            }

            yield return new WaitForSeconds(noteMana.timePerBeat);

            Destroy(countDownPref);
        }
    }
}
