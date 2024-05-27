using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioS;
    private AudioClip music;
    private string songName;
    public bool isPlayed;

    // Start is called before the first frame update
    void Start()
    {
        songName = "Shangri-La of Neko";
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
            Debug.Log("Song");
            GManager.instance.isStart = true;
            GManager.instance.startTime = Time.time;
            isPlayed = true;
            audioS.Play();
        }

        //if (!isPlayed)
        //{
        //    Debug.Log("Song");
        //    GManager.instance.isStart = true;
        //    GManager.instance.startTime = Time.time;
        //    isPlayed = true;
        //    audioS.Play();
        //}
    }
}
