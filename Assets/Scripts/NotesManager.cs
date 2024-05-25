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
    public Note[] notes;
}

[Serializable]
public class Note
{
    public int type;
    public int num;
    public int block;
    public int LPB;
}


public class NotesManager : MonoBehaviour
{

    public int noteNum; //総ノーツ数
    private string songName;

    public List<int> LaneNum = new List<int>(); //何番のレーンにノーツが落ちてくるか
    public List<int> NoteType = new List<int>(); //何ノーツか
    public List<float> NotesTime = new List<float>(); //ノーツが判定線と重なる時間
    public List<GameObject> NotesObj = new List<GameObject>(); //ノーツ自体

    [SerializeField] private float notesSpeed;
    [SerializeField] GameObject noteObj; //ノーツのプレハブ

    private void OnEnable()
    {
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

        for (int i = 0; i < noteNum; i++)
        {
            float timePerBeat = 60 / (float)inputJson.BPM; //1拍(1ビート)にかかる時間
            float beatNum = inputJson.notes[i].num / (float)inputJson.notes[i].LPB; //ノーツが何拍目にあるか
            float time = beatNum * timePerBeat + inputJson.offset * 0.0001f; //ノーツが判定ラインにたどり着くまでに要する時間

            NotesTime.Add(time);
            LaneNum.Add(inputJson.notes[i].block);
            NoteType.Add(inputJson.notes[i].type);

            float y = NotesTime[i] * notesSpeed - 4;
            NotesObj.Add(Instantiate(noteObj, new Vector2((inputJson.notes[i].block - 2.5f) * 2, y), Quaternion.identity));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
