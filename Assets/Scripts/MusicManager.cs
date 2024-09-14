using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MusicManager : MonoBehaviour
{
    [Header("アタッチ&パラメータ")]
    [SerializeField] NoteManager noteMana;
    [SerializeField] string songName;
    [SerializeField] Image startPanel;
    [SerializeField] TextMeshProUGUI songNameTx;

    private AudioSource audioS;
    private AudioClip music;

    [HideInInspector] public bool isPlayed;

    // Start is called before the first frame update
    void Start()
    {
        audioS = GetComponent<AudioSource>();
        music = (AudioClip)Resources.Load("Musics/" + songName);
        audioS.clip = music;
        isPlayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isPlayed)
        {
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        isPlayed = true;
        startPanel.DOFade(0, 1);
        songNameTx.DOFade(0, 1);

        yield return new WaitForSeconds(1);

        GManager.instance.isStart = true;
        GManager.instance.startTime = Time.time;

        yield return new WaitForSeconds(noteMana.startOffset * noteMana.timePerBeat);

        audioS.Play();
        yield break;
    }
}
