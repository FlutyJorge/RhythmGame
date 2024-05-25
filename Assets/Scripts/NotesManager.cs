using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
//json�t�@�C����ǂݍ��ނ��߂̎󂯎M
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

    public int noteNum; //���m�[�c��
    private string songName;

    public List<int> LaneNum = new List<int>(); //���Ԃ̃��[���Ƀm�[�c�������Ă��邩
    public List<int> NoteType = new List<int>(); //���m�[�c��
    public List<float> NotesTime = new List<float>(); //�m�[�c��������Əd�Ȃ鎞��
    public List<GameObject> NotesObj = new List<GameObject>(); //�m�[�c����

    [SerializeField] private float notesSpeed;
    [SerializeField] GameObject noteObj; //�m�[�c�̃v���n�u

    private void OnEnable()
    {
        noteNum = 0;
        songName = "Shangri-La of Neko";
        Load(songName);
    }

    private void Load(string SongName)
    {
        //json�t�@�C���̓ǂݍ���
        string inputString = Resources.Load<TextAsset>(SongName).ToString();
        Data inputJson = JsonUtility.FromJson<Data>(inputString);

        //���m�[�c���̐ݒ�
        noteNum = inputJson.notes.Length;

        for (int i = 0; i < noteNum; i++)
        {
            float timePerBeat = 60 / (float)inputJson.BPM; //1��(1�r�[�g)�ɂ����鎞��
            float beatNum = inputJson.notes[i].num / (float)inputJson.notes[i].LPB; //�m�[�c�������ڂɂ��邩
            float time = beatNum * timePerBeat + inputJson.offset * 0.0001f; //�m�[�c�����胉�C���ɂ��ǂ蒅���܂łɗv���鎞��

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
