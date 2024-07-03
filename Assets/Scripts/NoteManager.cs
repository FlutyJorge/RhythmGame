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
    public List<int> NoteType = new List<int>(); //1:ノーマルノーツ, 2:ホールドノーツ, 3:ロングノーツの始点, 4:ロングノーツの終点
    public List<float> NotesTime = new List<float>(); //ノーツが判定線と重なる時間
    public List<GameObject> nNote = new List<GameObject>(); //ノーマルノーツ
    public List<GameObject> lNote3 = new List<GameObject>(); //ロングノーツの始点
    public List<GameObject> lNote4 = new List<GameObject>(); //ロングノーツの終点

    private float noteSpeed;
    [SerializeField] float selfOffset;
    [SerializeField] GameObject nNotePrefab; //ノーマルノーツのプレハブ
    [SerializeField] GameObject lNotePrefab; //ロングノーツのプレハブ

    private void Start()
    {
        noteSpeed = GManager.instance.noteSpeed;
        noteNum = 0;
        songName = "Shangri-La of Neko";

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
