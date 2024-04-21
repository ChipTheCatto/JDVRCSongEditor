using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LightingEditorVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private bool paused = true;
    public Animator[] pauseIconAnimators;
    public Slider[] videoSliders;
    private bool dragging;
    private int draggingIndex;
    
    
    public void SetUrl(string url)
    {
        videoPlayer.url = url;
        videoPlayer.Play();
        StartCoroutine(PauseAfterShortTime());
    }
    
    private IEnumerator PauseAfterShortTime()
    {
        yield return new WaitForSeconds(0.1f);
        videoPlayer.Pause();
    }
    
    public void PlayPause()
    {
        SetPaused(!paused);
    }


    public void SetPaused(bool value)
    {
        paused = value;
        if (!paused)
        {
            videoPlayer.Play();
            SetPauseIcon(false);
        }
        else
        {
            videoPlayer.Pause();
            SetPauseIcon(true);
        }
    }

    private void SetPauseIcon(bool paused)
    {
        foreach (Animator pauseIconAnimator in pauseIconAnimators)
        {
            pauseIconAnimator.SetBool("Paused", paused);
        }
    }
    
    public void BeginDrag(int index)
    {
        dragging = true;
        draggingIndex = index;
    }
    
    public void EndDrag(int index)
    {
        dragging = false;
        draggingIndex = index;
    }

    public float GetVideoTime()
    {
        return (float)videoPlayer.time;
    }
    
    public float GetVideoLength()
    {
        return (float)videoPlayer.length;
    }

    private void Update()
    {
        if (dragging)
        {
            videoPlayer.time = videoPlayer.length * videoSliders[draggingIndex].value;
        } else
        {
            float v = (float)(videoPlayer.time / videoPlayer.length);
            if (!float.IsNaN(v))
            {
                SetSliders(v);
            }
        }
    }
    
    
    private void SetSliders(float value)
    {
        foreach (Slider slider in videoSliders)
        {
            slider.value = value;
        }
    }
}
