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

    public int noteNum; //���m�[�c��
    private string songName;

    public List<int> LaneNum = new List<int>(); //���Ԃ̃��[���Ƀm�[�c�������Ă��邩
    public List<int> NoteType = new List<int>(); //���m�[�c��
    public List<float> NotesTime = new List<float>(); //�m�[�c��������Əd�Ȃ鎞��
    public List<GameObject> nNote = new List<GameObject>(); //�m�[�}���m�[�c
    public List<GameObject> lNote = new List<GameObject>(); //�����O�m�[�c
    //�����O�m�[�c�̒���������^�C�~���O���i�[�B���������X�g�͎�������B
    public List<List<float>> holdTiming = new List<List<float>>();
    public List<float> holdTiming0 = new List<float>();
    public List<float> holdTiming1 = new List<float>();
    public List<float> holdTiming2 = new List<float>();
    public List<float> holdTiming3 = new List<float>();
    public List<float> holdTiming4 = new List<float>();
    public List<float> holdTiming5 = new List<float>();

    private float noteSpeed;
    [SerializeField] float selfOffset;
    [SerializeField] GameObject nNotePrefab; //�m�[�}���m�[�c�̃v���n�u
    [SerializeField] GameObject lNotePrefab; //�����O�m�[�c�̃v���n�u

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
        //json�t�@�C���̓ǂݍ���
        string inputString = Resources.Load<TextAsset>(SongName).ToString();
        Data inputJson = JsonUtility.FromJson<Data>(inputString);

        //���m�[�c���̐ݒ�
        noteNum = inputJson.notes.Length;

        //�m�[�c�̐���

        float timePerBeat = 60f / (float)inputJson.BPM; //1��(1�r�[�g)�ɂ����鎞��
        for (int i = 0; i < noteNum; ++i)
        {
            float beatNum = inputJson.notes[i].num / (float)inputJson.notes[i].LPB; //�m�[�c�������ڂɂ��邩�B
            float time = beatNum * timePerBeat + selfOffset * 0.01f; ; //�m�[�c�����胉�C���ɂ��ǂ蒅���܂łɗv���鎞��

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

        //�����O�m�[�c�̒����������̔��萶��
        for (int i = 0; i < noteNum; ++i)
        {
            if (NoteType[i] != 2)
            {
                continue;
            }

            Debug.Log("�����O�m�[�c�̐�[����");
            for (int j = i + 1; j < noteNum; ++j)
            {
                if (NoteType[j] != 3 || LaneNum[j] != LaneNum[i])
                {
                    continue;
                }

                Debug.Log("�I�_����");
                int buttonDownArea = inputJson.notes[j].num - inputJson.notes[i].num;
                for (int k = 1; k < buttonDownArea; ++k)
                {
                    float beatNumL = (inputJson.notes[i].num + k) / (float)inputJson.notes[i].LPB;
                    float timeL = beatNumL * timePerBeat + selfOffset * 0.01f;
                    holdTiming[LaneNum[i]].Add(timeL);
                }
            }
        }

        //����Max�X�R�A�̐ݒ�
        int holdTimingAllCount = 0;
        for (int i = 0; i <= 5; ++i)
        {
            holdTimingAllCount += holdTiming[i].Count;
        }
        Debug.Log(holdTimingAllCount);
        GManager.instance.realMaxScore = (noteNum + holdTimingAllCount) * 5;
    }
}
