using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;

    [Header("スコア設定")]
    public float nominalMaxScore;
    [HideInInspector] public float realMaxScore;
    [HideInInspector] public float nominalScore;
    [HideInInspector] public float realScore;
    public float perfectScore;
    public float greatScore;
    public float badScore;

    [Space(20)]
    public int songID;
    public float noteSpeed;

    public bool isStart;
    public float startTime;

    public int combo;

    public int perfect;
    public int great;
    public int bad;
    public int miss;


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ChangeScore(string judge)
    {
        if (judge == "Perfect")
        {
            realScore += perfectScore;
            ++perfect;
            ++combo;
        }
        else if (judge == "Great")
        {
            realScore += greatScore;
            ++great;
            ++combo;
        }
        else if (judge == "Bad")
        {
            realScore += badScore;
            ++bad;
            combo = 0;
        }
        else if (judge == "Miss")
        {
            ++miss;
            combo = 0;
        }
        else
        {
            Debug.Log("判定エラー");
        }

        nominalScore = (int)Mathf.Floor(realScore / realMaxScore * nominalMaxScore);
    }

    public void UpdateScore()
    {
        nominalScore = (int)Mathf.Floor(realScore / realMaxScore * nominalMaxScore);
    }
}
