using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
//jsonファイルを読み込むための受け皿
public class Data
{
    public string name;
    public int maxBlock;
    public int BPM;
    public int offset;
    public Notes[] notes;
}

[Serializable]
public class Notes
{
    public int type;
    public int num;
    public int block;
    public int LPB;
}


public class NoteManager : MonoBehaviour
{

    public int noteNum; //総ノーツ数
    private string songName;

    public List<int> LaneNum = new List<int>(); //何番のレーンにノーツが落ちてくるか
    public List<int> NoteType = new List<int>(); //何ノーツか
    public List<float> NotesTime = new List<float>(); //ノーツが判定線と重なる時間
    public List<GameObject> nNote = new List<GameObject>(); //ノーマルノーツ
    public List<GameObject> lNote = new List<GameObject>(); //ロングノーツ
    //ロングノーツの長押し判定タイミングを格納。多次元リストは実現困難。
    public List<List<float>> holdTiming = new List<List<float>>();
    public List<float> holdTiming0 = new List<float>();
    public List<float> holdTiming1 = new List<float>();
    public List<float> holdTiming2 = new List<float>();
    public List<float> holdTiming3 = new List<float>();
    public List<float> holdTiming4 = new List<float>();
    public List<float> holdTiming5 = new List<float>();

    private float noteSpeed;
    [SerializeField] float selfOffset;
    [SerializeField] GameObject nNotePrefab; //ノーマルノーツのプレハブ
    [SerializeField] GameObject lNotePrefab; //ロングノーツのプレハブ

    private void Start()
    {
        noteSpeed = GManager.instance.noteSpeed;
        noteNum = 0;
        songName = "Shangri-La of Neko";

        holdTiming.Add(holdTiming0);
        holdTiming.Add(holdTiming1);
        holdTiming.Add(holdTiming2);
        holdTiming.Add(holdTiming3);
        holdTiming.Add(holdTiming4);
        holdTiming.Add(holdTiming5);

        Load(songName);
    }

    private void Load(string SongName)
    {
        //jsonファイルの読み込み
        string inputString = Resources.Load<TextAsset>(SongName).ToString();
        Data inputJson = JsonUtility.FromJson<Data>(inputString);

        //総ノーツ数の設定
        noteNum = inputJson.notes.Length;

        //ノーツの生成

        float timePerBeat = 60f / (float)inputJson.BPM; //1拍(1ビート)にかかる時間
        for (int i = 0; i < noteNum; ++i)
        {
            float beatNum = inputJson.notes[i].num / (float)inputJson.notes[i].LPB; //ノーツが何拍目にあるか。
            float time = beatNum * timePerBeat + selfOffset * 0.01f; ; //ノーツが判定ラインにたどり着くまでに要する時間

            NotesTime.Add(time);
            LaneNum.Add(inputJson.notes[i].block);
            NoteType.Add(inputJson.notes[i].type);

            float y = NotesTime[i] * noteSpeed - 4;

            if (NoteType[i] == 1)
            {
                nNote.Add(Instantiate(nNotePrefab, new Vector3((inputJson.notes[i].block - 2.5f) * 2, y, 0), Quaternion.identity));
            }
            else if (NoteType[i] == 2 || NoteType[i] == 3)
            {
                lNote.Add(Instantiate(lNotePrefab, new Vector3((inputJson.notes[i].block - 2.5f) * 2, y, 0), Quaternion.identity));
            }
        }

        //ロングノーツの長押し部分の判定生成
        for (int i = 0; i < noteNum; ++i)
        {
            if (NoteType[i] != 2)
            {
                continue;
            }

            Debug.Log("ロングノーツの先端発見");
            for (int j = i + 1; j < noteNum; ++j)
            {
                if (NoteType[j] != 3 || LaneNum[j] != LaneNum[i])
                {
                    continue;
                }

                Debug.Log("終点発見");
                int buttonDownArea = inputJson.notes[j].num - inputJson.notes[i].num;
                for (int k = 1; k < buttonDownArea; ++k)
                {
                    float beatNumL = (inputJson.notes[i].num + k) / (float)inputJson.notes[i].LPB;
                    float timeL = beatNumL * timePerBeat + selfOffset * 0.01f;
                    holdTiming[LaneNum[i]].Add(timeL);
                }
            }
        }

        //実質Maxスコアの設定
        int holdTimingAllCount = 0;
        for (int i = 0; i <= 5; ++i)
        {
            holdTimingAllCount += holdTiming[i].Count;
        }
        Debug.Log(holdTimingAllCount);
        GManager.instance.realMaxScore = (noteNum + holdTimingAllCount) * 5;
    }
}
