using System;
using System.Collections.Generic;
using UnityEngine;

//jsonファイルを読み込むための受け皿
[Serializable]
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
    [Header("アタッチ&パラメータ")]
    [SerializeField] FadeManager fadeMana;
    [SerializeField] GameObject nNotePrefab; //ノーマルノーツのプレハブ
    [SerializeField] GameObject lNotePrefab; //ロングノーツのプレハブ
    [SerializeField] string songName;
    public float startOffset;
    [SerializeField] float selfOffset;

    [Space(15)]

    [Header("ノーツ情報")]
    public int noteNum; //総ノーツ数
    public List<int> LaneNum = new List<int>(); //何番のレーンにノーツが落ちてくるか
    public List<int> NoteType = new List<int>(); //1:ノーマルノーツ, 2:ホールドノーツ, 3:ロングノーツの始点, 4:ロングノーツの終点
    public List<float> NotesTime = new List<float>(); //ノーツが判定線と重なる時間
    public List<GameObject> nNote = new List<GameObject>(); //ノーマルノーツ
    public List<GameObject> lNote3 = new List<GameObject>(); //ロングノーツの始点
    public List<GameObject> lNote4 = new List<GameObject>(); //ロングノーツの終点

    //ロングノーツ始点タイプ番号書き換えに用いる
    private bool[] laneChecker = new bool[6] { false, false, false, false, false, false };

    private float noteSpeed;
    [HideInInspector] public float timePerBeat;

    //曲終了時に使用
    private bool isSongEnd = false;

    private void Awake()
    {
        noteSpeed = GManager.instance.noteSpeed;
        noteNum = 0;

        Load(songName);
    }

    private void Update()
    {
        if (!isSongEnd && NotesTime.Count == 0)
        {
            isSongEnd = true;
            StartCoroutine(fadeMana.FadeAndChangeScene(5f, 0));
        }
    }

    private void Load(string SongName)
    {
        //jsonファイルの読み込み
        string inputString = Resources.Load<TextAsset>(SongName).ToString();
        Data inputJson = JsonUtility.FromJson<Data>(inputString);

        //総ノーツ数の設定
        noteNum = inputJson.notes.Length;

        Array.Sort(inputJson.notes, (a, b) => a.num - b.num);

        timePerBeat = 60f / (float)inputJson.BPM; //1拍(1ビート)にかかる時間

        //ノーツの時間計算
        for (int i = 0; i < noteNum; ++i)
        {
            float beatNum = inputJson.notes[i].num / (float)inputJson.notes[i].LPB; //ノーツが何拍目にあるか。
            float time = (beatNum + startOffset) * timePerBeat + selfOffset * 0.01f; ; //ノーツが判定ラインにたどり着くまでに要する時間

            NotesTime.Add(time);
            LaneNum.Add(inputJson.notes[i].block);
            NoteType.Add(inputJson.notes[i].type);

            //ロングノーツ始点のタイプ書き換え
            if (NoteType[i] == 2 && !laneChecker[LaneNum[i]])
            {
                NoteType[i] = 3;
                laneChecker[LaneNum[i]] = true;
            }
            else if(NoteType[i] == 4 && laneChecker[LaneNum[i]])
            {
                laneChecker[LaneNum[i]] = false;
            }

            //ノーツの生成
            float y = NotesTime[i] * noteSpeed - 4; //なぜ4引いてる？
            if (NoteType[i] == 1)
            {
                nNote.Add(Instantiate(nNotePrefab, new Vector3((LaneNum[i] - 2.5f) * 2, y, 0), Quaternion.identity));
            }
            else if (NoteType[i] == 3)
            {
                lNote3.Add(Instantiate(lNotePrefab, new Vector3((LaneNum[i] - 2.5f) * 2, y, 0), Quaternion.identity));
            }
            else if (NoteType[i] == 4)
            {
                lNote4.Add(Instantiate(lNotePrefab, new Vector3((LaneNum[i] - 2.5f) * 2, y, 0), Quaternion.identity));
            }
        }

        //ロングノーツの帯部分生成
        int lNote3Counter = 0;
        for (int i = 0; i < noteNum; ++i)
        {
            if (NoteType[i] != 3)
            {
                continue;
            }

            GameObject startObj = lNote3[lNote3Counter];
            LineRenderer lineRen = startObj.GetComponent<LineRenderer>();
            for (int j = i; j < noteNum; ++j)
            {
                if (NoteType[j] == 4 && LaneNum[j] == LaneNum[i])
                {
                    //ローカル座標を用いないと移動できない
                    Vector3 startPos = new Vector3(0, 0, 0);
                    float endPosY = NotesTime[j] * noteSpeed - 4;
                    //プレハブのYスケールが0.4のため、2.5をかけて1に戻している
                    Vector3 endPos = (new Vector3(0, endPosY, 0) - new Vector3(0, startObj.transform.position.y, 0)) * 2.5f;

                    lineRen.SetPosition(0, startPos);
                    lineRen.SetPosition(1, endPos);
                    ++lNote3Counter;
                    break;
                }
            }
        }

        GManager.instance.realMaxScore = noteNum * 5;
    }
}
