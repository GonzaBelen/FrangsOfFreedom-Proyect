using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoChanger : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    //[SerializeField] private VideoClip attack;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        //ChangeVideo();
    }

    public void ChangeVideo(VideoClip clip)
    {
        if (videoPlayer.clip != null)
        {
            videoPlayer.clip = clip;
        } else
        {
            videoPlayer.clip = null;
            videoPlayer.clip = clip;
        }
    }

    public void PauseClip()
    {
        videoPlayer.Pause();
    }

    public void PlayClip()
    {
        videoPlayer.Play();
    }
}
