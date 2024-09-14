using System;
using System.Collections.Generic;
using UnityEngine;

//json�t�@�C����ǂݍ��ނ��߂̎󂯎M
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
    [Header("�A�^�b�`&�p�����[�^")]
    [SerializeField] FadeManager fadeMana;
    [SerializeField] GameObject nNotePrefab; //�m�[�}���m�[�c�̃v���n�u
    [SerializeField] GameObject lNotePrefab; //�����O�m�[�c�̃v���n�u
    [SerializeField] string songName;
    public float startOffset;
    [SerializeField] float selfOffset;

    [Space(15)]

    [Header("�m�[�c���")]
    public int noteNum; //���m�[�c��
    public List<int> LaneNum = new List<int>(); //���Ԃ̃��[���Ƀm�[�c�������Ă��邩
    public List<int> NoteType = new List<int>(); //1:�m�[�}���m�[�c, 2:�z�[���h�m�[�c, 3:�����O�m�[�c�̎n�_, 4:�����O�m�[�c�̏I�_
    public List<float> NotesTime = new List<float>(); //�m�[�c��������Əd�Ȃ鎞��
    public List<GameObject> nNote = new List<GameObject>(); //�m�[�}���m�[�c
    public List<GameObject> lNote3 = new List<GameObject>(); //�����O�m�[�c�̎n�_
    public List<GameObject> lNote4 = new List<GameObject>(); //�����O�m�[�c�̏I�_

    //�����O�m�[�c�n�_�^�C�v�ԍ����������ɗp����
    private bool[] laneChecker = new bool[6] { false, false, false, false, false, false };

    private float noteSpeed;
    [HideInInspector] public float timePerBeat;

    //�ȏI�����Ɏg�p
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
        //json�t�@�C���̓ǂݍ���
        string inputString = Resources.Load<TextAsset>(SongName).ToString();
        Data inputJson = JsonUtility.FromJson<Data>(inputString);

        //���m�[�c���̐ݒ�
        noteNum = inputJson.notes.Length;

        Array.Sort(inputJson.notes, (a, b) => a.num - b.num);

        timePerBeat = 60f / (float)inputJson.BPM; //1��(1�r�[�g)�ɂ����鎞��

        //�m�[�c�̎��Ԍv�Z
        for (int i = 0; i < noteNum; ++i)
        {
            float beatNum = inputJson.notes[i].num / (float)inputJson.notes[i].LPB; //�m�[�c�������ڂɂ��邩�B
            float time = (beatNum + startOffset) * timePerBeat + selfOffset * 0.01f; ; //�m�[�c�����胉�C���ɂ��ǂ蒅���܂łɗv���鎞��

            NotesTime.Add(time);
            LaneNum.Add(inputJson.notes[i].block);
            NoteType.Add(inputJson.notes[i].type);

            //�����O�m�[�c�n�_�̃^�C�v��������
            if (NoteType[i] == 2 && !laneChecker[LaneNum[i]])
            {
                NoteType[i] = 3;
                laneChecker[LaneNum[i]] = true;
            }
            else if(NoteType[i] == 4 && laneChecker[LaneNum[i]])
            {
                laneChecker[LaneNum[i]] = false;
            }

            //�m�[�c�̐���
            float y = NotesTime[i] * noteSpeed - 4; //�Ȃ�4�����Ă�H
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

        //�����O�m�[�c�̑ѕ�������
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
                    //���[�J�����W��p���Ȃ��ƈړ��ł��Ȃ�
                    Vector3 startPos = new Vector3(0, 0, 0);
                    float endPosY = NotesTime[j] * noteSpeed - 4;
                    //�v���n�u��Y�X�P�[����0.4�̂��߁A2.5��������1�ɖ߂��Ă���
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
